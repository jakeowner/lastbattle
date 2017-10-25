---wild monster born func----


--记录新手引导野怪出生次数--
local MapGameGuideWMBornNum = {
  [1003] = {},
  [1002] = {},
  [1001] = {},
  [1000] = {},
};
--获取--
function GetVMBornNum(mapId,battleIdx,stepId)
  local num = 0
  if nil ~= stepId then
    if nil == MapGameGuideWMBornNum[mapId][battleIdx]   then
      MapGameGuideWMBornNum[mapId][battleIdx] = {}
      MapGameGuideWMBornNum[mapId][battleIdx][stepId] = {}
      MapGameGuideWMBornNum[mapId][battleIdx][stepId] = num
      --return num
    elseif  nil == MapGameGuideWMBornNum[mapId][battleIdx][stepId] then
      MapGameGuideWMBornNum[mapId][battleIdx][stepId] = {}
      MapGameGuideWMBornNum[mapId][battleIdx][stepId] = num
    elseif 	nil ~= MapGameGuideWMBornNum[mapId][battleIdx][stepId] then
      num = MapGameGuideWMBornNum[mapId][battleIdx][stepId]
    end
  else
    if nil == MapGameGuideWMBornNum[mapId][battleIdx]   then
      MapGameGuideWMBornNum[mapId][battleIdx] = {}
      MapGameGuideWMBornNum[mapId][battleIdx]["count"] = num
    else
      num = 	MapGameGuideWMBornNum[mapId][battleIdx]["count"]
    end
  end

  return num
end;

--改变--
function ChangeGuideVMBornNum(mapId,battleIdx,stepId, num)

  if nil == MapGameGuideWMBornNum[mapId][battleIdx] then

    MapGameGuideWMBornNum[mapId][battleIdx] = {}
    MapGameGuideWMBornNum[mapId][battleIdx][stepId] = {}

    table.insert(MapGameGuideWMBornNum[mapId][battleIdx][stepId], num)
  else
    MapGameGuideWMBornNum[mapId][battleIdx][stepId] = num
  end
end;

--删除BattleId
function RemoveVMBornNum(mapId,battleIdx)
  if nil ~= MapGameGuideWMBornNum[mapId][battleIdx] then
    MapGameGuideWMBornNum[mapId][battleIdx] = nil
   -- table.remove(MapGameGuideWMBornNum[mapId], battleIdx)
	--table.removeObj(MapGameGuideWMBornNum[mapId], battleIdx)
  end
end;

--普通野怪
local function NormalBornWMById(mapId,battleIdx,pointId)

  local bFlag = CheckMapBattleIdx(mapId,battleIdx)
  if bFlag == false then
    return
  end
  if 	nil == pointId or pointId < 1 then
    ThisBattle:LogDebug("--pointId is nil", pointId)
    return
  end
  local monsterIdexLen = #MapBattleCfg[mapId].WMonsterFunc[pointId].monsterIdex

  --print("NormalBornWMById",mapId,battleIdx,pointId,monsterIdexLen)

  local i=1
  while( i <= MapBattleCfg[mapId].WMonsterFunc[pointId].monsterBornNum ) do

    local randSed = math.random(1, monsterIdexLen)
    local getGroupId = MapBattleCfg[mapId].WMonsterFunc[pointId].nGroupId
    local getMonsterIdex = MapBattleCfg[mapId].WMonsterFunc[pointId].monsterIdex[randSed]
    local getMonsterBornPos = MapBattleCfg[mapId].WMonsterFunc[pointId].bornPos[i]
    local getMonsterBornDir = MapBattleCfg[mapId].WMonsterFunc[pointId].bornDir[i]
    local getMonsterIsPatrol = MapBattleCfg[mapId].WMonsterFunc[pointId].isPatrol[i]
    local getMonsterPatrolPos = MapBattleCfg[mapId].WMonsterFunc[pointId].patrolPos
    local getPatrolCdTime = MapBattleCfg[mapId].WMonsterFunc[pointId].patrolCDTime
    local getPatrolRadius = MapBattleCfg[mapId].WMonsterFunc[pointId].radius[i]


    local un64ObjIdx =  ThisBattle:AddVMObject(battleIdx,
      getMonsterIdex,
      MapBattleCfg[mapId].WMonsterFunc[pointId].camp,
      pointId,
      getPatrolRadius,
      getGroupId,
      getMonsterBornPos,
      getMonsterBornDir
    )

    if  0 < un64ObjIdx   then
      ThisBattle:StartGuardOrder(battleIdx,un64ObjIdx,0, getMonsterPatrolPos)--,getPatrolCdTime,getMonsterIsPatrol)

    else
      ThisBattle:LogDebug("OnBornNPC() "..battleIdx.." ObjIdx "..un64ObjIdx.." Obj is Nil")
    end
    i = i+1
  end
