---npc solder born func----
---------------------------------------------工程兵-------------------------------------
local     warChariotCfg={} 

local function   ChangeCurtBornNPCTimes(mapId,battleIdx,pathId)
	 
	if	 nil  == warChariotCfg then
		warChariotCfg = {}
	elseif  	nil == warChariotCfg[mapId] then
		warChariotCfg[mapId]  = {}
		warChariotCfg[mapId] [battleIdx]={}
		warChariotCfg[mapId] [battleIdx][pathId]={}
		warChariotCfg[mapId] [battleIdx][pathId] =1 
	elseif 	 nil  ==   warChariotCfg[mapId] [battleIdx] then
		warChariotCfg[mapId] [battleIdx]={}
		warChariotCfg[mapId] [battleIdx][pathId]={}
		warChariotCfg[mapId] [battleIdx][pathId] =1
	elseif 	nil == warChariotCfg[mapId] [battleIdx][pathId] then
		warChariotCfg[mapId] [battleIdx][pathId] = {}
		warChariotCfg[mapId] [battleIdx][pathId] =1 
	elseif 	nil 	== warChariotCfg[mapId] [battleIdx][pathId]  then 
		warChariotCfg[mapId] [battleIdx][pathId]  =  1 
	else
		warChariotCfg[mapId] [battleIdx][pathId]  =  warChariotCfg[mapId] [battleIdx][pathId]  + 1
	end 
end

function  RemoveCurtBornNPCTimes(mapId,battleIdx, pathId )
	if nil  ~= warChariotCfg   and   nil ~=  warChariotCfg[mapId]   and   nil ~=   warChariotCfg[mapId] [battleIdx] and nil ~=   warChariotCfg[mapId] [battleIdx][pathId] then
		warChariotCfg[mapId][battleIdx][pathId]   =   nil
		return
	end 
	
	if nil  ~= warChariotCfg   and   nil ~=  warChariotCfg[mapId]   and   nil ~=   warChariotCfg[mapId] [battleIdx] then
		warChariotCfg[mapId][battleIdx]  =   nil
	end 
end

--获取当前 出兵  轮数
local    function  GetCurtBornNPCTimes(mapId,battleIdx,pathId, campId)
	if nil  ~= warChariotCfg   and   nil ~=  warChariotCfg[mapId]   and   nil ~=  warChariotCfg[mapId] [battleIdx] and 
		nil ~= warChariotCfg[mapId] [battleIdx][pathId]  	then
		return  warChariotCfg[mapId][battleIdx] [pathId] 
	end 
	return  nil
end

--获取当前攻城车编号
local function    GetWarChariotIdx(mapId,pathId,curtNpcLv)
	if  nil == mapId  or  nil ==pathId  or  nil == curtNpcLv then
		return     nil
	end
	
	if nil == MapBattleCfg[mapId]  or  nil ==  MapBattleCfg[mapId].NPCBornListCfg    or
		nil ==   MapBattleCfg[mapId].NPCBornListCfg.NPCWarCahriotCfg[pathId]  or 
		nil == MapBattleCfg[mapId].NPCBornListCfg.NPCWarCahriotCfg[pathId][curtNpcLv] then
		return   nil
	end	  
	return  MapBattleCfg[mapId].NPCBornListCfg.NPCWarCahriotCfg[pathId][curtNpcLv].totalNPCBornTimesCfg,
	MapBattleCfg[mapId].NPCBornListCfg.NPCWarCahriotCfg[pathId][curtNpcLv].BornNPCIdx
end  

--出兵计数--
local n32BornNPCIdx = 1;
local gMapBattleBornIdx = {
[1003] = {},
[1002] = {},
[1001] = {},
[1000] = {},
};

--删除出兵计数中battleidx
function RemoveBornNPCIdx(mapId,battleId)
	if nil ~= gMapBattleBornIdx[mapId][battleId] then
		gMapBattleBornIdx[mapId][battleId] = nil 
	end
	end;

--获取battle出兵计数
local function GetBattleNPCBornIdx(mapId,battleId)
	if nil == gMapBattleBornIdx[mapId]	then
		return n32BornNPCIdx
	end

	if nil == gMapBattleBornIdx[mapId][battleId]  then
		return n32BornNPCIdx
	end

	return gMapBattleBornIdx[mapId][battleId].num
	end;
