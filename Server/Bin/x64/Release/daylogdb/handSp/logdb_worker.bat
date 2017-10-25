@echo off  
:begin   
 
::mysql -h 180.150.190.122 -uroot -p123321 fball_logdb_1 < log_sql.sql
 
mysql -h 127.0.0.1 -uroot -p123321 fball_logdb_1 < log_sql.sql
@echo ok 
	pause
	:end
 
 