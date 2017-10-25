---battle info---


----------------------------------Battle function------------------
--记录当前战斗idx---
MapGameBattleIdx = {
  [1003] = {},
  [1002] = {},
  [1001] = {},
  [1000] = {},
}; 

-----------------------------------map battle fucntion---------------------
  

--是否存在的battleIdx
function CheckMapBattleIdx(mapId, battleId)
	  local lFlag = CheckMapByMapId(mapId)
	  if lFlag == false then
		return false
	  end

	if MapGameBattleIdx[mapId][battleId] == 	battleId then
		return true
	end
  --ThisBattle:LogDebug("---error, there is no battleId ",battleId)
  return false
end;

--添加battleIdx
function AddMapBattleFunc(mapId, battleId)
 -- print("-1--add battle ---",mapId, battleId)
  local lFlag = CheckMapByMapId(mapId)
  if lFlag == false then
    
	   ThisBattle:AddMapBattleError(mapId,battleId)
    return
  end

  local ifExist = CheckMapBattleIdx(mapId, battleId)
  if ifExist == true then
	ThisBattle:LogDebug("===error, there is the battleid=======",mapId, battleId)
	--ThisBattle:AddMapBattleError(mapId,battleId)
	return
   end
  --table.insert(MapGameBattleIdx[mapId],battleId)
  MapGameBattleIdx[mapId][battleId] = battleId
end;

--删除battleidx
function RemoveBattleIdx(mapId, battleId)
  local lFlag = CheckMapByMapId(mapId)
  if lFlag == false then
    return
  end
 
	MapGameBattleIdx[mapId][battleId] = nil
end;


function onBattleFinish(mapId,battleIdx)
	----print("onBattleFinish begin")
		RemoveBattleIdx(mapId, battleIdx)  
		RemoveAltarNPCBattleIdx(mapId,battleIdx) 
		RemoveBornNPCIdx(mapId, battleIdx)

		RemoveVMBornNum(mapId,battleIdx)

	RemoveSuperNPCBattleId(mapId, battleIdx)

	RemoveBuildPathLv(mapId,battleIdx)
	RemoveCurtBornNPCTimes(mapId,battleIdx )
    --new guide func---
	GuideRemoveHeroNPCBornState(mapId,battleIdx) 
    RemoveGuideTableBattleIdx (mapId,battleIdx )
    RemoveGuideNPCHeroReborn(mapId,battleIdx)
 end



--战斗结束func
function CheckBattleEnd(mapId,battleIdx,un64DeadObjIdx, un64KillerObjIdx)
  local pathCamp = nil
  local endState = false
  for idx, npcId in ipairs(MapBattleEndCfg[mapId]) do
    if npcId == un64DeadObjIdx then
      pathCamp = GetPathCamp(mapId,idx)
      --ThisBattle:GuideFinishBattle(battleIdx,pathCamp,un64KillerObjIdx)
      endState = true
	  break
    end
  end
  return pathCamp,endState
end;


--战斗结束func
function CheckBattleEnd(mapId,battleIdx,un64DeadObjIdx, un64KillerObjIdx)
  local pathCamp = nil
  local endState = false
  for idx, npcId in ipairs(MapBattleEndCfg[mapId]) do
    if npcId == un64DeadObjIdx then
      pathCamp = GetPathCamp(mapId,idx)
      --ThisBattle:GuideFinishBattle(battleIdx,pathCamp,un64KillerObjIdx)
      endState = true
	  break
    end
  end
  return pathCamp,endState
end;

--战斗结束
function OnGameUnitDie(mapId,battleIdx,un64DeadObjIdx, un64KillerObjIdx)
  ----print("-----0--------")
  local bFlag = CheckMapBattleIdx(mapId,battleIdx)
  if bFlag == false then
    ----print("-----end--------")
    return
  end

  local campId, isEndGame = CheckBattleEnd(mapId,battleIdx,un64DeadObjIdx, un64KillerObjIdx)
  ----print(campId, isEndGame )

  if isEndGame == true then
	if mapId == 1000 then
		ThisBattle:LogDebug("---new guide end!",mapId,battleIdx)
		ThisBattle:GuideFinishBattle(battleIdx,campId,un64KillerObjIdx)
    else
		ThisBattle:LogDebug("---battle end !--",mapId,battleIdx)
      	ThisBattle:FinishBattle(battleIdx,campId)
    end
  end
end