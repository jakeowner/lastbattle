/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 50614
Source Host           : localhost:3306
Source Database       : fball_gamedb_1

Target Server Type    : MYSQL
Target Server Version : 50614
File Encoding         : 65001

Date: 2014-12-19 16:01:15
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `gameuser`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser`;
CREATE TABLE `gameuser` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `obj_id` bigint(20) unsigned NOT NULL,
  `sdk_id` int(8) DEFAULT '0',
  `obj_cdkey` char(30) COLLATE utf8_unicode_ci NOT NULL,
  `obj_name` varchar(64) COLLATE utf8_unicode_ci DEFAULT NULL,
  `obj_sex` int(4) NOT NULL DEFAULT '0',
  `obj_lv` int(8) NOT NULL DEFAULT '0',
  `obj_score` bigint(20) NOT NULL DEFAULT '0',
  `obj_headid` int(8) NOT NULL DEFAULT '0',
  `obj_diamond` bigint(16) NOT NULL DEFAULT '0',
  `obj_gold` bigint(20) NOT NULL DEFAULT '0',
  `obj_register_time` bigint(20) NOT NULL DEFAULT '0',
  `obj_last_login_time` bigint(20) NOT NULL DEFAULT '0',
  `obj_game_inns` int(16) NOT NULL DEFAULT '0',
  `obj_game_winns` int(16) NOT NULL DEFAULT '0',
  `obj_kill_hero_num` int(16) NOT NULL DEFAULT '0',
  `obj_ass_kill_num` int(16) NOT NULL DEFAULT '0',
  `obj_dest_building_num` int(16) NOT NULL DEFAULT '0',
  `obj_dead_num` int(16) NOT NULL DEFAULT '0',
  `obj_first_win_time` bigint(20) NOT NULL DEFAULT '0',
  `obj_cur_lv_exp` int(11) NOT NULL DEFAULT '0',
  `obj_cldays` int(8) NOT NULL DEFAULT '0',
  `obj_friends` text COLLATE utf8_unicode_ci,
  `obj_last_loginreward_time` int(16) unsigned zerofill DEFAULT NULL,
  `obj_vip_lv` int(8) NOT NULL DEFAULT '0',
  `obj_vip_score` int(16) NOT NULL DEFAULT '0',
  `obj_task_data` varchar(4096) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`,`obj_id`),
  KEY `eUserPlatform` (`sdk_id`),
  KEY `szUserName` (`obj_cdkey`),
  KEY `szNickName` (`obj_name`)
) ENGINE=InnoDB AUTO_INCREMENT=1576 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of gameuser
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_guide`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_guide`;
CREATE TABLE `gameuser_guide` (
  `obj_id` bigint(20) NOT NULL,
  `obj_cs_guide_com_steps` varchar(128) CHARACTER SET utf8 DEFAULT NULL,
  `obj_ss_battle_steps` varchar(128) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`obj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of gameuser_guide
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_hero`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_hero`;
CREATE TABLE `gameuser_hero` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `user_id` bigint(20) NOT NULL,
  `hero_id` int(12) NOT NULL,
  `hero_end_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁炬儳缍婇弻鐔兼⒒鐎?缂佺偓鍎崇紞濠囧蓟閿濆?绠ｉ柨婵嗘啗閹剧粯鐓曢柕鍫濇噹?闂?婵犵數鍋涘?鍫曟偋閻樿尙鏆?濠电姷鏁告慨鐑藉极閸涘﹥鍙忛柣鎴ｆ?閺嬩線鏌熼梻瀵?缁惧墽绮?换娑㈠箣濞嗗繒浠鹃梺绋?閸婃繈寮婚弴鐔虹?鐟滃秹宕?閹?ɑ绻濋崶??濠电偠?缁犳捇鐛?幇鏉跨??闁告垹濞?弻鈥?閸愩劌?缂備胶濞?粻鏍?蓟閻?婵?鍚嬮崳褔姊虹拠鑼?畾闁哄懐濞??濠氭晲婢跺﹦?闁诲酣娼ч幗婊堟偪閸?鈷戦柛婵嗗?閻掕法绱掔紒妯肩畵闁伙絽鍢??閹?喖鎮?闂傚倸鍊搁崐鐑芥嚄??闂備浇?鐎涒晠鎯?瀹曪繝宕?婵犵數濮电喊宥夊疾閹?閻犺櫣灏ㄩ崝?闂?闂?闂?缂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁惧墽鎳?閻?閹癸綁鏌?婵?闂??闂傚倸鍊峰ù鍥?閵娾晜鍊块柨鏇炲?缁??鏌??闂傚倸鍊搁崐鎼佸磹閹间礁纾瑰?瀣???濠?闁荤喐绮岀换姗?嵁閹达箑?闁靛牆鎳?闂備礁婀遍崑鎾?濞?闁秆?閳锋垶鎱ㄩ悷鐗堟悙闁诲繆鏅犻弻鐔烘嫚瑜忕弧??閺囩喎濮嶆俊?鍟?埥澶愬箳閹惧瓨婢戦梻鍌欒兌缁?鏁嬬紒?闂傚倸鍊烽悞锔界箾婵犲洤缁╅梺?绉撮崹鍌炴煕瑜?闁?缂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌ｉ幋锝呅撻柛銈呭?閺屾盯骞橀懠?闂?闂??闂傚倸鍊搁崐宄懊?婵?婵犵數濮烽弫鎼佸磻濞?闁?娲橀崑瀣?煟濡?搫甯犵紒璇叉?閺屾盯骞橀懠璺哄帯闂?闂?闂傚倸鍊烽懗??婵?缂佽?鐗?閻犲洦绁村Σ鍫熸叏?闁告洖鍟村?娲?川婵犲啫鐦烽梺?閻?閰ｅ?',
  `hero_buy_time` bigint(20) NOT NULL DEFAULT '0' COMMENT '闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁炬儳缍婇弻鐔兼⒒鐎?缂佹儳澧介幊?闁?闁圭櫢缍侀弻?婵??婵?闂備浇宕甸崰鎰版偡鏉堚晝涓嶉柟鎹?缁?缂傚倸鍊烽懗鍫曞磻閹捐?纾块柛?闂佽法鍠撴慨鎾?嫅閻斿吋鐓熼柡鍐ㄥ?鐢?墎绱掗悩鍐插摵闁?闂傚倸鍊烽懗鍫曞箠閹捐?瑙﹂悗?濠电偛妫欓幐濠氬疾?闁??闂?缂傚倸鍊搁崐鎼佸磹閹间礁纾瑰?瀣?捣閻?闂傚倸鍊峰ù鍥?閵娿儍?闂傚倷妞掔槐?寰婇崸妤??婵?缁犳牠鏌ｉ敐鍛?伇闁?鐟╅幃褰掑箒閹烘垵?闂佸搫?閹?倸?缂佹ɑ濯撮柧蹇曟嚀缁?闂傚倷鑳剁划?闂佸摜鍠愰幐鎶芥偘?缂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾剧懓?鐎?鎹ｉ柣?闂傚倸鍊搁崐宄懊归崶?闁靛ň鏅滈崐鍧楁煥閺囩偛?缂佺姵鐗犻弻娑氫沪閻?娈ら梺鍝勬４缁犳捇寮婚敐鍛?濠⒀佸灮缁?闂佸搫鏈?惄?淇婇悜钘夌?婵犻潧娲ら悵鏃傜磽娴ｄ粙鍝洪柣鎾??缂備焦蓱婵?挳鏌ц箛鏇熷殌缂佹?绻濆??闂傚倸鍊搁崐椋庣矆娓?绠熼梽鍥?箺?濮?闂?濠?闁诲海鎳撻幉锛勬崲閸岀偛鐓橀柟杈鹃檮閸嬫劙鏌??闂傚倸鍊搁崐鎼佸磹妞嬪海鐭嗗〒?缁犵喖鏌ㄩ悢鍝勑㈤柦鍐?枛閺屾洘绻涢悙?鐝曢梺?闁哄被鍔岄埞鎴﹀幢濡?儤?闂備礁鎼?Λ鎾?⒔閸?宕?闂傚倷绀侀幖?骞婅箛娑欏亗闁跨喓濮撮拑鐔哥箾閹存瑥鐏╅柣鎺撴そ閺屾盯骞囬妸锔界彇?缂',
  `del_state` bit(1) NOT NULL DEFAULT b'0' COMMENT '闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁炬儳缍婇弻鐔兼⒒鐎?缂佺偓鍎崇紞濠囧蓟閿濆?绠ｉ柨婵嗘啗閹剧粯鐓曢柕鍫濇噹?闂?婵犵數鍋涘?鍫曟偋閻樿尙鏆?濠电姷鏁告慨鐑藉极閸涘﹥鍙忛柣鎴ｆ?閺嬩線鏌熼梻瀵?缁惧墽绮?换娑㈠箣濞嗗繒浠鹃梺绋?閸婃繈寮婚弴鐔虹?鐟滃秹宕?閹?ɑ绻濋崶??濠电偠?缁犳捇鐛?幇鏉跨??闁告垹濞?弻鈥?閸愩劌?缂備胶濞?粻鏍?蓟閻?婵?鍚嬮崳褔姊虹拠鑼?畾闁哄懐濞??濠氭晲婢跺﹦?闁诲酣娼ч幗婊堟偪閸?鈷戦柛婵嗗?閻掕法绱掔紒妯肩畵闁伙絽鍢??閹?喖鎮?闂傚倸鍊搁崐鐑芥嚄??闂備浇?鐎涒晠鎯?瀹曪繝宕?婵犵數濮电喊宥夊疾閹?閻犺櫣灏ㄩ崝?闂?闂?闂?闂傚倸鍊搁崐鎼佸磹閹间礁纾归柣鎴ｅГ閸ゅ嫰鏌涢幘鑼?闁?闂?缂?婵犵數濮烽弫鍛婃叏閻戣棄鏋侀柛娑橈攻閸欏繘鏌ｉ幋锝?闁哄?绶氶弻娑樷槈濮楀牊鏁鹃梺鍛婄懃缁绘﹢寮婚敐澶?闁绘ê鍟块弳鍫ユ⒑閹肩偛?闁?閻忓啴姊洪幐搴ｇ畵闁瑰啿閰ｅ?鎶芥倷閻戞?鍘辨繝鐢靛Т閸婄?危婵犳碍鐓冮悷娆忓?閻忔挳鏌?閺呯姴鐣烽敐鍡楃窞?闂傚倸鍊烽懗鍓佸垝?楠炴劙宕?妷锕?簥闂佸湱澧楀?姗?偂濮?鐓欓弶鍫ョ畺濡?鏌涚??濮嶉柡??闂傚倸鍊搁崐宄?閹烘梹??缂佽京鍋為幏鍛村捶?闂?婵炲弶鐗犻幏?闂?1闂',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci ROW_FORMAT=COMPACT;

-- ----------------------------
-- Records of gameuser_hero
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_item`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_item`;
CREATE TABLE `gameuser_item` (
  `user_id` int(12) NOT NULL,
  `item_id` bigint(20) NOT NULL DEFAULT '0',
  `item_num` int(8) NOT NULL DEFAULT '0',
  `buy_time` int(12) NOT NULL,
  `end_time` int(12) NOT NULL,
  PRIMARY KEY (`user_id`,`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_item
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_mail`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_mail`;
CREATE TABLE `gameuser_mail` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `mail_id` int(20) NOT NULL COMMENT 'mailid',
  `user_id` int(20) NOT NULL,
  `mail_state` int(16) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_mail
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
  PRIMARY KEY (`id`,`obj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_money
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_runne`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_runne`;
CREATE TABLE `gameuser_runne` (
  `id` int(12) NOT NULL AUTO_INCREMENT,
  `user_id` int(12) NOT NULL,
  `runnebag_json` text,
  `runeslot_json` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1618 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_runne
-- ----------------------------

-- ----------------------------
-- Table structure for `gameuser_sns`
-- ----------------------------
DROP TABLE IF EXISTS `gameuser_sns`;
CREATE TABLE `gameuser_sns` (
  `user_id` int(12) NOT NULL,
  `related_id` int(12) NOT NULL,
  `relation` int(8) NOT NULL,
  PRIMARY KEY (`user_id`,`related_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of gameuser_sns
-- ----------------------------

-- ----------------------------
-- Table structure for `game_mail`
-- ----------------------------
DROP TABLE IF EXISTS `game_mail`;
CREATE TABLE `game_mail` (
  `mail_id` int(20) NOT NULL,
  `mail_sdk` int(8) NOT NULL DEFAULT '0',
  `mail_type` int(12) NOT NULL DEFAULT '0' COMMENT '闂?濠电姷鏁告慨鐑藉极閸涘﹥鍙忛柣鎴ｆ?閺嬩線鏌涘☉姗堟敾闁告瑥绻橀弻锝夊箣閿?闂佸搫?缁舵岸寮婚悢鍏尖拻閻?缂備焦鍎虫晶鐣屽垝?閹广垹鈽夐姀鐘??濠电姷鏁搁崑鐐哄垂閸?洘鍋￠柨鏇炲?绾惧潡鏌ｉ弬鍨?倯闁绘挸绻愰埞鎴︽倷閼?铏庨梺鍛婃⒐瀹?悂寮?缂傚倷鑳剁划?鎹㈤崼銉ョ畺濞寸姴??濠电偟??缂佹彃娼￠幆?鐎?澧庨幑鍕?Ω閹?闂佸搫娲ㄩ崰搴ㄦ倶鐎??闂傚倷鑳剁划?骞愮拠瑁佹椽鎮㈤悡搴ゆ憰闂佺粯鏌ㄩ崥瀣?磹?闂?闂?闂佽?绻戝??闂?婵?闁?浜??',
  `mail_user_id` int(20) DEFAULT NULL COMMENT '闂?濠电姷鏁告慨鐑藉极閸涘﹥鍙忛柣鎴ｆ?閺嬩線鏌涘☉姗堟敾闁告瑥绻橀弻锝夊箣閿?闂佸搫?缁舵岸寮婚悢鍏尖拻閻?缂備焦鍎虫晶鐣屽垝?閹广垹鈽夐姀鐘??濠电姷鏁搁崑鐐哄垂閸?洘鍋￠柨鏇炲?绾惧潡鏌ｉ弬鍨?倯闁绘挸绻愰埞鎴︽倷閼?铏庨梺鍛婃⒐瀹?悂寮?缂傚倷鑳剁划?绮?缂傚倷绀?闁?闂?闂??闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌ｉ幋锝呅撻柛銈呭?閺?婵炲弶鐗滅划濠氭偐缂佹?鍘?闂?婵犵數鍋涘?鍫曟偋濠婂懎鍨濈紓浣姑?闁诲孩绋?闁告?鏁诲?娲?川婵犲啫鐭?梺缁樺釜缁犳垿鍩㈠?澶婂嵆?闂備浇?瀹曟?浜稿▎鎴犵幓闁哄啫鐗婇悡娑??闁哄?鐩?弻?闂?闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁炬儳缍婇弻鐔兼⒒鐎?缂佺偓鍎崇紞濠囧蓟閿濆?绠ｉ柨婵嗘啗閹剧粯鐓曞┑鐘插?閸嬨儵鏌?闂傚倷娴囧畷鐢稿窗閹?鍨濈??瀹曟煡鏌熼悜?缂?妫濋弻鐔兼倻濡?櫣浜堕梺鍝?闁帮綁寮婚悢鐓庣畾鐟滃繘鏁?闂傚倸鍊搁崐鐑芥倿閿?鐟滃秷鐏嬫繛杈剧秬閸婅棄鈽夐姀鐘??缂?閻楀繘骞夐幖浣瑰亱闁?绻勯悷鏌ユ⒑閹惰姤鏁遍柛銊ユ贡濡叉劙骞???濠?婵犮垻鎳撳Λ娑樜?闂?闂佺?锕ラ幃鍌炲箖?闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌ｉ幋锝呅撻柛銈呭?閺?婵炲弶鐗滅划濠氭偐缂佹?鍘甸梺?闂?婵犵數濮烽弫鍛婃叏閻戣棄鏋侀柛娑橈攻閸??闂傚倸鍊搁崐鐑芥嚄閸撲礁鍨?闁??闁?绻傞崑宥夋偡濠婂嫭?妤?闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾惧綊鏌熼梻瀵?缁炬儳缍婇弻鐔兼⒒鐎?缂佹儳澧介幊?闁?闁圭櫢缍侀弻?婵??婵?缂?闂?濠',
  `mail_title` varchar(128) NOT NULL COMMENT 'mail title',
  `mail_content` text NOT NULL COMMENT 'content',
  `mail_gift` text COMMENT '闂?濠?缂?闂傚倸鍊搁崐鎼佸磹閹间礁纾归柟闂寸?绾剧懓?鐎?鎹ｉ柣?妫楅妴?缂?闂傚倸鍊搁崐鎼佸磹閻戣姤鍊块柨鏇炲?缁犳牠鏌?濞叉牜澹??key:value)',
  `mail_send` varchar(64) DEFAULT NULL,
  `mail_create_time` varchar(32) NOT NULL COMMENT '闂?濠电姷鏁告慨鐑藉极閸涘﹥鍙忛柣鎴ｆ?閺嬩線鏌涘☉姗堟敾闁告瑥绻橀弻锝夊箣閿?闂佸搫?缁舵岸寮婚悢鍏尖拻閻?缂備焦鍎虫晶鐣屽垝?閹广垹鈽夐姀鐘??濠电姷鏁搁崑鐐哄垂閸?洘鍋￠柨鏇炲?绾惧潡鏌ｉ弬鍨?倯闁绘挸绻愰埞鎴︽倷閼?铏庨梺鍛婃⒐瀹?悂寮?婵?閿曘儱?娴犲?绠犻柣鎰?惈鍞?闂傚倸鍊搁崐椋庣矆娓?绠?閾荤偤鏌?闁抽攱妫冮弻?缂?婵犵數濮烽弫鍛婃叏?闂備礁鎲￠幐鑽ゆ崲閸℃稑桅闁圭増婢樼粈?闂佺???闁宠?鍨块幃鈺呭箵閹烘挻?闂?闂傚倸鍊搁崐鎼佸磹閻戣姤鍤??闂傚倸鍊风粈渚?箟閳?鐟滅増甯楅崑?闂?缂?闂傚倸鍊搁崐鐑芥倿閿旂晫绠惧┑鐘叉搐缂佲晠姊?缁夋挳宕归崒鐐寸厱鐟?闂佽棄鍟伴崰鎰?崲濞戙垹绠ｉ柣鎰?暞瀹???閵婏妇?閻?閸?闂傚倸鍊搁崐鎼佸磹妞嬪海鐭嗗〒?缁?闁捐崵鍋炵换娑㈠幢濡??闉嶉梺绋?缁绘繈寮诲?澶婁紶闁?濞咃綁姊洪挊澶婃殶闁哥姵鐗犲?濠氬Ω閵夈垺鏂?闂?闂?闂傚倷鑳剁划?鎮ч弴銏犖ч柟闂磋兌瀹',
  `mail_over_time` varchar(32) NOT NULL,
  `mail_del_state` int(8) NOT NULL,
  PRIMARY KEY (`mail_id`),
  KEY `mail_id` (`mail_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of game_mail
-- ----------------------------

-- ----------------------------
-- Table structure for `notice`
-- ----------------------------
DROP TABLE IF EXISTS `notice`;
CREATE TABLE `notice` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `platform_id` int(12) NOT NULL,
  `title` varchar(20) NOT NULL,
  `eflag` int(8) NOT NULL DEFAULT '0',
  `estate` int(8) NOT NULL DEFAULT '0',
  `priority` int(8) NOT NULL DEFAULT '0',
  `notice` varchar(200) NOT NULL,
  `star_time` int(16) NOT NULL DEFAULT '0',
  `end_time` int(16) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of notice
-- ----------------------------
