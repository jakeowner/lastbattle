---config.func---

--获取路径阵营--
function GetPathCamp(mapId,pathId)
  if nil == MapBattleLinePathCamp then
    ThisBattle:LogDebug("MapBattleLinePathCamp is nil")
    return
  end

  if nil == MapBattleLinePathCamp[mapId] then
    ThisBattle:LogDebug("MapBattleLinePathCamp[mapId] is nil")
    return
  end
	
   local campTb = MapBattleLinePathCamp[mapId] 
   for capKey, capTb in pairs(campTb) do
		for id, pid  in pairs(capTb) do
			if pid == pathId then
				return capKey
			end
		end
   end

  return nil
end;


function CheckMapByMapId(mapId)
--	allMapTable = {1000,  1002, };
	if nil == allMapTable  then
		ThisBattle:LogEoor("MapBattleLinePathCamp[mapId] is nil")
		return false
	end

	for pos, idx in ipairs(allMapTable) do
		if idx == mapId then
			return true
		end
	end
	return false
end