--修改出兵下标
local function ChangeBattleNPCBornNum(mapId,battleId, num) 
	if nil == gMapBattleBornIdx[mapId][battleId] then 
		gMapBattleBornIdx[mapId][battleId] = {} 
		gMapBattleBornIdx[mapId][battleId].num = nil
		gMapBattleBornIdx[mapId][battleId].num = num
	else
		gMapBattleBornIdx[mapId][battleId].num = num
	end
	end; 
-------------------------------------------------------小兵npc出生------------------------------------------

--超级兵-----------------------------------------------------------


--超级兵功能:记录打爆的祭坛ID
local gBrokenBuilding={
};
--添加--
local function AddBuildPath(mapId,battleIdx,path, buildIdex,lv,campId)
	if nil == gBrokenBuilding[mapId] then
		gBrokenBuilding[mapId]={}  
		gBrokenBuilding[mapId][battleIdx] = {} 
		gBrokenBuilding[mapId][battleIdx][path]= {}
		gBrokenBuilding[mapId][battleIdx][path][campId] = {}
		gBrokenBuilding[mapId][battleIdx][path][campId][lv] = {}
		
	elseif nil == gBrokenBuilding[mapId][battleIdx] then
		gBrokenBuilding[mapId][battleIdx] = {} 
		gBrokenBuilding[mapId][battleIdx][path]= {}
		gBrokenBuilding[mapId][battleIdx][path][campId] = {}
		gBrokenBuilding[mapId][battleIdx][path][campId][lv] = {}

	elseif nil == gBrokenBuilding[mapId][battleIdx][path] then
		gBrokenBuilding[mapId][battleIdx][path]= {}
		gBrokenBuilding[mapId][battleIdx][path][campId] = {}
		gBrokenBuilding[mapId][battleIdx][path][campId][lv] = {}

	elseif nil == 	gBrokenBuilding[mapId][battleIdx][path][campId] then
		gBrokenBuilding[mapId][battleIdx][path][campId]  = {}
		gBrokenBuilding[mapId][battleIdx][path][campId][lv] = {}
		
	elseif nil == 	gBrokenBuilding[mapId][battleIdx][path][campId][lv] then
		gBrokenBuilding[mapId][battleIdx][path][campId][lv] = {}
	end

	table.insert(gBrokenBuilding[mapId][battleIdx][path][campId][lv],buildIdex); 
end; 

local function GetBuildPathLv(sourceTb, descTb) 
	for pathId, pathTb in pairs(sourceTb) do 
		for lvId, lvTb in pairs(pathTb) do

			for dest, desTb in pairs(descTb) do 

				if  true == table.comparekeyvalue(pathTb, desTb) then
					return lvId
				end 
			end
		end
	end
	return nil
end;

--删除--
function RemoveBuildPathLv(mapId,battleIdx)
	if nil ~= gBrokenBuilding[mapId] and  nil ~= gBrokenBuilding[mapId][battleIdx] then
		gBrokenBuilding[mapId][battleIdx] = nil
	end
end;

--获取建筑所属路径,lv--
--local pathId, curtLv 1001 1 1 
local function CheckIfBuildPathUpLv(mapId,battleIdx,campId)
	if nil == gBrokenBuilding[mapId][battleIdx] then
		return  nil, nil
	end

	for pathId, pathBuildTbCfg in pairs(MapBattleCfg[mapId].NPCBornListCfg.BelongPathCfg) do

		if nil ~= gBrokenBuilding[mapId][battleIdx][pathId]  and nil ~= gBrokenBuilding[mapId][battleIdx][pathId][campId]  then

			local localSource = pathBuildTbCfg
			local localDest = gBrokenBuilding[mapId][battleIdx][pathId][campId]

			local  lvId = GetBuildPathLv(localSource, localDest )
			if nil ~= lvId then
				return pathId,lvId
			end
		end
	end
	return nil, nil
end;

--1001 1 14 1
local function CheckAddBuildPathLv(mapId,battleIdx, buildIdex,campId)

	if nil == MapBattleCfg[mapId]  then
		ThisBattle:LogDebug("--MapBattleCfg["..mapId.."].NPCBornListCfg is nil --")
		return
	end
	if nil == MapBattleCfg[mapId].NPCBornListCfg then
		ThisBattle:LogDebug("--MapBattleCfg["..mapId.."].NPCBornListCfg.BelongPathCfg is nil --")
		return
	end

	for k, v in pairs(MapBattleCfg[mapId].NPCBornListCfg.BelongPathCfg) do
		for lv, buildTable in ipairs(v) do
			for idx, buildId in ipairs(buildTable) do
				local path = k
				if buildId == buildIdex then
				--local lv = m
					AddBuildPath(mapId,battleIdx,path,buildIdex,lv,campId)
				  return
				end
			end 
		end
	end 
