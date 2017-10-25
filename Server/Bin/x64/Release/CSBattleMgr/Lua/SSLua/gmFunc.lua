---GM func--


----------------------------------GM cmd config--------------------------------------------------------------
 
GM_Cmd = {
  [1]="lv",	--eg:lv 10
  [2]="cp",	--eg:cp 100
  [3]="speed",--eg:speed 5
  [4]="hp",	--eg:hp  1000
  [5]="mp",	--eg:mp  1000
  [6]="rage",	--eg:rage 1000
  [7]="finish",	--eg:finish 1
  [8]="guidefinish",	--eg:guidefinish 1
  [9] ="notice",
};
 
 local GM_Cmd_switch = 1 
----------------------------------GM function------------------------------------------------------------
--lua spilit string
function LuaSpilitStr(cmd)
  if nil == cmd or string.len(cmd) < 2 then
    ThisBattle:LogDebug("---from lua: too short cmd!--")
    return
  end
  local cmdTable = {}
  local lcmd  = string.lower(cmd)
  lcmd = string.gsub(lcmd, "^%s*(.-)%s*$", "%1")

  local leng = string.len(lcmd)
  local n1 = 1
  while n1 < leng do
    local nPos = string.find(lcmd," ")
    if nil == nPos or nPos < 2 then

      table.insert(cmdTable,lcmd)
      return cmdTable
    end
    local strSub = string.sub(lcmd,1,nPos)
    strSub = string.gsub(strSub, "^%s*(.-)%s*$", "%1")
    table.insert(cmdTable,strSub)

    lcmd = string.sub(lcmd,nPos)
    lcmd = string.gsub(lcmd, "^%s*(.-)%s*$", "%1")

    leng = string.len(lcmd)
    if leng <=1 then

      table.insert(cmdTable,lcmd)
      return cmdTable
    end
  end
  return cmdTable
end;

--do gm cmd function
function GMCmd( cmd )
  if nil == GM_Cmd_switch or GM_Cmd_switch < 1 then
    ThisBattle:LogDebug("--GM Cmd is off! ",GM_Cmd_switch)
    return
  end
  if   nil == cmd or string.len(cmd) < 2 then
    ThisBattle:LogDebug("---GMCmd param is nil or cmd is too short!,",objIdx,cmd)
    return
  end

  if nil == GM_Cmd then
    ThisBattle:LogDebug("---GM_Cmd config is not find" )
    return
  end

  local cmdTable = LuaSpilitStr(cmd)
  local battleIdx = cmdTable[1]
  battleIdx = tonumber(battleIdx)
  if nil == battleIdx  or battleIdx < 1 then
    ThisBattle:LogDebug("---error:battleIdx is nil!->",un64ObjIdx)
    return
  end

  local un64ObjIdx = cmdTable[2]
  un64ObjIdx = tonumber(un64ObjIdx)
  if nil == un64ObjIdx or un64ObjIdx < 1 then
    ThisBattle:LogDebug("---error:objIdx is nil!->",un64ObjIdx)
    return
  end
  local cmdHead = cmdTable[3]
  local cmdExist = false
  for k, v in pairs(GM_Cmd) do
    local cmdCfg = string.lower(v)
    if cmdCfg == cmdHead then

      local cmdValue = cmdTable[4]
      cmdValue = tonumber(cmdValue)
      if nil == cmdValue then
        ThisBattle:LogDebug("--error: cmdValue is nil,",cmdValue)
        return
      end
      ThisBattle:LogDebug("--ok-->"..k..","..cmdHead..","..cmdValue)

      if 1 == k then
        ThisBattle:SetCurLv(battleIdx,un64ObjIdx,cmdValue)
      elseif 2 == k then
        cmdValue = cmdValue * 1000
        if cmdValue > 10000000 then
          cmdValue = 100000000
        end
        ThisBattle:SetCurCp(battleIdx,un64ObjIdx,cmdValue)
      elseif 3 == k then
        ThisBattle:SetCurSpeed(battleIdx,un64ObjIdx,cmdValue)
      elseif 4 == k then
        ThisBattle:SetCurHp(battleIdx,un64ObjIdx,cmdValue)
      elseif 5== k then
        ThisBattle:SetCurMp(battleIdx,un64ObjIdx,cmdValue)
      elseif 6== k then
        ThisBattle:SetCurRage(battleIdx,un64ObjIdx,cmdValue)
      elseif 7== k then
        ThisBattle:FinishBattle(battleIdx,cmdValue)
      elseif 8== k then
        ThisBattle:GuideFinishBattle(battleIdx,cmdValue,un64ObjIdx)
      elseif 9== k then

      --ThisBattle:BattleNotice(battleIdx,cmdValue,un64ObjIdx)
      end

      cmdExist = true
      return
    end
  end
  if cmdExist == false then
    ThisBattle:LogDebug("--shit: i can not find gm cmd!--")
    return
  end
end;
-- check table is the same, only used number content
--return 0:==; 1: big; 2:small; 99: other
local equial = 0;
local bigger = 1;
local small = 2;
local other = 99;
function IfTheSameContent(tbS, tbD)
  if (tbS == nil or tbD == nil) or (tbS == nil and tbD ~= nil) or (tbS ~= nil and tbD == nil) then
    return other
  end
  if type(tbS) ~= "table"  then
    ThisBattle:LogDebug("--error from public.lua, tbS   is not table--",tbS)
    return other
  end
  if   type(tbD) ~= "table" then
    ThisBattle:LogDebug("--error from public.lua,  tbD is not table--",tbD)
    return other
  end

  local tbsN = #tbS
  local tbDN = #tbD
  if  tbsN > tbDN then
    return small
  end
  if  tbsN < tbDN then
    return bigger
  end
  local tNum = 0
  for m, n in pairs(tbD) do
    for k, v in pairs(tbS) do
      if (n == v) then
        tNum = tNum + 1
        break
      end
    end
  end
  if tbDN == tNum or (tbDN > tNum and tNum == tbsN )then
    return equial
  end
  return other
end;

local eEffectCate_ChangeAttackSpeed = 10;

function OnCheckFPChange(un64ObjIdx, eEffectCate, changeNum, changePersent)
  un64ObjIdx = tonumber(un64ObjIdx)

  if curGO == nil then
    ThisBattle:LogDebug("adjustAtkSpeed failed for curGO is nil with un64ObjIdx"..un64ObjIdx)
    return 0
  end

  if ThisBattle:IfTypeHero(un64ObjIdx) == 0 then
    return 0
  end

  if eEffectCate == eEffectCate_ChangeAttackSpeed then
    if changePersent > 4000 then
      changePersent = 4000
    end
    ThisBattle:AdjustAtkSpeedChange(un64ObjIdx,changeNum, changePersent)
  end

  return 0
end;