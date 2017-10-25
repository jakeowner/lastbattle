--public.lua--
 

---------------------------------------------------------------------新手引导--------------------------------------------------------------
local GuideTableStepId = {
  [1000]={}, 
};

--添加battle stepId--
local function AddGuideStepIdIntoStepTable(mapId,battleIdx,stepId)
  --local ifExist = CheckExistBattleStepId(mapId,battleIdx,stepId)
  if nil == GuideTableStepId[mapId][battleIdx] then
    GuideTableStepId[mapId][battleIdx] = {}
  end
  table.insert(GuideTableStepId[mapId][battleIdx],stepId)
end;

--完成删除stepid
local function RemoveGuideTableStepId (mapId,battleIdx,stepId )
  if  nil ~= GuideTableStepId[mapId] and nil ~= GuideTableStepId[mapId][battleIdx] then
    GuideTableStepId[mapId][battleIdx] = nil 
  end
end;

--战斗结束,删除BattleId
function RemoveGuideTableBattleIdx (mapId,battleIdx )
  if  nil ~= GuideTableStepId[mapId]  and	nil ~= GuideTableStepId[mapId][battleIdx] then
    GuideTableStepId[mapId][battleIdx] = nil 
  end
end;

local gGuideHeroBorn = {
  [1000] = {},
};

--tianjia
local function ChangeGuideHeroNPCBornState(mapId,battleIdx, stepId, stopFlag)

  if nil == gGuideHeroBorn[mapId][battleIdx] then
    gGuideHeroBorn[mapId][battleIdx]={}
    gGuideHeroBorn[mapId][battleIdx][stepId]={}
    gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop = 0
    gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop  = stopFlag
 
  elseif 	nil == gGuideHeroBorn[mapId][battleIdx][stepId] then
    gGuideHeroBorn[mapId][battleIdx][stepId]={}
    gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop = 0
    gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop  = stopFlag
  elseif nil == gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop then
    gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop  = stopFlag
  end
end;

--获取
local function GetGuideHeroNPCBornState(mapId,battleIdx, stepId)
  if nil == gGuideHeroBorn[mapId][battleIdx] then
    gGuideHeroBorn[mapId][battleIdx] = {}
    return 0
  end
  if nil == gGuideHeroBorn[mapId][battleIdx][stepId] then
    gGuideHeroBorn[mapId][battleIdx][stepId] = {}
    return 0
  end

  return gGuideHeroBorn[mapId][battleIdx][stepId].HeroNPStop
end;

--删除
function GuideRemoveHeroNPCBornState(mapId,battleIdx)
  if nil ~= gGuideHeroBorn[mapId] and nil ~= gGuideHeroBorn[mapId][battleIdx] then
    gGuideHeroBorn[mapId][battleIdx] = nil 
  end
end; 


--新手引导野怪重复出生
function  GuideReBornWM(mapId,battleIdx,stepId , npcId)

	local wildNpcBornCfg = guideMapBattleCfg[mapId].guideTaskCfg[stepId].wildNPCCfg
	  local bornPos = wildNpcBornCfg.bornPosCfg
	  local bornDir = wildNpcBornCfg.bornDirCfg

	  local npcPatrolPos = wildNpcBornCfg.patrolPos
	  local isNpcPatrol = wildNpcBornCfg.isPatrol
	  local patrolRadius = wildNpcBornCfg.radius
	  local pointId = stepId
	  local bornIdx = wildNpcBornCfg.npcIdex
	  local bornIdxLen = #bornIdx

	  local i = 1
	  while i <= bornIdxLen do
		--local npcId = bornIdx[i]
		if  npcId == bornIdx[i]  then
			local npcBornPos = bornPos[i]
			local npcDirPos =  bornDir[i]
			local isPatrol = isNpcPatrol[i]
			local patrolPos = npcPatrolPos
			local patrolRadius = patrolRadius[i]
			local npcCamp = wildNpcBornCfg.camp

			local un64ObjIdx =  ThisBattle:AddVMObject(battleIdx,  npcId,  npcCamp,  pointId,  patrolRadius,	  0,  npcBornPos,  npcDirPos)
			if  0 < un64ObjIdx   then
				ThisBattle:StartGuardOrder(battleIdx,un64ObjIdx,0, patrolPos)
			else
				ThisBattle:LogDebug("--OnBornNPC() "..un64ObjIdx..", ObjIdx "..un64ObjIdx.." Obj is Nil")
			end

			 if  nil ~= wildNpcBornCfg.cdTime and nil ~= wildNpcBornCfg.isBornAgain and wildNpcBornCfg.isBornAgain > 0 then
				ThisBattle:SetWMCDCof( mapId,battleIdx, stepId,npcId, wildNpcBornCfg.cdTime,bornIdxLen )
			 end
			 return
		end
		i = i + 1
	  end
