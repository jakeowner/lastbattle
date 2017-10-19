---altar func----

-------------------------------------------------------------祭坛功能-----------------------------------------------------
local gAltarBornNPC = {
  [1003] ={},
  [1002] ={},
  [1001] ={},
  [1000] ={},
};
--添加祭坛小兵--
local function AddAltarNPC(mapId,battleIdx,altarIdex, solderType, campId)
  local bFlag = true
  if nil == gAltarBornNPC[mapId][battleIdx] then

    gAltarBornNPC[mapId][battleIdx] = {}
    gAltarBornNPC[mapId][battleIdx][altarIdex] ={}
    gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"] = 1

    bFlag = false
  elseif nil == gAltarBornNPC[mapId][battleIdx][altarIdex]  then
    gAltarBornNPC[mapId][battleIdx][altarIdex] ={}
    gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"] = 1
    bFlag = false
  end

  gAltarBornNPC[mapId][battleIdx][altarIdex]["solderType"] = solderType
  gAltarBornNPC[mapId][battleIdx][altarIdex]["campId"] = campId

  if bFlag then
    gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"] = gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"] + 1
  end
end;

--获取--
function GetAltarNPCIdx(mapId,battleIdx,altarIdex)
  if nil == gAltarBornNPC[mapId][battleIdx] then
    return nil
  end

  if nil == gAltarBornNPC[mapId][battleIdx][altarIdex] then
    return nil
  end

  local soldeType = gAltarBornNPC[mapId][battleIdx][altarIdex]["solderType"]
  local  campId = gAltarBornNPC[mapId][battleIdx][altarIdex]["campId"]
  local count = gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"]
  return  soldeType,campId, count
end;
--SaveCount--
local function ChangeAltarNPCNum(mapId,battleIdx,altarIdex,num)
  if nil ~= gAltarBornNPC[mapId][battleIdx][altarIdex] then
    gAltarBornNPC[mapId][battleIdx][altarIdex]["SaveCount"] = num
  end
end;

function  GetAltarNPCICO(mapId, battleId)
  if nil == gAltarBornNPC[mapId][battleId] then
    return nil
  end
  local altarTable = gAltarBornNPC[mapId][battleId]

  for idx, other in pairs(altarTable) do
      ThisBattle:RefreshAltarIcoMsg(battleId,idx,other["solderType"])
  end
end;
function RemoveAltarNPCBattleIdx(mapId,battleIdx)
  if nil ~= gAltarBornNPC[mapId][battleIdx] then
    gAltarBornNPC[mapId][battleIdx] = nil 
  end
end;


--吸附小兵
function AltarAddBornSolderBySolderIdex(mapId,battleIdx,altarIdex, solderType, campId)
  ----print("00000000AltarAddBornSolderBySolderIdex,",mapId,battleIdx,altarIdex, solderType, campId)
  if altarIdex == nil or MapBattleCfg[mapId].AltarBornSolderInfo[altarIdex] == nil   then
    ThisBattle:LogDebug("-------------error. altarIdex is null or invalid altarIdex!------------",altarIdex,solderType,campId,areaId)
    return
  end

  local bFlag = CheckMapBattleIdx(mapId,battleIdx)
  if bFlag == false then
    return
  end

  AddAltarNPC(mapId,battleIdx,altarIdex, solderType, campId)
end;

--生产小兵
local function DoAltarBornSolderByIdex(mapId,battleIdx,altarIdx)
  ----print("--1---DoAltarBornSolderByIdex,",mapId,battleIdx,altarIdx)
  local localTemp = MapBattleCfg[mapId].AltarBornSolderInfo[altarIdx]

  local pathNum = localTemp.AltarPathNum
  local bornSolderTable = localTemp.AltarBornSolderPos
  local bornSolderDirTable = localTemp.AltarBornSolderDir

  local BornSolderType,bornSolderCamp, count = GetAltarNPCIdx(mapId,battleIdx,altarIdx)

  if #bornSolderTable ~= #bornSolderDirTable then
    ThisBattle:LogDebug("------------error, born pos  <> born dir")
    return
  end

  if nil ~= bornSolderTable or nil ~= BornSolderType then
    local index = 1
    for _,k in pairs(bornSolderTable) do

      local ditTable = bornSolderDirTable[index]
      if nil == ditTable then
        ThisBattle:LogDebug("-----------error, DoAltarBornSolderByIdex bornSolderDirTable["..index.."] is nil")
        return
      end

      local un64ObjIdx = ThisBattle:AddAltarVMObject(battleIdx,altarIdx,BornSolderType, bornSolderCamp, k, ditTable)

      if  0 < un64ObjIdx   then
        ThisBattle:StartOccupyOrder(battleIdx,un64ObjIdx,0, MapBattleLinePathCfg[mapId][pathNum])
      else
        ThisBattle:LogDebug("OnBornNPC() "..battleIdx.." ObjIdx "..un64ObjIdx.." Obj is Nil")
        return
      end

	 --出生特效--
      ThisBattle:SendBornSolderMsg(battleIdx,un64ObjIdx,BornSolderType ,bornSolderCamp, k )

      --设置该祭坛当前出兵数量
      --v.CurtBornSolderNum =  v.CurtBornSolderNum + 1

      index = index + 1
    end
  end

  if count < 2 then
    ThisBattle:SendBornObjSoundMsg(battleIdx,BornSolderType)
  end

  ChangeAltarNPCNum(mapId,battleIdx,altarIdx,count + 1)
end;

--生产小兵
function AltarBornSolderByIdex(mapId, battleIdx, altarIdx)

  local bFlag = CheckMapBattleIdx(mapId,battleIdx)
  if bFlag == false then
    return
  end
  DoAltarBornSolderByIdex(mapId,battleIdx,altarIdx)
end;