end;

local gBattleSuperNPC = {
	 
	[1000]={
		curtBornTime = 0,
	},
};

--战斗结束删除--
function RemoveSuperNPCBattleId(mapId,battleIdx)
	if nil ~= gBattleSuperNPC[mapId] and nil ~= gBattleSuperNPC[mapId][battleIdx] then
		gBattleSuperNPC[mapId][battleIdx] = nil
	end
end;



--升级N路小兵
local function UpdateLvPathCfgByPathId(mapId,battleIdx, pathId,curtLv,campId,buildIdex)
	if nil == gBattleSuperNPC[mapId] then
		gBattleSuperNPC[mapId] = {}
		gBattleSuperNPC[mapId][battleIdx] = {}
		gBattleSuperNPC[mapId][battleIdx][pathId] = {}
		gBattleSuperNPC[mapId][battleIdx][pathId][campId] = {} 
		
	elseif nil == gBattleSuperNPC[mapId][battleIdx] then
		gBattleSuperNPC[mapId][battleIdx] = {}
		gBattleSuperNPC[mapId][battleIdx][pathId] = {}
		gBattleSuperNPC[mapId][battleIdx][pathId][campId] = {}

	elseif    nil == gBattleSuperNPC[mapId][battleIdx][pathId] then
		gBattleSuperNPC[mapId][battleIdx][pathId] ={}
		gBattleSuperNPC[mapId][battleIdx][pathId][campId] = {}

	elseif 	nil == gBattleSuperNPC[mapId][battleIdx][pathId][campId] then
		gBattleSuperNPC[mapId][battleIdx][pathId][campId]={}
	end
	--1001 1 =>2 1 1
	--1002 1 =>1 2 1
	--print("update curtLv:",mapId,battleIdx, pathId,campId,curtLv)
	gBattleSuperNPC[mapId][battleIdx][pathId][campId]["lv"] = curtLv  
end;

--获取最新等级NPC编号
local function GetPathLvByParam(mapId,battleIdx,pathId,campId)

	if nil ~= gBattleSuperNPC[mapId] and nil ~= gBattleSuperNPC[mapId][battleIdx] and 
		nil ~= gBattleSuperNPC[mapId][battleIdx][pathId] and nil ~= gBattleSuperNPC[mapId][battleIdx][pathId][campId] and 
		nil ~= gBattleSuperNPC[mapId][battleIdx][pathId][campId]["lv"] 	then  
		 
			 return gBattleSuperNPC[mapId][battleIdx][pathId][campId]["lv"] 
	end
	
	return 0
end; 

 

--1001 1 1 2 1
local function GetPathLvSolderListByPathId(mapId,battleIdx,pathId,campId) 
	
	local curtNpcLv  =  GetPathLvByParam(mapId,battleIdx,pathId,campId)  
	--print("curtNpcLv:",mapId,battleIdx,pathId,campId,curtNpcLv)
	
	local solderList = MapBattleCfg[mapId].NPCBornListCfg.PathSolderCfg[pathId][curtNpcLv]
	if nil == solderList then
		ThisBattle:LogDebug("--error,NPCBornListCfg.PathSolderCfg["..pathId.."]["..curtLv.."] is nil--")
		return
	end 
	
	local    timesCfg, warChariotIdx  =   GetWarChariotIdx(mapId,pathId,curtNpcLv)   
	local    curtNpcTimes  =  GetCurtBornNPCTimes(mapId,battleIdx,pathId, campId)  
	--print(timesCfg, warChariotIdx,curtNpcTimes)
	if	 nil ~= timesCfg 	and 	timesCfg > 0 and 	nil ~= curtNpcTimes 	and 	curtNpcTimes > 0  and
		( curtNpcTimes  %  timesCfg  == 0 ) then   
		--print("warChariotIdx",warChariotIdx)  
		return solderList, warChariotIdx
	end 
	return solderList, nil
end;