end;

--出兵
function OnGuideBornNPC(mapId,battleIdx, stepId)

  if nil ~= stepId and stepId > 0  then
    AddGuideStepIdIntoStepTable(mapId,battleIdx,stepId)
    --print("------add ok ---------",stepId)
  end

  local localTable = GuideTableStepId[mapId][battleIdx]
  if nil == localTable then
    return
  end

  local totalPath = #MapBattleLinePathCfg[mapId]

  for k, v in ipairs(localTable) do

    local stepId = v
    local i = 1
    --print("OnBornNPC("..mapId..","..battleIdx..", "..stepId..")", stepId)
    --npc born
    local localNPCEverySpaceTime = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCEverySpaceTime
    local localNPCEveryBornNum = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCEveryBornNum
    local localNPCspaceTime = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCspaceTime

    while (i <= totalPath ) do

      if nil ~= guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCCfg[i] then

        local localNpcIdxCfg = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCCfg[i].npcIdex
        local localNpcDirCfg = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCCfg[i].npcDir

        local goLinePath = MapBattleLinePathCfg[mapId][i]

        local localNpcIdxCfgLen = #localNpcIdxCfg
        local localNpcDirCfgLen = #localNpcDirCfg

        if localNpcIdxCfgLen ~= localNpcDirCfgLen then
          ThisBattle:LogDebug("localNpcIdxCfgLen != localNpcDirCfgLen"..localNpcIdxCfgLen.."~= ".. localNpcDirCfgLen)
        end

        local localK = 1
        while localK <= localNpcIdxCfgLen do
          local npcBornId = localNpcIdxCfg[localK]
          local npcBornIdDir = localNpcDirCfg[localK]

          if  nil ~= npcBornId and npcBornId > 0  then

            local pathCamp =GetPathCamp(mapId,i)-- MapBattleLinePathCamp[mapId][i]

            local un64ObjIdx = ThisBattle:AddObject(battleIdx, npcBornId, pathCamp ,goLinePath[1], npcBornIdDir )

            if   nil ~= un64ObjIdx  and un64ObjIdx > 0  then
              ThisBattle:StartOccupyOrder(battleIdx,un64ObjIdx,0, goLinePath)
            else
              ThisBattle:LogDebug("--error,-stepId:"..
                stepId..",NpcBornId:"..npcBornId..",pathId:"..i..",n32BornNPCIdx:"..n32BornNPCIdx)
            end
          end

          localK = localK + 1
        end
      end
      i = i + 1
    end

    ChangeBattleNPCBornPortion(mapId,battleIdx, localNPCspaceTime,localNPCEverySpaceTime)

  end

  return 0
end;

local gNPCHeroReborn={
  [1000]={},
};

