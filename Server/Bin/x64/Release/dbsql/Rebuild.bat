@echo off  
:begin
@echo ----------1, create all game database------------ 
mysql -uroot -p123321<createdb.sql
mysql -uroot -p123321<grantuser.sql 

mysql -uroot -p123321<fball_accountdb.sql
mysql -uroot -p123321<fball_chargedb.sql
mysql -uroot -p123321<fball_robedb.sql

mysql -uroot -p123321 fball_accountdb < fball_accountdb.sql
mysql -uroot -p123321 fball_chargedb < fball_chargedb.sql
mysql -uroot -p123321 fball_robedb < fball_robedb.sql

mysql -uroot -p123321 fball_gamedb_1 < fball_gamedb.sql
mysql -uroot -p123321 fball_gamedb_2 < fball_gamedb.sql
mysql -uroot -p123321 fball_gamedb_3 < fball_gamedb.sql 

mysql -uroot -p123321 fball_logdb_1 < fball_logdb.sql
mysql -uroot -p123321 fball_logdb_2 < fball_logdb.sql
mysql -uroot -p123321 fball_logdb_3 < fball_logdb.sql 

