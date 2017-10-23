using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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

namespace BlGame.View
{
    //大厅战斗主界面，包括创建房间，加入房间， 匹配选择等
    public class HomePageWindow : BaseWindow
    {
        public HomePageWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameHomePageUI;
            mResident = true;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_HomePageEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_HomePageExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_HomePageEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_HomePageExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            mQuickPlayHum = mRoot.Find("MenuBtn/QuickPlay_Hum").GetComponent<UIButton>();
            mQuickPlayAI = mRoot.Find("MenuBtn/QuickPlay_AI").GetComponent<UIButton>();
            mDailyBonusBtn = mRoot.Find("MenuBtn/SignUp").GetComponent<UIButton>();
            m_RuneEuip_Button = mRoot.Find("MenuBtn/Runes").GetComponent<UIButton>();
            mHeroDate = mRoot.Find("MenuBtn/HeroData").GetComponent<UIButton>();
            mVIPCard = mRoot.Find("MenuBtn/VIPCard").GetComponent<UIButton>();
            mLuxuryPack = mRoot.Find("MenuBtn/LuxuryPack").GetComponent<UIButton>();
            mCharge = mRoot.Find("MenuBtn/Charge").GetComponent<UIButton>();

            UIGuideCtrl.Instance.AddUiGuideEventBtn(m_RuneEuip_Button.gameObject);
            mScrollBar = mRoot.Find("ScrollBar").GetComponent<UIScrollBar>();
            mFocusPic = mRoot.Find("FocusPic").GetComponent<UIPanel>();

            mActivities = mRoot.Find("MenuBtn/Activities").GetComponent<UIButton>();
            
            mPoint1 = mRoot.Find("Point/1/Highlight").gameObject;
            mPoint2 = mRoot.Find("Point/2/Highlight").gameObject;
            mPoint3 = mRoot.Find("Point/3/Highlight").gameObject;

            EventDelegate.Add(mQuickPlayHum.onClick, OnQuickPlayHum);
            EventDelegate.Add(mQuickPlayAI.onClick, OnQuickPlayAI);
            EventDelegate.Add(mDailyBonusBtn.onClick, OnDaliyBonusBtnClick);
            EventDelegate.Add(m_RuneEuip_Button.onClick, OnRuneEquipClick);
            EventDelegate.Add(mHeroDate.onClick, OnHeroDateClick);
            EventDelegate.Add(mScrollBar.onChange, OnPicChange);
            EventDelegate.Add(mActivities.onClick, OnActivities);
            EventDelegate.Add(mVIPCard.onClick,Notification);
            EventDelegate.Add(mLuxuryPack.onClick,Notification);
            EventDelegate.Add(mCharge.onClick,Notification);
        }

        private void Notification()
        {
            Debug.Log("敬请期待");
            //当我们点击 超级周卡 、 豪华礼包 、 游戏充值 四个按钮时，弹出对应的提示
            //10043为MsgCfg中敬请期待的ID号
            MsgInfoManager.Instance.ShowMsg(10043);
        }
        private void OnActivities()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Activity);

            SystemNoticeCtrl.Instance.Enter();
        }
        

        //窗口控件释放
        protected override void RealseWidget()
        {

        }


        //游戏事件注册
        protected override void OnAddListener()
        {

        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {


        }

        //显示
        public override void OnEnable()
        {
            mScrollBar.value = 0f;
            mTime = Time.time;
        }

        //隐藏
        public override void OnDisable()
        {

        }

        //更新
        public override void Update(float deltaTime) 
        {
            if (Time.time - mTime > 3)
            {
                mPage += 1;

                mScrollBar.value = (mPage % 3) * 0.5f;
            }
        }

        //打开英雄资料
        private void OnHeroDateClick()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_HeroInfo);

            HeroDatumCtrl.Instance.Enter();
        }

        public void OnQuickPlayHum()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_FastMarch);

            HomePageCtrl.Instance.AskQuickPlay(1001, EBattleMatchType.EBMT_Normal);
        }

        public void OnQuickPlayAI()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_FastAIMarch);

            HomePageCtrl.Instance.AskQuickPlay(1001, EBattleMatchType.EBMT_Ai);
        }

        public void OnRuneEquipClick()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Rune);

            RuneEquipCtrl.Instance.Enter();
        }

        public void OnDaliyBonusBtnClick()
        {
            GameLog.Instance.AddUIEvent(GameLog.UIEventType.UIEventType_Daily);

            DailyBonusCtrl.Instance.Enter();
        }

        //切页
        private void OnPicChange()
        {
            if (mScrollBar.value < 0.3)
            {
                ChangePageCount(1);
            }
            else if (mScrollBar.value < 0.6)
            {
                ChangePageCount(2);
            }
            else
            {
                ChangePageCount(3);
            }

            
        }

        //显示页码
        private void ChangePageCount(int i)
        {
            if (mCurPage == i)
                return;

            mPoint1.SetActive(false);
            mPoint2.SetActive(false);
            mPoint3.SetActive(false);

            switch (i)
            {
                case 1:
                    mPoint1.SetActive(true);

                    //mFocusPic.transform.localPosition = new Vector3(514, mFocusPic.transform.localPosition.y, mFocusPic.transform.localPosition.z);
                    //mFocusPic.clipOffset = new Vector2(-514, mFocusPic.clipOffset.y);
                    break;
                case 2:
                    mPoint2.SetActive(true);
                    //mFocusPic.transform.localPosition = new Vector3(0, mFocusPic.transform.localPosition.y, mFocusPic.transform.localPosition.z);
                    //mFocusPic.clipOffset = new Vector2(0, mFocusPic.clipOffset.y);
                    break;
                case 3:
                    mPoint3.SetActive(true);
                    //mFocusPic.transform.localPosition = new Vector3(-514, mFocusPic.transform.localPosition.y, mFocusPic.transform.localPosition.z);
                   // mFocusPic.clipOffset = new Vector2(514, mFocusPic.clipOffset.y);
                    break;
            }

            mCurPage = i;
            mTime = Time.time;
        }

        private UIButton mQuickPlayHum;
        private UIButton mQuickPlayAI;
        private UIButton m_RuneEuip_Button;
        private UIButton mDailyBonusBtn;
        private UIButton mHeroDate;
        private UIButton mActivities;
        private UIButton mVIPCard;
        private UIButton mLuxuryPack;
        private UIButton mCharge;
        
        private UIPanel mFocusPic;
        private UIScrollBar mScrollBar;
        private GameObject mPoint1;
        private GameObject mPoint2;
        private GameObject mPoint3;

        float mTime = 0;
        int mPage = 1;
        int mCurPage = 1;
    }

}