--添加--
local function GuideAddNPCHeroRebornIdx(mapId,battleIdx, stepId,npcId,un64ObjIdx)
 -- print("--add hero",mapId,battleIdx, stepId,npcId,un64ObjIdx)
  if nil == gNPCHeroReborn[mapId][battleIdx] then
    gNPCHeroReborn[mapId][battleIdx]={}
    gNPCHeroReborn[mapId][battleIdx][stepId]={}
    gNPCHeroReborn[mapId][battleIdx][stepId][npcId]={}
  elseif nil == gNPCHeroReborn[mapId][battleIdx][stepId]  then
    gNPCHeroReborn[mapId][battleIdx][stepId]={}
    gNPCHeroReborn[mapId][battleIdx][stepId][npcId]={}
  elseif nil == 	gNPCHeroReborn[mapId][battleIdx][stepId][npcId] then
    gNPCHeroReborn[mapId][battleIdx][stepId][npcId] = {}
  end
  table.insert(gNPCHeroReborn[mapId][battleIdx][stepId][npcId] , un64ObjIdx)
end;

-- 判断存在  并且删除
local function GuideCheckNPCHeroReborn(mapId,battleIdx, stepId,npcId,objIdx)
  --print("---GuideCheckNPCHeroReborn():",mapId,battleIdx, stepId,npcId,objIdx)
  if nil ~= gNPCHeroReborn[mapId][battleIdx] and nil ~= gNPCHeroReborn[mapId][battleIdx][stepId] and
    nil ~= gNPCHeroReborn[mapId][battleIdx][stepId][npcId] then

    for pos, Idx in ipairs( gNPCHeroReborn[mapId][battleIdx][stepId][npcId] ) do
      if objIdx == Idx then
        ThisBattle:LogDebug("---i  find it:",stepId,npcId,objIdx)
        table.remove(gNPCHeroReborn[mapId][battleIdx][stepId][npcId] , objIdx)
        return true
      end
    end
  end
--  print("---i can not find npcId:",stepId,npcId,objIdx)
  return false
end;



--删除--
function RemoveGuideNPCHeroReborn(mapId,battleIdx)
  if nil ~= gNPCHeroReborn[mapId] and nil ~= gNPCHeroReborn[mapId][battleIdx] then
    gNPCHeroReborn[mapId][battleIdx] = nil 
  end
end;

----NPC英雄出生-------
local function OnGuideHeroDeadBorn(mapId,battleIdx, stepId,npcId, un64ObjIdx)
  --print("--OnGuideHeroDeadBorn(mapId,battleIdx, stepId,npcId, un64ObjIdx)---")
  local delayCfg = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCHeroRebornDelayTime
  if nil ~= delayCfg and un64ObjIdx > 0 then
    GuideAddNPCHeroRebornIdx(mapId,battleIdx, stepId,npcId, un64ObjIdx)

    --print("--OnGuideHeroDeadBorn",battleIdx, stepId,npcId,un64ObjIdx,delayCfg)
    ThisBattle:InitNPCHeroRbDTime(battleIdx, stepId,npcId,un64ObjIdx,delayCfg)
  end
end;

