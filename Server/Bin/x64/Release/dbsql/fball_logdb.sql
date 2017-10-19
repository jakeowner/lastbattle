/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50614
Source Host           : localhost:3306
Source Database       : fball_logdb_1

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2014-12-05 19:31:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `game_log`
-- ----------------------------
DROP TABLE IF EXISTS `game_log`;
CREATE TABLE `game_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `log_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `log_type` int(8) NOT NULL,
  `log_str` text CHARACTER SET utf8 NOT NULL,
  `log_ip` varchar(32) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of game_log
-- ----------------------------

-- ----------------------------
-- Procedure structure for `sp_fetch_fball_logdb`
-- ----------------------------
DROP PROCEDURE IF EXISTS `sp_fetch_fball_logdb`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_fetch_fball_logdb`()
BEGIN  
	SET @log_path = "D:/zhyz/gamelog/";  
	SET @ctime = DATE_FORMAT(NOW(), '%Y%m%d%H%i');
	SET @file_path = CONCAT("\"",@log_path,@ctime,".txt","\"");   
	SET @mysql = CONCAT("SELECT log_type,log_time,log_str from game_log ORDER BY log_time ASC INTO OUTFILE", @file_path);
	PREPARE user_log FROM @mysql; 
	EXECUTE  user_log; 
END
;;
DELIMITER ;

-- ----------------------------
-- Event structure for `event_fetch_logdb`
-- ----------------------------
DROP EVENT IF EXISTS `event_fetch_logdb`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` EVENT `event_fetch_logdb` ON SCHEDULE EVERY 1 DAY STARTS '2014-10-14 00:05:00' ON COMPLETION NOT PRESERVE ENABLE DO CALL sp_fetch_fball_logdb()
;;
DELIMITER ;
