using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using GameDefine;
using JT.FWW.Tools;
using JT.FWW.GameData;
using GameDefine;
using UICommon;
using BlGame.GameData;
using BlGame;
using BlGame.GameEntity;
using BlGame.GuideDate;
using BlGame.Resource;

using BlGame.Ctrl;
using BlGame.Model;

namespace BlGame.View
{
    public class LobbyWindow : BaseWindow
    {
        public LobbyWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameLobbyUI;
            mResident = false;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide); 

#if UNITY_STANDALONE_WIN || UNITY_EDITOR|| SKIP_SDK
#else
             SdkConector.HideToolBar();
#endif
        }

        //窗口控件初始化
        protected override void InitWidget()
        {           
            mHomepage = mRoot.Find("StartMenuManager/StartMenuBtn/Homepage").GetComponent<UIToggle>();
            UIGuideCtrl.Instance.AddUiGuideEventBtn(mHomepage.gameObject);
            mBattle = mRoot.Find("StartMenuManager/StartMenuBtn/Battle").GetComponent<UIToggle>();
            mMarket = mRoot.Find("StartMenuManager/StartMenuBtn/Market").GetComponent<UIToggle>();
            mInteraction = mRoot.Find("StartMenuManager/StartMenuBtn/Interaction").GetComponent<UIToggle>();

            UIGuideCtrl.Instance.AddUiGuideEventBtn(mMarket.gameObject);
            mDiamondText = mRoot.Find("Status/Diamond/Label").GetComponent<UILabel>();
            mGoldText = mRoot.Find("Status/Gold/Label").GetComponent<UILabel>();

            mMailBtn = mRoot.Find("Status/Mail").gameObject;
            mMailBtnBg = mRoot.Find("Status/Mail/Dark").gameObject;

            mSettingBtn = mRoot.Find("Status/Setting").GetComponent<UIButton>();

            mChatBtn = mRoot.Find("StartMenuManager/Chat").gameObject;
            NewChatTask = mChatBtn.transform.Find("Tip").GetComponent<UISprite>();
            mInerPage = mRoot.Find("InerPage");

            mLevel = mRoot.Find("Status/Level").GetComponent<UILabel>();
            mHeadIcon = mRoot.Find("Status/Head/Icon").GetComponent<UISprite>();
            mNickName = mRoot.Find("Status/Name").GetComponent<UILabel>();
            mExp = mRoot.Find("Status/EXP/Num").GetComponent<UILabel>();
            mGold = mRoot.Find("Status/Gold/Label").GetComponent<UILabel>();
            mDiamond = mRoot.Find("Status/Diamond/Label").GetComponent<UILabel>();
            VipSignLevel = mRoot.Find("Status/VipSign/Label").GetComponent<UILabel>();

            //大厅GM命令
            mGMInput = mRoot.Find("InputArea/Input").GetComponent<UIInput>();
            mGMBtn = mRoot.Find("InputArea/SendMsg").gameObject;

            UIGuideCtrl.Instance.AddUiGuideEventBtn(mBattle.gameObject);
            EventDelegate.Add(mHomepage.onChange, OnHomePageChange);
            EventDelegate.Add(mBattle.onChange, OnBattleChange);
            EventDelegate.Add(mMarket.onChange, OnMarketChange);
            EventDelegate.Add(mInteraction.onChange, OnInteractionChange);
            UIEventListener.Get(mHeadIcon.transform.parent.gameObject).onClick += InfoPersonPress;

            //战斗匹配
            // BattleWindow.Instance.SetParent(mInerPage);

            UIEventListener.Get(mChatBtn).onClick += ChatTaskBtn;

            UIEventListener.Get(mMailBtn).onClick += MailListBtn;

            //大厅GM命令
            UIEventListener.Get(mGMBtn).onClick += AddNewGMCmd;

            UIEventListener.Get(mSettingBtn.gameObject).onClick += GameSetting;
        }

        private void GameSetting(GameObject go)
        {
            GameSettingCtrl.Instance.Enter();
        }

        private void InfoPersonPress(GameObject go)
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_PersonalInfo);

            LobbyCtrl.Instance.AskPersonInfo();
            //PresonInfoCtrl.Instance.Enter();
        }
        //////////////////
        /// 大厅GM命令///
        /// ////////////
        private void AddNewGMCmd(GameObject obj)
        {
            string cmd = mGMInput.value;
            if (cmd.Length > 0)
            {
                Debug.Log(cmd);
                LobbyCtrl.Instance.AskNewGMCmd(cmd);

                mGMInput.value = "";
            }
        }
        
        /// <summary>
        /// 聊天按钮
        /// </summary>
        /// <param name="go"></param>
        private void ChatTaskBtn(GameObject go)
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Chat);

            NewChat(false);
            ChatTaskCtrl.Instance.Enter(0);
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }


        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener(EGameEvent.eGameEent_ChangeMoney, RefreshMoney);
            EventCenter.AddListener<bool>(EGameEvent.eGameEvent_ReceiveLobbyMsg, NewChat);
            EventCenter.AddListener(EGameEvent.eGameEvent_AddNewMailReq, NoticeNewMail);
            EventCenter.AddListener(EGameEvent.eGameEvent_ChangeNickName,ChangeNickName);
            EventCenter.AddListener(EGameEvent.eGameEvent_ChangeHeadID, ChangeHeadID);

            EventCenter.AddListener(EGameEvent.eGameEvent_ChangeUserLevel, ChangeLevel);
        }

        private void ChangeLevel()
        {
            mLevel.text = GameUserModel.Instance.UserLevel.ToString();

        }
        private void ChangeNickName()
        {
            MsgInfoManager.Instance.ShowMsg(10039);
            mNickName.text = GameUserModel.Instance.GameUserNick;
        }

        private void ChangeHeadID()
        {
            mHeadIcon.spriteName = GameUserModel.Instance.GameUserHead.ToString();
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEent_ChangeMoney, RefreshMoney);
            EventCenter.RemoveListener<bool>(EGameEvent.eGameEvent_ReceiveLobbyMsg, NewChat);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_AddNewMailReq, NoticeNewMail);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ChangeNickName, ChangeNickName);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ChangeHeadID, ChangeHeadID);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ChangeUserLevel, ChangeLevel);
        }

        private void NewChat(bool isVib)
        {
            NewChatTask.gameObject.SetActive(FriendManager.Instance.AllTalkDic.Values.Count != 0);
            if (FriendManager.Instance.AllTalkDic.Values.Count != 0)
            {
                foreach (var item in FriendManager.Instance.AllTalkDic.Values)
                    NewChatTask.gameObject.SetActive(item.TalkState == GameDefine.MsgTalk.UnReadMsg);
            }
        }

        //显示
        public override void OnEnable()
        {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR|| SKIP_SDK
#else
            SdkConector.ShowToolBar(1);
#endif
            Debug.Log("GameUserData.Instance.GameUserGold.ToString()  " + GameUserModel.Instance.mGameUserGold.ToString());
            mGoldText.text = GameUserModel.Instance.mGameUserGold.ToString();
            mDiamondText.text = GameUserModel.Instance.mGameUserDmd.ToString();


            //初始化
            mBattle.value = true;

            //             if (SystemNoticeData.Instance.StateType == GameDefine.SystemNoticeState.SystemUnRead)
            //             {
            //                 CreateSystemNotice();
            //             }

            // #if UNITY_STANDALONE_WIN
            //             if (InputNotice == null && IGuideTaskManager.Instance().IsLineTaskFinish())
            //             {
            //                 ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadInputSend, ResourceType.PREFAB);
            //                 InputNotice = GameObject.Instantiate(objUnit.Asset) as GameObject;
            //                 InputNotice.transform.parent = mRoot;
            //                 InputNotice.transform.localPosition = Vector3.zero;
            //                 InputNotice.transform.localScale = Vector3.one;
            //             }
            // #endif



        }

        //隐藏
        public override void OnDisable()
        {

        }

        public void ChangeUserLevel()
        {
            mLevel.text = GameUserModel.Instance.UserLevel.ToString();
        }

        public void OnHomePageChange()
        {
            //todo
            mLevel.text = GameUserModel.Instance.UserLevel.ToString();
            mNickName.text = GameUserModel.Instance.GameUserNick;
            mHeadIcon.spriteName = GameUserModel.Instance.GameUserHead.ToString();
            mGold.text = GameUserModel.Instance.mGameUserGold.ToString();
            mDiamond.text = GameUserModel.Instance.mGameUserDmd.ToString();
            VipSignLevel.text = "VIP "+GameUserModel.Instance.GameUserVipLevel.ToString();
            int level = GameUserModel.Instance.UserLevel;
            mLevel.text = level.ToString();
            
            LevelConfigInfo leveinfo = ConfigReader.GetLevelInfo(level);
            if (leveinfo != null)
            {
                mExp.text = GameUserModel.Instance.GameUserExp + "/" + leveinfo.LevelUpExp;
                mExp.transform.parent.GetComponent<UISprite>().fillAmount = GameUserModel.Instance.GameUserExp / leveinfo.LevelUpExp;
                if (level >= 29 && GameUserModel.Instance.GameUserExp >= leveinfo.LevelUpExp)
                {
                    level = 30;
                    mLevel.text = level.ToString();
                    mExp.gameObject.SetActive(false);
                    mExp.transform.parent.GetComponent<UISprite>().fillAmount = 1f;
                }
            }
            
            if (mHomepage.value)
            {
                GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_HomePage);
                HomePageCtrl.Instance.Enter();
            }
            else
            {
                HomePageCtrl.Instance.Exit();
            }
        }

        public void OnBattleChange()
        {
            if (mBattle.value)
            {
                GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Battle);
                BattleCtrl.Instance.Enter();
            }
            else
            {
                BattleCtrl.Instance.Exit();
            }
        }

        public void OnMarketChange()
        {
            if (mMarket.value)
            {
                GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Market);
                MarketCtrl.Instance.Enter();
            }
            else
            {
                MarketCtrl.Instance.Exit();
            }
        }

        public void OnInteractionChange()
        {
            if (mInteraction.value)
            {
                GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Friend);
                SocialCtrl.Instance.Enter();
            }
            else
            {
                SocialCtrl.Instance.Exit();
            }
        }

        public void MailListBtn(GameObject obj)
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Mail);

            mMailBtnBg.SetActive(false);

            MailCtrl.Instance.Enter();
        }

        //新邮件按钮状态闪烁
        void NoticeNewMail()
        {
            mMailBtnBg.SetActive(true); 
        }

        /// <summary>
        /// 刷新帐号的货币
        /// </summary>
        private void RefreshMoney()
        {
            mGoldText.text = GameUserModel.Instance.mGameUserGold.ToString();
            mDiamondText.text = GameUserModel.Instance.mGameUserDmd.ToString();
        }

        private UITweener uit;
        private UIToggle mHomepage;  //主页
        private UIToggle mBattle;    //战斗
        private UIToggle mMarket;    //商城
        private UIToggle mInteraction;  //社交

        private UILabel mDiamondText;   //钻石
        private UILabel mGoldText;      //金币

        private GameObject mMailBtnBg;        //邮件按钮背景
        private GameObject mMailBtn;      //邮箱
        private UIButton mSettingBtn;   //设置
        private GameObject mChatBtn;      //聊天
        private UISprite NewChatTask;
        
        //大厅GM输入框
        private UIInput mGMInput;
        private GameObject mGMBtn; 
        

        //个人简单信息
        UILabel mLevel = null;
        UISprite mHeadIcon = null;
        UILabel mNickName = null;
        UILabel mExp = null;
        GameObject mInfoBtn = null;
        UILabel mGold = null;
        UILabel mDiamond = null;
        UILabel VipSignLevel = null;

        private Transform mInerPage;
        List<string> TempChat = new List<string>();
        /*
                GameObject InputNotice = null;
                void CreateSystemNotice()
                {
                    ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadSystemNotice, ResourceType.PREFAB);
                    if (NoticeObj == null)
                    {
                        NoticeObj = GameObject.Instantiate(objUnit.Asset) as GameObject;
                        NoticeObj.transform.parent = this.transform;
                        NoticeObj.transform.localPosition = Vector3.zero;
                        NoticeObj.transform.localScale = Vector3.one;
                    }
                    EventCenter.Broadcast<string>(EGameEvent.eGameEent_SystemNotice, SystemNoticeData.Instance.SystemNotice);
                }
                GameObject NoticeObj;

                void InvitatAddFriend(string nickName)
                {
                    if (string.IsNullOrEmpty(nickName) || DailyBonusData.Instance.IsHaveReward)
                        return;

                    ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadInvitationTips, ResourceType.PREFAB);
                    GameObject obj = GameObject.Instantiate(objUnit.Asset) as GameObject;
                    obj.transform.parent = mRoot;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    UIinviteTips tip = obj.GetComponent<UIinviteTips>();
                    tip.AddInvitaion(nickName);
                }
 
     
                private void BtnOnPress(int ie, bool pressed)
                {
                    if (pressed)
                    {
                        return;
                    }
                    for (int i = 0; i < SpriteBg.Length && ie < SpriteBg.Length; i++)
                    {
                        //			UICommonMethod.TweenAlphaBegin(SpriteBg[i].gameObject,0.3f,0.5f,UITweener.Style.PingPong);
                        SpriteBg[i].gameObject.SetActive(ie == i);
                        if (last == ie)
                        {
                            return;
                        }
                    }
                    DestroyWindow();

                    switch ((OptionLobby)ie)
                    {
                        case OptionLobby.FIGHTQUICK:
                            OnNML();
                            //			CGLCtrl_GameLogic.Instance.GamesLobby();
                            break;
                        case OptionLobby.STORE:
                            PvpAllUI = GameMethod.CreateWindow("Guis/ToBeContinued", Vector3.zero, this.transform);
                            break;
                        case OptionLobby.PERSONALINFO:
                            CGLCtrl_GameLogic.Instance.PersonInfo(GameUser.Instance.GameUserNick);
                            break;
                        case OptionLobby.GODROAD:
                            PvpAllUI = GameMethod.CreateWindow("Guis/ToBeContinued", Vector3.zero, this.transform);
                            break;
                        case OptionLobby.ACCOUNTCHANGE:
                            SetGameLobbyBtn(GameDefine.OptionLobby.ACCOUNTCHANGE);
                            break;
                        case OptionLobby.GOLD:
                            break;
                        case OptionLobby.DIAMOND:
                            break;
                    }
                    last = ie;
                }
      
     

                void NiticeOnPress(int ie, bool isPress)
                {
                    CGLCtrl_GameLogic.Instance.AskCurrNotice();
                 }
                */

    }


}
