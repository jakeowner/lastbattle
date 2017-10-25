---Game start an init samething---


--英雄坐标--
function InitHeroBornPos(mapId)
	 
	  
	  if nil == mapId  or mapId < 0 then
		ThisBattle:LogDebug("--nil mapId")
		return
	  end
	  if nil == MapBattleHeroBornCfg or nil == MapBattleHeroBornCfg[mapId] then
		 ThisBattle:LogDebug("--MapBattleHeroBornCfg or MapBattleHeroBornCfg["..mapId.."] is nil")
		return
	  end

	  for idx, bornPos in ipairs( MapBattleHeroBornCfg[mapId] ) do
		ThisBattle:SetHeroBornPos(mapId, idx, bornPos)
	  end
	  ThisBattle:LogInfo("--InitHeroBornPos ..ok!")
end;

----祭坛出兵功能配置--
function InitAltarBornSolderConfig(mapId)
  if nil == mapId  or mapId < 0 then
    ThisBattle:LogDebug("--nil mapId")
    return
  end
  if nil == MapBattleCfg or MapBattleCfg[mapId] == nil then
	return
  end
  if MapBattleCfg[mapId].AltarBornSolderInfo == nil then
	return
  end
  for k,v in pairs(MapBattleCfg[mapId].AltarBornSolderInfo) do
    ThisBattle:SetAltarBornAnimialCof(mapId, k, v.AltarBornSolderTimeSpace ,v.AltarBornSolderDelayTime,v.AltarBornNum, v.AltarMaxBornSolder)
  end

   ThisBattle:LogInfo("--InitAltarBornSolderConfig ..ok!")
end;

--怪物等级配置
function InitWildMonster(mapId)
  if nil == mapId  or mapId < 0 then
    ThisBattle:LogDebug("--nil mapId")
    return
  end 

  if nil == MapBattleVMDelayCfg then
      ThisBattle:LogDebug("--MapBattleVMDelayCfg is nil!")
    return
  end

  if nil == MapBattleVMDelayCfg[mapId] then
      ThisBattle:LogDebug("--MapBattleVMDelayCfg["..mapId.."] is nil!")
      return
  end
  
  for m, n in pairs(MapBattleVMDelayCfg[mapId]) do
    ThisBattle:SetWMDelayBornCof(mapId,  m, n)
  end 

  if nil == MapBattleCfg then
     ThisBattle:LogDebug("--MapBattleCfg is nil!")
     return
  end

  if nil == MapBattleCfg[mapId] or nil == MapBattleCfg[mapId].WMonsterFunc then
     ThisBattle:LogDebug("-- MapBattleCfg["..mapId.." ] is nil!")
     return
  end

  for k,v in pairs(MapBattleCfg[mapId].WMonsterFunc) do
    ThisBattle:SetWMCDCof(mapId,  k, v.monsterLevel ,v.cdTime,v.BUffId)
  end
   ThisBattle:LogInfo("--InitWildMonster ..ok!")
end;

--OB--
function InitOB(mapId)
  if nil == mapId  or mapId < 0 then
    ThisBattle:LogDebug("--nil mapId")
    return
  end
  if nil == MapBattleBaseCfg or nil == MapBattleBaseCfg[mapId] then
	return
  end
  if nil ~= MapBattleBaseCfg[mapId].OBSwitch then
    ThisBattle:InitOBSwitchCfg(mapId,MapBattleBaseCfg[mapId].OBSwitch )
  end

  ThisBattle:LogInfo("--InitOB ..ok!")
end;

--NPCBornDelay cfg
function InitNPCDelayCfg(mapId)
  if nil == mapId  or mapId < 0 then
    ThisBattle:LogDebug("--nil mapId")
    return
  end
  if nil == MapBattleBaseCfg or nil == MapBattleBaseCfg[mapId] then
	return
  end
  if nil ~= MapBattleBaseCfg[mapId].startTimeDelay then
    ThisBattle:SetNPCBornDelayTime(mapId,MapBattleBaseCfg[mapId].startTimeDelay)
  end

   ThisBattle:LogInfo("--InitNPCDelayCfg ..ok!")
end;

 

local function InitGuideCfg(mapId)
 -- print("InitGuideCfg")
  if nil == mapId  or mapId < 0 then
    ThisBattle:LogDebug("--nil mapId")
    return
  end

  if nil == guideMapBattleCfg then
    ThisBattle:LogDebug("--guideMapBattleCfg is nil")
    return
  end

  if nil == guideMapBattleCfg[mapId] then
    ThisBattle:LogDebug("--guideMapBattleCfg["..mapId.."]  is nil")
    return
  end  

  ThisBattle:InitGuideCfg(mapId,1)

   if nil ==  guideMapBattleCfg[mapId].acGuideHeroIdCfg then
    ThisBattle:LogDebug("--guideMapBattleCfg["..mapId.."].acGuideHeroIdCfg is nil")
    return
  end

  ThisBattle:SetHeroBornIdCfg(mapId,  guideMapBattleCfg[mapId].acGuideHeroIdCfg) 

   ThisBattle:LogInfo("--InitGuideCfg..ok!")
end;
 

local function LuaGlobalInitWithMapID(mapId)
		--InitHeroBornPos(mapId)
		--InitAltarBornSolderConfig(mapId)
		--InitWildMonster(mapId)
		--InitOB(mapId)
		--InitGuideCfg(mapId)
		--InitNPCDelayCfg(mapId)
end;
  

function initGameConfig()  
 
	for k, v in ipairs(allMapTable) do
		LuaGlobalInitWithMapID(v)
	end

  ThisBattle:LogInfo("--initGameConfig..ok!")
end;
