rem 创建定时备份数据库系统计划任务
rem 
rem 设置时需要修改启动备份脚本的路径 eg:D:/workspace/zuihouyizhan/FBAll/Bin/x64/Release/dbsql/db_back.bat 为真实的路径
rem
rem 添加本机任务计划
rem 打开本机计划任务服务
rem 利用sc config改变服务启动方式,auto为自动
sc config schedule start = auto
rem 启动服务
net start schedule
rem 添加计划任务 ,如果需要修改时间，则修改一下时间
SCHTASKS /Create /SC DAILY /ST 04:05:00 /TN db_back /TR D:/workspace/zhyz_main/Main/Server/Bin/x64/Release/dbsql/db_back.bat 
::pause