--出生英雄--
function OnGuideBornHeroNPC(mapId,battleIdx, stepId,  npcId,objIdx)

  if nil ~= objIdx and objIdx > 0 then
    ThisBattle:LogDebug("OnGuideBornHeroNPC()",mapId,battleIdx, stepId, objIdx)
  end

  local totalPath = #MapBattleLinePathCfg[mapId]

  local heroNum = guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCHeroBornNum
  local n = 1
  local i = 1

  while (i <= totalPath ) do

    if nil ~= guideMapBattleCfg[mapId].guideTaskCfg[stepId].HeroNPCCfg[i] then
      local heroNpcList = guideMapBattleCfg[mapId].guideTaskCfg[stepId].HeroNPCCfg[i]

      local NpcBornId = heroNpcList.npcIdex[n]
      local NpcBornDir = heroNpcList.npcDir[n]
      local goLinePath = MapBattleLinePathCfg[mapId][i]
      local lineCamp =GetPathCamp(mapId,i)-- MapBattleLinePathCamp[mapId][i]

      if 	  nil ~= objIdx and objIdx > 0 and nil ~=npcId and npcId > 0 then
        if NpcBornId == npcId and  true == GuideCheckNPCHeroReborn(mapId,battleIdx, stepId,NpcBornId,objIdx) then

          local	un64ObjIdx = ThisBattle:AddObject(battleIdx, NpcBornId, lineCamp,	goLinePath[1], NpcBornDir )

          if   nil ~= un64ObjIdx  and un64ObjIdx > 0  then
            ThisBattle:StartOccupyOrder(battleIdx,un64ObjIdx,0, goLinePath)

            OnGuideHeroDeadBorn(mapId,battleIdx, stepId,NpcBornId,un64ObjIdx)
          else
            ThisBattle:LogDebug("--error,ThisBattle:AddObject wrong--stepId:"..stepId..",NpcBornId:"..NpcBornId..",pathId:"..i..",n32BornNPCIdx:"..n32BornNPCIdx)
          end
        end

      else

		  local	un64ObjIdx = ThisBattle:AddObject(battleIdx, NpcBornId, lineCamp,	goLinePath[1], NpcBornDir )

		  if   nil ~= un64ObjIdx  and un64ObjIdx > 0  then
			ThisBattle:StartOccupyOrder(battleIdx, un64ObjIdx,0, goLinePath)

			OnGuideHeroDeadBorn(mapId,battleIdx,stepId,NpcBornId,un64ObjIdx)
		  else
			ThisBattle:LogDebug("--error,ThisBattle:AddObject wrong--stepId:"..stepId..",NpcBornId:"..NpcBornId..",pathId:"..i..",n32BornNPCIdx:"..n32BornNPCIdx)
		  end
		end
    end
    i = i + 1
  end
end;


function OnGuideBornNPC(mapId,battleIdx, stepId, ifStop)

  local bFlag = CheckMapBattleIdx(mapId,battleIdx)
  if bFlag == false then
    return
  end

  --vm
  if   guideMapBattleCfg[mapId].guideTaskCfg[stepId].wildNPCCfg ~= nil then
    if ifStop > 0  then
      --ThisBattle:LogDebug("--ok, BornWMById is start ...!",stepId)
      BornWMById(mapId,battleIdx,stepId )
    end
    --npc
  elseif  guideMapBattleCfg[mapId].guideTaskCfg[stepId].NPCCfg ~= nil   then
    ----print("--12--",stepId)
    OnBornNPC(mapId,battleIdx,stepId )

    --heroNPC
  elseif guideMapBattleCfg[mapId].guideTaskCfg[stepId].HeroNPCCfg ~= nil and nil ~= guideMapBattleCfg[mapId].guideTaskCfg[stepId].HeroNPStop and
    GetGuideHeroNPCBornState(mapId,battleIdx, stepId) < 1 then
    ----print("--12-2222-",stepId)
    OnGuideBornHeroNPC(mapId, battleIdx,stepId )

    ----print("OnGuideBornNPC("..mapId..","..battleIdx..",".. stepId..", "..ifStop..") ")
    ChangeGuideHeroNPCBornState(mapId,battleIdx, stepId,1)
  end

  return 0
end;

--complete one step
function OnGuideCompStep(mapId,battleId,stepId,un64ObjIdx,campId)

	  local bFlag = CheckMapBattleIdx(mapId,battleId)
	  if bFlag == false then
		return
	  end

	  local laward = guideMapBattleCfg[mapId].guideTaskCfg[stepId].award
	  if nil ~=laward and nil ~= laward.gold and laward.gold > 0 then
		ThisBattle:AddGold(battleId, un64ObjIdx,laward.gold)
	  end

	  if nil ~=laward and nil ~= laward.cp and laward.cp > 0 then
		ThisBattle:SetCurCp(battleId,un64ObjIdx,laward.cp)
	  end

	  -- clear stepId VM
	  if nil ~= guideMapBattleCfg[mapId].guideTaskCfg[stepId].compStep  then
		local compTable = guideMapBattleCfg[mapId].guideTaskCfg[stepId].compStep
		if #compTable > 0 then
		  for m, n in ipairs(compTable) do
			ThisBattle:RemoveWMCDCof(battleId, n)
		  end
		end
	  end
end; 