end;

--出生野怪
function BornWMById(mapId,battleIdx,stepId )

  if nil == MapBattleBaseCfg[mapId].guideSwitch then
    return NormalBornWMById(mapId,battleIdx,stepId)
  end

  local num = GetVMBornNum(mapId,battleIdx,stepId)
  if num >=  MapBattleCfg[mapId].guideTaskCfg[stepId].bornTime then
    return
  end

  local wildNpcBornCfg = MapBattleCfg[mapId].guideTaskCfg[stepId].wildNPCCfg
  local bornPos = wildNpcBornCfg.bornPosCfg
  local bornDir = wildNpcBornCfg.bornDirCfg
  if nil == bornPos or nil == bornDir then
    ThisBattle:LogDebug("-------------error. wildNpcBornCfg.bornPosCfg if nil-----------")
    return
  end

  if   #bornPos ~= #bornDir then
    ThisBattle:LogDebug("-------------error. number of bornPos ~= number of bornDir-----------")
    return
  end

  local npcPatrolPos = wildNpcBornCfg.patrolPos
  local isNpcPatrol = wildNpcBornCfg.isPatrol
  local patrolRadius = wildNpcBornCfg.radius
  local pointId = stepId
  local bornIdx = wildNpcBornCfg.npcIdex
  local bornIdxLen = #bornIdx
  --print("----bornIdxLen:",bornIdxLen)
  local i = 1
  while i <= bornIdxLen do
    local npcId = bornIdx[i]

    local npcBornPos = bornPos[i]
    local npcDirPos =  bornDir[i]
    local isPatrol = isNpcPatrol[i]
    local patrolPos = npcPatrolPos
    local patrolRadius = patrolRadius[i]
    local npcCamp = wildNpcBornCfg.camp

    if nil == npcId or nil == npcBornPos or nil ==npcDirPos or nil ==isPatrol or nil == patrolPos or nil == patrolRadius then
      ThisBattle:LogDebug("--error:nil == npcId or nil == npcBornPos or nil ==npcDirPos or nil ==isPatrol or nil == patrolPos or nil == patrolRadius-----------"..stepId)
      return
    end

    local un64ObjIdx =  ThisBattle:AddVMObject(battleIdx,
      npcId,
      npcCamp,
      pointId,
      patrolRadius,
      0,
      npcBornPos,
      npcDirPos
    )

    if  0 < un64ObjIdx   then
      ThisBattle:StartGuardOrder(battleIdx,un64ObjIdx,0, patrolPos)
    else
      ThisBattle:LogDebug("--OnBornNPC() "..un64ObjIdx..", ObjIdx "..un64ObjIdx.." Obj is Nil")
    end
    i = i + 1
	 if  nil ~= wildNpcBornCfg.cdTime and nil ~= wildNpcBornCfg.isBornAgain and wildNpcBornCfg.isBornAgain > 0 then
		ThisBattle:SetWMCDCof( mapId,battleIdx, stepId,npcId, wildNpcBornCfg.cdTime,bornIdxLen )
	end
  end


  ChangeGuideVMBornNum(mapId, battleIdx, stepId, num + 1)
end;


