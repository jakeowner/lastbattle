using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using GameDefine;
using JT.FWW.Tools;
using JT.FWW.GameData;
using UICommon;
using BlGame.GameData;
using BlGame;
using BlGame.GameEntity;
using BlGame.GuideDate;
using BlGame.Resource;
using BlGame.Ctrl;

namespace BlGame.View
{
    public class MarketWindow : BaseWindow
    {
        public MarketWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameMarketUI;
            mResident = true;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            mRecommendGoods = mRoot.Find("MenuBtn/Recommend").GetComponent<UIToggle>();
            mHeroGoods = mRoot.Find("MenuBtn/Hero").GetComponent<UIToggle>();
            mRuneGoods = mRoot.Find("MenuBtn/Rune").GetComponent<UIToggle>();
            mTimeLimitHero = mRoot.Find("MenuBtn/TimeLimitHero").GetComponent<UIToggle>();
            UIGuideCtrl.Instance.AddUiGuideEventBtn(mHeroGoods.gameObject);
            UIGuideCtrl.Instance.AddUiGuideEventBtn(mRuneGoods.gameObject);
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }

     
        //游戏事件注册
        protected override void OnAddListener()
        {
            EventDelegate.Add(mRecommendGoods.onChange, OnRecomment);
            EventDelegate.Add(mHeroGoods.onChange, OnHero);
            EventDelegate.Add(mRuneGoods.onChange, OnRune);
            EventDelegate.Add(mTimeLimitHero.onChange, OnTimeLimitHero);
        }

        private void OnTimeLimitHero()
        {
            if (mTimeLimitHero.value)
            {
                HeroTimeLimitCtrl.Instance.Enter();
            }
            else
            {
                HeroTimeLimitCtrl.Instance.Exit();
            }
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventDelegate.Remove(mRecommendGoods.onChange, OnRecomment);
            EventDelegate.Remove(mHeroGoods.onChange, OnHero);
            EventDelegate.Remove(mRuneGoods.onChange, OnRune);
            EventDelegate.Remove(mTimeLimitHero.onChange, OnTimeLimitHero);
           
        }

        //显示
        public override void OnEnable()
        {
            if (!mHeroGoods.value)
            {
                mHeroGoods.value = true;
            }
            OnHero();
        }

        //隐藏
        public override void OnDisable()
        {
            
        }

        public void OnRecomment()
        {
            if (mRecommendGoods.value)
            {
                 
            }
        }

        public void OnHero()
        {
            if (mHeroGoods.value)
            {
                MarketHeroListCtrl.Instance.Enter();
            }
            else
            {
                MarketHeroListCtrl.Instance.Exit();
            }
        }

        public void OnRune()
        {
            if (mRuneGoods.value)
            {
                MarketRuneListCtrl.Instance.Enter();
            }
            else
            {
                MarketRuneListCtrl.Instance.Exit();
            }
        }
         
        UIToggle mRecommendGoods;
        UIToggle mHeroGoods;
        UIToggle mRuneGoods;
        UIToggle mTimeLimitHero;
 }

}
