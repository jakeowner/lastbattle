package.path = package.path..";./CSBattleMgr/Lua/?.lua;"..";./CSBattleMgr/Lua/CSLua/?.lua;"

--print(package.path)

	-- require "Config"

	-- require "guideConfig"

	-- require "GuideFunc"

	-- require "GMFunc"  

	-- require "BattleFunc"

	-- require "InitGameFunc"
	 
	-- require "ConfigFunc"

	-- require "NpcSolderFunc"

	-- require "monsterFunc"

	-- require "altarFunc"

	require "..\ToolsFunc"
	
	ThisBattle:LogInfo("---this is from cslua load.lua--")
-------------load.lua------------------------------
    DoRandomByTime()