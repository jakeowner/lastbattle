---guide config---

guideMapBattleCfg = { 

[1000]={
    acGuideHeroIdCfg = {19999,}, 		--新手引导出生英雄ID配置(多个配置则随机出生)--
    -------------------------------------任务步骤ID,isGuideComp:引导是否完成，award：该步骤的奖励，compStep:该步骤能够完成其他步骤的ID------------------------------------------
    guideTaskCfg = {
    [1001]={ isGuideComp = 0,award={} ,compStep={},},
    [1002]={ isGuideComp = 0,award={} ,compStep={},},
    [1004]={ isGuideComp = 0,award={} ,compStep={},},
    [1008]={ isGuideComp = 0,award={} ,compStep={},},
    [1005]={ isGuideComp = 0,award={} ,compStep={1003,},},
    [1007]={ isGuideComp = 0,award={} ,compStep={1006,},},
    [1019]={ isGuideComp = 1, award={} ,compStep={}, },
    [1012]={ isGuideComp = 0,award={} ,compStep={},},
    [1015]={ isGuideComp = 0,award={} ,compStep={},},
    [1013]={ isGuideComp = 0,award={} ,compStep={},},
    [1016]={ isGuideComp = 0,award={} ,compStep={},},
    [1018]={ isGuideComp = 0,award={} ,compStep={},},
    [1023]={ isGuideComp = 0,award={} ,compStep={},},
    [1025]={ isGuideComp = 0,award={} ,compStep={},},
    [1021]={ isGuideComp = 0,award={} ,compStep={},},
    [1022]={ isGuideComp = 0,award={} ,compStep={},},
    [1003]={ isGuideComp = 0,award={},compStep={},
        wildNPCCfg={--野怪点及具体配置
          npcIdex = {21027,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{1901.0, 6600.0, 14310.0}, },
          bornDirCfg = {{0, 0, 1},  },
          patrolPos={{5985.0, 1802.0, 11749.0},},--巡逻点
          cdTime = 5000,  			--cd 时间
          isBornAgain = 1,			--是否再次出生
          camp = -1,		 			--阵营
          },
        bornTime = 99,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1006]={ isGuideComp = 0,award={},compStep={},
        wildNPCCfg={--野怪点及具体配??
          npcIdex = {20001,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{3993.0, 6600.0, 12250.0},   },
          bornDirCfg = {{0, 0, 1},   },
          patrolPos={{5985.0, 1802.0, 11749.0}, },--巡逻点
          cdTime = 10000,  			--cd 时间
          isBornAgain = 1,			--是否再次出生
          camp = -1,		 			--阵营
          },
        bornTime = 99,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1010]={ isGuideComp = 0,award={ },compStep={},
        wildNPCCfg={--野怪点及具体配??
          npcIdex = {21027,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{5600.0, 6600.0, 14400.0},  },
          bornDirCfg = {{0, 0, 1}, },
          patrolPos={{5985.0, 1802.0, 11749.0},},--巡逻点
          cdTime = 5000,  			--cd 时间
           isBornAgain = 1,			--是否再次出生
          camp = -1,		 			--阵营
          },
        bornTime = 99,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1009]={ isGuideComp = 0,award={ },compStep={1009},
        wildNPCCfg={--野怪点及具体配??
          npcIdex = {21027,21027,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{5500.0, 6600.0, 14380.0}, {6000.0, 6590.0, 14390.0}, },
          bornDirCfg = {{0, 0, 1}, {0, 0, -1}, },
          patrolPos={{5985.0, 1802.0, 11749.0},{6198.0, 1802.0, 11222.0},},--巡逻点
          cdTime = 5000,  			--cd 时间
           isBornAgain = 1,			--是否再次出生
          camp = -1,					 --阵营
          },
        bornTime = 1,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1011]={ isGuideComp = 0,award={},compStep={1010,},
        wildNPCCfg={--野怪点及具体配??
          npcIdex = {21027,21027,21027,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{6000.0, 6600.0, 13900.0}, {6200.0, 6590.0, 14000.0}, {6100.0, 6590.0, 14000.0}, },
          bornDirCfg = {{0, 0, 1}, {0, 0, -1}, {0, 0, -1}, },
          patrolPos={{5985.0, 1802.0, 11749.0},{6198.0, 1802.0, 11222.0},{6198.0, 1802.0, 11222.0},},--巡逻点
          cdTime = 5000,  			--cd 时间
          isBornAgain = 1,			--是否再次出生
          camp = -1,		 			--阵营
          },
        bornTime = 1,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1022]={ isGuideComp = 0,award={},compStep={},
        wildNPCCfg={--野怪点及具体配??
          npcIdex = {21026,}, 		--刷怪ID
          isPatrol = {0,0,0,},
          radius = {1000,1000,1000},			--活动半径
          bornPosCfg = {{2073.0, 6600.0, 14670.0}, },
          bornDirCfg = {{0, 0, 1}, },
          patrolPos={{5985.0, 1802.0, 11749.0},},--巡逻点
          cdTime = 50000000,  			--cd 时间
          isBornAgain = 1,			--是否再次出生
          camp = -1,		 			--阵营
          },
        bornTime = 1,							--出生次数
        curtBornTime = 0,						--当前出生次数
        },
        [1014]={ isGuideComp = 0,award={},compStep={1014,},
        HeroNPCCfg = {--出生NPC英雄配置： 路径--->出生NPC英雄编号,出生NPC方向
        [1]={npcIdex={21017,}, npcDir = {{0, 0, 1},},},
        },
        NPCHeroBornNum = 1,  				--单轮出生NPC数量
        HeroNPStop = 0, 				--是否停止出生
        },

        [1024]={ isGuideComp = 0,award={},compStep={},
        HeroNPCCfg = {--出生NPC英雄配置： 路径--->出生NPC英雄编号,出生NPC方向
        [4]={npcIdex={21025,}, npcDir = {{0, 0, 1},},},
        [7]={npcIdex={21024,}, npcDir = {{0, 0, 1},},},
        },
        NPCHeroRebornDelayTime = 15000,		--英雄NPC死亡从新延迟时间
        NPCHeroBornNum = 1,  					--单轮出生NPC数量
        HeroNPStop = 0, 					--是否停止出生
        },

        [1017]={ isGuideComp = 0,award={},compStep={},
        NPCCfg={--NPC配置： 路径--->出生NPC编号,出生NPC方向
        [2]={npcIdex={20012,20012,},npcDir = {{0, 0, 1},{0, 0, 1},},},
        [3]={npcIdex={20012,20012,},npcDir = {{0, 0, 1},{0, 0, 1},},},
        [5]={npcIdex={20010,},npcDir = {{0, 0, 1},},},
        [6]={npcIdex={20010,},npcDir = {{0, 0, 1},},},
        },
        NPCEveryBornNum = 1,  			    --单轮出生NPC数量
        NPCspaceTime = 20000, 				--出生一波NPC时间间隔
        NPCEverySpaceTime = 10000, 			--单个NPC出生时间间隔
        },
        }, 
        },
        };
        
        