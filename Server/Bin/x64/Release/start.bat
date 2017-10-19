taskkill /f /t /im redis-server.exe 
taskkill /f /t /im CSBattleMgr.exe 
taskkill /f /t /im SSBattleMgr.exe 
taskkill /f /t /im GSConsole.exe 
taskkill /f /t /im BalanceServer.exe
taskkill /f /t /im LoginServer.exe
taskkill /f /t /im GSKernel.exe
taskkill /f /t /im RobotConsole.exe
taskkill /f /t /im LogServer.exe


ping -n 1 127.0>nul
start /min "redis-server" "redis-server.exe" redis.conf

ping -n 1 127.0>nul
start /min "redis-Logicserver" "redis-server.exe" redis-logic.conf

ping -n 1 127.0>nul
echo "start CSBattleMgr.exe"
start /min "CSBattleMgr" "CSBattleMgr.exe"

ping -n 1 127.0>nul
echo "start SSBattleMgr.exe"
start /min "SSBattleMgr" "SSBattleMgr.exe"

ping -n 1 127.0>nul
echo "start GSKernel.exe"
start /min "GSKernel" "GSKernel.exe"

ping -n 1 127.0>nul
echo "start BalanceServer.exe"
start /min "BalanceServer" "BalanceServer.exe"

ping -n 1 127.0>nul
echo "start LoginServer.exe"
start /min "LoginServer" "LoginServer.exe"

ping -n 1 127.0>nul
echo "start LogServer.exe"
start /min "LogServer" "LogServer.exe"
