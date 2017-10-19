set GLOBAL event_scheduler =1;
CREATE EVENT event_fetch_logdb 
ON SCHEDULE EVERY 1 DAY
STARTS TIMESTAMP '2014-10-14 00:01:00'
DO CALL sp_fetch_fball_logdb() ;