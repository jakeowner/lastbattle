rem 1 通过系统计划任务，实现备份数据库 
rem 2 备份的文件名以 数据库名_back_备份时间.sql   形式存在 备份时间命名的文件夹 eg:d:\dbback\201410301139\fball_gamedb_1_backup_201410301139.sql
rem 注意需要修改备份到的具体路径 eg:d:\dbback\
rem
 
@echo off
:begin
cd /d %~dp0

set /a t1=(1%time:~0,2%-100)*1
if %t1% LSS 10 set t1=0%t1%
set ymd_hms=%date:~0,4%%date:~5,2%%date:~8,2%%t1%%time:~3,2%%time:~6,2%
::这里设置数据库备份的路径
::eg: d:\dbback\201410301139
set backPath="d:\zhyz\dbback\"%ymd_hms%
md %backPath%
cd %backPath%

set 
mysqldump -uroot -p123321 fball_gamedb_1>fball_gamedb_1_backup_%ymd_hms%.sql
mysqldump -uroot -p123321 fball_gamedb_2>fball_gamedb_2_backup_%ymd_hms%.sql
mysqldump -uroot -p123321 fball_gamedb_3>fball_gamedb_3_backup_%ymd_hms%.sql

mysqldump -uroot -p123321 fball_logdb_1>fball_logdb_1_backup_%ymd_hms%.sql
mysqldump -uroot -p123321 fball_logdb_2>fball_logdb_2_backup_%ymd_hms%.sql
mysqldump -uroot -p123321 fball_logdb_3>fball_logdb_3_backup_%ymd_hms%.sql

::mysqldump -uroot -p123321 fball_chargedb>fball_chargedb_backup_%ymd_hms%.sql
::mysqldump -uroot -p123321 fball_robedb>fball_robedb_backup_%ymd_hms%.sql
mysqldump -uroot -p123321 fball_accountdb>fball_accountdb_backup_%ymd_hms%.sql
echo backup db ok!
:end