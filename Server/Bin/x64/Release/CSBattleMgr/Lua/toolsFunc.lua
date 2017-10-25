--tools.lua--

----------------------------------工具类--------------------
function table.indexof(array, obj)
	local ret = 0
	for i, v in pairs(array) do
		if v == obj then
			ret = i
			break
		end
	end
	return ret
end;

--删除表对象--
function table.removeObj(array, obj)
	local idx = table.indexof(array, obj)
	local ret = table.remove(array, idx)

	return ret
end;

-- 深层复制
function deepcopy(object)
	local lookup_table = {}
	local function _copy(object)
		if type(object) ~= "table" then
			return object
		elseif lookup_table[object] then
			return lookup_table[object]
		end  -- if
		local new_table = {}
		lookup_table[object] = new_table
		for index, value in pairs(object) do
			new_table[_copy(index)] = _copy(value)
		end  -- for
		return setmetatable(new_table, getmetatable(object))
	end  -- function _copy
	return _copy(object)
end;  -- function deepcopy
-- 两个表进行比较(key,value全等)
function table.compare( tbl1, tbl2 )
	for k, v in pairs( tbl1 ) do
		if ( type(v) == "table" and type(tbl2[k]) == "table" ) then
			if (not table.compare( v, tbl2[k] ) ) then return false end
		else
			if ( v ~= tbl2[k] ) then return false end
		end
	end
	for k, v in pairs( tbl2 ) do
		if ( type(v) == "table" and type(tbl1[k]) == "table" ) then
			if (not table.compare( v, tbl1[k] ) ) then return false end
		else
			if ( v ~= tbl1[k] ) then return false end
		end
	end
	return true
end;
--表比较(只要求值等)
function table.comparekeyvalue( tbl1, tbl2 )
	for k, v in pairs( tbl1 ) do
		if ( type(v) == "table" and type(tbl2[k]) == "table" ) then
			if (not table.compare( v, tbl2[k] ) ) then return false end
			else
			local hasValue = false
			for k2,v2 in pairs(tbl2) do
				if(v2 == v) then
					hasValue = true
					break
				end
			end
			if (not hasValue) then return false end
		end
	end
	for k, v in pairs( tbl2 ) do
		if ( type(v) == "table" and type(tbl1[k]) == "table" ) then
			if (not table.compare( v, tbl1[k] ) ) then return false end
			else
			for k1,v1 in pairs(tbl1) do
				if(v1 == v) then
					hasValue = true
					break;
				end
			end
			if (not hasValue) then return false end
		end
	end
	return true
end;


-----------------------------------------功能结束----------------------------------------
function CollectGarbage(un32SceneID, un32MapID)
  local n32MemUsed = collectgarbage("count")
  local strLog = "SceneID : "..un32SceneID.." MapID : "..un32MapID..", before call garbage mem used : "..n32MemUsed
  thisScene:WriteLog(strLog)
  collectgarbage("collect")
  n32MemUsed = collectgarbage("count")
  strLog = "SceneID : "..un32SceneID.." MapID : "..un32MapID..", after call garbage mem used : "..n32MemUsed
  thisScene:WriteLog(strLog)
end;

function DoRandomByTime()
  math.randomseed(tostring(os.time()):reverse():sub(1, 6))
end;
