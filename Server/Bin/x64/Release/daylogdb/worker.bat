@echo off  
:begin  
mysql -uroot -p123321 fball_logdb_1 < event_fball_logdb.sql
mysql -uroot -p123321 fball_logdb_2 < event_fball_logdb.sql
mysql -uroot -p123321 fball_logdb_3 < event_fball_logdb.sql

mysql -uroot -p123321 fball_logdb_1 < pr_fball_logdb.sql
mysql -uroot -p123321 fball_logdb_2 < pr_fball_logdb.sql
mysql -uroot -p123321 fball_logdb_3 < pr_fball_logdb.sql
@echo mysql ok 
 
	
rem 创建明天的日志文件夹
rem CScript timeCreateFolder.vbs 
rem 添加本机任务计划
rem 打开本机计划任务服务
rem 利用sc config改变服务启动方式,auto为自动
rem sc config schedule start = auto
rem 启动服务
rem net start schedule
rem 添加计划任务
rem SCHTASKS /Create /SC DAILY /ST 00:05:00 /TN logftp /TR D:/workspace/zuihouyizhan/publish/FBAll/Bin/x64/Release/ftp_log/ftp.bat 
pause