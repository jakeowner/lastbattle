/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50614
Source Host           : localhost:3306
Source Database       : fball_accountdb

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2014-11-21 10:31:01
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `account_user`
-- ----------------------------
DROP TABLE IF EXISTS `account_user`;
CREATE TABLE `account_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cs_id` int(10) unsigned NOT NULL,
  `sdk_id` int(10) unsigned NOT NULL,
  `cdkey` varchar(32) NOT NULL,
  `user_name` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_name_check` (`user_name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=10000 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account_user
-- ----------------------------

DROP TABLE IF EXISTS `cdkey_events`;
CREATE TABLE `cdkey_events` (
  `event_id` int(20) NOT NULL,
  `event_title` varchar(32) DEFAULT NULL,
  `event_platform` int(8) NOT NULL,
  `event_end_time` int(20) NOT NULL,
  `event_content` varchar(150) DEFAULT NULL,
  `event_type` int(8) NOT NULL,
  `event_item` varchar(32) DEFAULT NULL,
  `event_code_num` int(8) NOT NULL,
  `event_code_len` int(8) NOT NULL,
  PRIMARY KEY (`event_id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `cdkey_info`;
CREATE TABLE `cdkey_info` (
  `cdkey` varchar(32) DEFAULT NULL,
  `event_id` int(20) NOT NULL,
  `use_state` int(8) NOT NULL,
  `use_time` int(20) NOT NULL,
  `server_id` int(16) NOT NULL,
  `user_name` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`cdkey`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Procedure structure for `query_id`
-- ----------------------------
DROP PROCEDURE IF EXISTS `query_id`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `query_id`(in csid INT, in sdkid INT, IN act VARCHAR(32), OUT xid INTEGER, OUT xname VARCHAR(32) )
BEGIN   
		declare v_id  INT;
    declare v_name VARCHAR(32) ;
		
		SELECT  id,user_name INTO v_id ,v_name from account_user  WHERE  cs_id=csid and  sdk_id=sdkid and  cdkey=act;  
		IF  v_id is null THEN  
				INSERT INTO account_user(cs_id,sdk_id,cdkey)VALUES(csid,sdkid,act); 
				SELECT LAST_INSERT_ID() INTO v_id;
		END IF; 
		 
		SET xid = v_id;
		SET xname = v_name; 
end
;;
DELIMITER ;

-- ----------------------------
-- Procedure structure for `query_update_user_name`
-- ----------------------------
DROP PROCEDURE IF EXISTS `query_update_user_name`;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `query_update_user_name`(IN uname VARCHAR(32),in csid INT, in sdkid INT,IN ucdkey VARCHAR(32),  OUT xid INT)
BEGIN   
		SELECT id INTO xid from account_user WHERE user_name= uname;
		IF  xid is null THEN   
				UPDATE account_user SET user_name=uname where cs_id=csid and sdk_id=sdkid and cdkey = ucdkey;
				
				set xid = 0;
		END IF;  
		
		SELECT xid;
end
;;
DELIMITER ;
