@echo off  
echo =================start server================= 

ping 127.1 -n 1 >nul
echo "cose CSBattleMgr.exe"
taskkill /f /t /im CSBattleMgr.exe
::关闭CSConsole.exe
::start "" "CSConsole.exe"   

ping 127.1 -n 1 >nul
echo "cose SSBattleMgr.exe"
taskkill /f /t /im SSBattleMgr.exe
::CSConsole.exe
::"start "" "SSBattleMgr.exe"

ping 127.1 -n 1 >nul
echo "cose GSConsole.exe"
taskkill /f /t /im GSConsole.exe
::CSConsole.exe
::start "" "GSConsole.exe"
taskkill /f /t /im BalanceServer.exe
taskkill /f /t /im LoginServer.exe
taskkill /f /t /im GSKernel.exe
taskkill /f /t /im RobotConsole.exe
taskkill /f /t /im LogServer.exe
taskkill /f /t /im redis-server.exe 
echo =================cose all server ok=================

::ping 127.1 -n 4 >nul



