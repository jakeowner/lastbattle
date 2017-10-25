##################################################
##设置mysql删除过期邮件 pr
##################################################
DROP PROCEDURE if EXISTS sp_del_time_over_mail;
CREATE PROCEDURE sp_del_time_over_mail()
BEGIN 
	##设置删除过期邮件，邮件有效期为15天
	SET @del_mail = "DELETE from game_mail WHERE mail_del_state = 1 or  DATEDIFF( NOW() , mail_over_time) > 1 ";
	##设置删除邮件查看的邮件，邮件查看的邮件有效期为2天
	##SET @del_look_mail = "DELETE from game_mail WHERE mail_del_state = 1 AND mail_get_gift_state = 1 AND DATEDIFF( NOW(),mail_look_time) > 2 ";
	
	PREPARE time_over_del_mail FROM @del_mail;
	##PREPARE time_over_del_look_mail FROM @del_look_mail;
	
	EXECUTE time_over_del_mail;
	##EXECUTE time_over_del_look_mail;
END