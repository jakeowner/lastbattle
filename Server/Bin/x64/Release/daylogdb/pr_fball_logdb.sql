 
##use fball_logdb; 
drop PROCEDURE if exists  sp_fetch_fball_logdb; 
DELIMITER  // 
CREATE PROCEDURE sp_fetch_fball_logdb()
BEGIN   
	SET @log_path = "D:/zhyz/gamelog/";  
	SET @ctime = DATE_FORMAT(NOW(), '%Y%m%d%H%i');
	SET @file_path = CONCAT("\"",@log_path,@ctime,".txt","\"");   
	SET @mysql = CONCAT("SELECT log_type,log_time,log_str from game_log ORDER BY log_time ASC INTO OUTFILE", @file_path);
	PREPARE user_log FROM @mysql; 
	EXECUTE  user_log; 
END;
//
DELIMITER ;

