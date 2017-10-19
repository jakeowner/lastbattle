set GLOBAL event_scheduler =1;
CREATE EVENT event_del_time_over_mail
ON SCHEDULE EVERY 1 DAY
STARTS TIMESTAMP '2014-10-14 00:10:00'
DO CALL sp_del_time_over_mail() ;