/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50614
Source Host           : localhost:3306
Source Database       : fball_chargedb

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2014-11-07 14:32:10
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `gameuser_charge`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_charge`;
CREATE TABLE `gameuser_charge` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `charge_cs_id` bigint(20) DEFAULT NULL COMMENT '鍏呭?鍖烘湇',
  `charge_account` varchar(32) NOT NULL COMMENT '鍏呭?璐﹀彿',
  `charge_channel` varchar(32) NOT NULL COMMENT '鍏呭?骞冲彴',
  `charge_amount` int(12) NOT NULL COMMENT '鍏呭?閲戦?',
  `charge_uuid` varchar(32) NOT NULL COMMENT '鍏呭?璁㈠崟鍙',
  `charge_state` bit(1) NOT NULL DEFAULT b'0' COMMENT '鍏呭?鏄?惁鎴愬姛:0澶辫触,1鎴愬姛',
  `charge_start_time` datetime NOT NULL COMMENT '鍏呭?寮??鏃堕棿',
  `charge_end_time` datetime DEFAULT NULL COMMENT '鍏呭?鎴愬姛鏃堕棿',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_charge
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_money`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_money`;
CREATE TABLE `gameuser_money` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `obj_id` bigint(20) NOT NULL,
  `obj_diamond` int(12) NOT NULL DEFAULT '0',
  `obj_gold` int(12) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_money
-- ----------------------------