--生成小兵1
local function DoOnBronNPC(mapId,battleIdx, pathId, n32BornNPCIdx) 
	
	local campId = GetPathCamp(mapId,pathId) --MapBattleLinePathCamp[mapId][pathId]
	if nil == campId then
		ThisBattle:LogError("==error, the nil campId,"..mapId..","..battleIdx..","..pathId..","..n32BornNPCIdx)	
		return 	
	end	
	
	local goLinePath = MapBattleLinePathCfg[mapId][pathId] 
	
	local NPCList,  warChariotIdx =  GetPathLvSolderListByPathId(mapId,battleIdx,pathId,campId)  
	
	local   npcIdx = 0	
	if   nil ~=	warChariotIdx and  warChariotIdx >  0  then
		npcIdx  =  warChariotIdx 
	else
		npcIdx =   NPCList[n32BornNPCIdx] 
	end

	if  nil 	== 	npcIdx    or 	 npcIdx  <  1        then 
		ThisBattle:LogDebug("===ther is no npcid of npclist==",n32BornNPCIdx)
		return
	end    
	
	local SolderBornDir = MapBattleCfg[mapId].NPCBornListCfg.PathSolderDir[pathId]   
	newLinePath = deepcopy(goLinePath)
	local bornPos = newLinePath[1]
	table.remove(newLinePath,1)  
	 
	
	local un64ObjIdx = ThisBattle:AddObject(battleIdx,npcIdx, campId , bornPos,SolderBornDir[0])
	if  un64ObjIdx > 0    then
		ThisBattle:StartOccupyOrder(battleIdx,un64ObjIdx,0, newLinePath)  
	else
		ThisBattle:LogDebug("OnBornNPC() "..battleIdx.." ObjIdx "..un64ObjIdx.." Obj is Nil,"..pathId..","..campId..","..npcIdx)
	end 
	
	--RemoveBuildPathLv(mapId,battleIdx)
	
	if   nil ~= warChariotIdx and warChariotIdx > 0 then
		RemoveCurtBornNPCTimes(mapId,battleIdx, pathId )
	end
end;


--当建筑被打爆的时候调用--
-- 1001 1 14 1
function SetPathBuildingState(mapId,battleIdx, buildIdex,campId)
	print("--0-------------",mapId,battleIdx, buildIdex,campId) 

	local bFlag = CheckMapBattleIdx(mapId,battleIdx)
	if  bFlag == false then
			print("--1-------------")
		return
	end

	CheckAddBuildPathLv(mapId,battleIdx,buildIdex,campId)

	  local pathId, curtLv = CheckIfBuildPathUpLv(mapId,battleIdx,campId)
	  if  pathId == nil or curtLv == nil then
			  print("--2-------------")
		   return
	   end 
	print("--2-------------",curtLv)
	UpdateLvPathCfgByPathId(mapId,battleIdx,pathId,curtLv,campId,buildIdex) 
end;

   local function ChangeBattleNPCBornPortion(mapId,battleId ,totalPath, localNPCspaceTime,localNPCEverySpaceTime )
   	local nNPCIDx =  GetBattleNPCBornIdx(mapId,battleId)
   	nNPCIDx = nNPCIDx +1

   	ChangeBattleNPCBornNum(mapId,battleId, nNPCIDx) 
   	local bornNum  =  MapBattleBaseCfg[mapId].n32TotalNPcBornNum    
	
	local    changeTimes = false
   	if  nNPCIDx > bornNum  then  
   		ChangeBattleNPCBornNum(mapId,battleId, 1) 
   		ThisBattle:SetNPCBornCDMilsec(battleId,localNPCspaceTime)    
		
		changeTimes = true
	else
		ThisBattle:SetNPCBornCDMilsec(battleId,localNPCEverySpaceTime) 
	end 
	
	--工程车
	if  changeTimes then
		local pathId = 1
		while  (pathId < totalPath) do
			ChangeCurtBornNPCTimes(mapId,battleId,pathId) 
			pathId = pathId + 1
		end
	end
	
	end;
 

---普通出兵--
function OnNormalBornNPC(mapId,battleIdx)
	local bFlag = CheckMapBattleIdx(mapId,battleIdx)
	if bFlag == false then
		ThisBattle:LogDebug("--i can not find this---",mapId,battleIdx)
		return
	end 
	local nPCIdxNum = GetBattleNPCBornIdx(mapId,battleIdx) 
	if nPCIdxNum > MapBattleBaseCfg[mapId].n32TotalNPcBornNum  then
		nPCIdxNum = 1
	end

	local totalPath = #MapBattleLinePathCfg[mapId]
	local i = 1
	while (i <= totalPath ) do
		DoOnBronNPC(mapId,battleIdx,i, nPCIdxNum)
		i = i + 1
	end 
	ChangeBattleNPCBornPortion(mapId,battleIdx,totalPath , MapBattleBaseCfg[mapId].bornTimeSpace, MapBattleBaseCfg[mapId].everyTimeTimeSpace ) 
	end;