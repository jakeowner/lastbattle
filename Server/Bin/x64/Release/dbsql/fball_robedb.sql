/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50614
Source Host           : localhost:3306
Source Database       : fball_robedb

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2014-11-07 14:32:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `game_robe`
-- ----------------------------
DROP TABLE IF EXISTS `game_robe`;
CREATE TABLE `game_robe` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `robe_batch_id` int(20) NOT NULL COMMENT '鎵规?',
  `robe_channel` varchar(32) NOT NULL COMMENT '娓犻亾',
  `part_cs` varchar(32) NOT NULL COMMENT '鍖烘湇',
  `robe_type` int(16) NOT NULL COMMENT '婵?椿鐮佺被鍨',
  `robe_number` varchar(64) NOT NULL COMMENT '绀煎寘鐮',
  `robe_use_state` bit(1) NOT NULL DEFAULT b'0' COMMENT '浣跨敤鐘舵?:0鏈?娇鐢?1宸茬粡浣跨敤',
  `robe_start_time` datetime NOT NULL COMMENT '婵?椿鏃堕棿',
  `robe_end_time` datetime NOT NULL COMMENT '缁撴潫鏃堕棿',
  `robe_del_state` bit(1) NOT NULL DEFAULT b'0' COMMENT '杩囨湡鐘舵?',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of game_robe
-- ----------------------------
INSERT INTO `game_robe` VALUES ('7', '1', '100', '1', '1', '1e1bddf0-5e98-11e4-a0e4-74d4353947f8', '', '2014-10-28 19:46:57', '2014-11-04 19:46:57', '');
INSERT INTO `game_robe` VALUES ('8', '1', '100', '1', '1', '378f6b16-5e98-11e4-a0e4-74d4353947f8', '', '2014-10-28 19:47:39', '2014-11-04 19:47:39', '');
INSERT INTO `game_robe` VALUES ('9', '1', '100', '1', '1', '379758be-5e98-11e4-a0e4-74d4353947f8', '', '2014-10-28 19:47:39', '2014-11-04 19:47:39', '');
INSERT INTO `game_robe` VALUES ('10', '1', '100', '1', '1', '379fafa2-5e98-11e4-a0e4-74d4353947f8', '', '2014-10-28 19:47:39', '2014-11-04 19:47:39', '');
INSERT INTO `game_robe` VALUES ('11', '1', '100', '1', '1', '37a8fea9-5e98-11e4-a0e4-74d4353947f8', '', '2014-10-28 19:47:40', '2014-11-04 19:47:40', '');
