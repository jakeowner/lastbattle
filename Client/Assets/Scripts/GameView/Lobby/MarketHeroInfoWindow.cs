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
using BlGame.Model;
using BlGame.GameEntity;
using BlGame.GuideDate;
using BlGame.Resource;
using BlGame.Ctrl;
using System.Linq;


namespace BlGame.View
{
    public class MarketHeroInfoWindow : BaseWindow
    {
        public MarketHeroInfoWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameMarketHeroInfoUI;
            mResident = false;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketHeroInfoEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketHeroInfoExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketHeroInfoEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketHeroInfoExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            m_SelectToggle = mRoot.Find("CheckBoxButton/Skills").GetComponent<UIToggle>();
            mBgStroySelect = mRoot.Find("CheckBoxButton/Story").GetComponent<UIToggle>();
            mUseWaySelect = mRoot.Find("CheckBoxButton/Tutorial").GetComponent<UIToggle>();
            mSkinSelect = mRoot.Find("CheckBoxButton/Skins").GetComponent<UIToggle>();
            mBtnClose = mRoot.Find("CloseBtn").GetComponent<UIButton>();

            mHeroTrickInfo = mRoot.Find("Interface/Tutorial/Label").GetComponent<UILabel>();
            //ui info
            Transform iTface = mRoot.Find("Interface");
            mSpriteHead = iTface.Find("Info/Portrait/Pic").GetComponent<UISprite>();
            mNameHead = iTface.Find("Info/Name").GetComponent<UILabel>();

            mSpriteSkill_1 = iTface.Find("Skills/Skill1/Icon").GetComponent<UISprite>();
            mSpriteSkill_2 = iTface.Find("Skills/Skill2/Icon").GetComponent<UISprite>();
            mSpriteSkill_3 = iTface.Find("Skills/Skill3/Icon").GetComponent<UISprite>();
            mLabelSkill_1 = iTface.Find("Skills/Skill1/Instruction").GetComponent<UILabel>();
            mLabelSkill_2 = iTface.Find("Skills/Skill2/Instruction").GetComponent<UILabel>();
            mLabelSkill_3 = iTface.Find("Skills/Skill3/Instruction").GetComponent<UILabel>();

            mLabelStroy = iTface.Find("Story/Label").GetComponent<UILabel>();
            mLabelUseInfo = iTface.Find("Tutorial/Label").GetComponent<UILabel>();
            mLabelGold = iTface.Find("BuyBtn/Gold/label").GetComponent<UILabel>();
            mLabelDiamond = iTface.Find("BuyBtn/Crystal/label").GetComponent<UILabel>();

            mBtnGold = iTface.Find("BuyBtn/Gold").GetComponent<UIButton>();
            UIGuideCtrl.Instance.AddUiGuideEventBtn(mBtnGold.gameObject);
            mBtnDiamond = iTface.Find("BuyBtn/Crystal").GetComponent<UIButton>();
            InitWindowEvent();
        }

        /// <summary>
        /// 增加界面事件
        /// </summary>
        private void InitWindowEvent()
        {
            EventDelegate.Add(m_SelectToggle.onChange, OnSkillSelect);
            EventDelegate.Add(mBgStroySelect.onChange, OnStroySelect);
            EventDelegate.Add(mUseWaySelect.onChange, OnUseWaySelect);
            EventDelegate.Add(mSkinSelect.onChange, OnSkinSelect);

            EventDelegate.Add(mBtnClose.onClick, OnCloseInterface);
            EventDelegate.Add(mBtnGold.onClick, OnBuyByGold);
            EventDelegate.Add(mBtnDiamond.onClick, OnBuyByDiamond);
            //UIEventListener.Get(mBtnClose).onClick += OnCloseInterface;
            //UIEventListener.Get(mBtnGold).onClick += OnBuyByGold;
            //UIEventListener.Get(mBtnDiamond).onClick += OnBuyByDiamond;
        }


        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener<int>(EGameEvent.eGameEvent_RefreshGoodHero, RefreshMarketHeroInfo);
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener<int>(EGameEvent.eGameEvent_RefreshGoodHero, RefreshMarketHeroInfo);
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }

        //显示
        public override void OnEnable()
        {
            int goodsId = MarketHeroListCtrl.Instance.GetGoodsSelect();
            mHeroInfo = ConfigReader.GetHeroSelectInfo(ConfigReader.HeroBuyXmlInfoDict[goodsId].UnlockHeroID);
            mSpriteHead.spriteName = mHeroInfo.HeroSelectThumb;
            bool isEnable = GameUserModel.Instance.OwnedHeroInfoDict.ContainsKey(goodsId) ? false : true;
            SetShow(goodsId,  isEnable);
        }

        void SetShow(int goodsId,bool isEnable)
        {
            MarketGoodsInfo mGoodInfo;
            DeltGoodBuyBtnEnabled(isEnable);
            UILabel ual = null;
            if (MarketHeroListModel.Instance.HeroListDict.TryGetValue(goodsId, out mGoodInfo) && isEnable)
            {
                if (mGoodInfo.RLDiamondPrice != 0 && mGoodInfo.RLGoldPrice != 0)
                {
                    HeroBuyConfigInfo heroInfo = ConfigReader.GetHeroBuyInfo(goodsId);
                    if (heroInfo == null)
                        return;
                    bool isVib = GameUserModel.Instance.OwnedHeroInfoDict.ContainsKey(MarketHeroListCtrl.Instance.GetRootGoods()) ? false : true;
                    if (heroInfo.Time == -1 || !isVib || heroInfo.Consume == null || heroInfo.Consume.Count <= 1)
                    {
                        DeltGoodBuyBtnEnabled(false);
                        return;
                    }
                    
                    ual = mBtnGold.transform.Find("label").GetComponent<UILabel>();
                    ual.text = heroInfo.Consume.ElementAt(0).Value.ToString();
                    ual = mBtnDiamond.transform.Find("label").GetComponent<UILabel>();
                    ual.text = heroInfo.Consume.ElementAt(1).Value.ToString();
                }
                else
                {
                    if (mGoodInfo.RLDiamondPrice == 0)
                        mBtnDiamond.gameObject.SetActive(false);
                    mBtnDiamond.transform.Find("label").GetComponent<UILabel>().text = mGoodInfo.RLDiamondPrice.ToString();
                    if (mGoodInfo.RLGoldPrice == 0)
                    {
                        mBtnGold.gameObject.SetActive(false);
                    }
                    mBtnGold.transform.Find("label").GetComponent<UILabel>().text = mGoodInfo.RLGoldPrice.ToString();
                    mBtnGold.transform.localPosition = new Vector3(-190f, 29f, 0);
                }
            }
            
        }

        //隐藏
        public override void OnDisable()
        {

        }

        /// <summary>
        /// yingx购买的按钮是否可用
        /// </summary>
        /// <param name="isEnable"></param>
        /// 是否可用
        private void DeltGoodBuyBtnEnabled(bool isEnable)
        {
            mBtnDiamond.gameObject.SetActive(isEnable);
            mBtnGold.gameObject.SetActive(isEnable);
        }

        /// <summary>
        /// 刷新商城的英雄显示信息
        /// </summary>
        /// <param name="goodsId"></param>
        private void RefreshMarketHeroInfo(int goodsId)
        {
            DeltGoodBuyBtnEnabled(MarketHeroListCtrl.Instance.GetGoodsSelect() != goodsId);
            SetShow(goodsId, MarketHeroListCtrl.Instance.GetGoodsSelect() != goodsId);
        }

        void OnBuyByGold()
        {
            MarketGoodsInfo mGoodInfo;
            int goodsId = MarketHeroListCtrl.Instance.GetGoodsSelect();
            bool isEnable = GameUserModel.Instance.OwnedHeroInfoDict.ContainsKey(goodsId) ? false : true;
            if (MarketHeroListModel.Instance.HeroListDict.TryGetValue(goodsId, out mGoodInfo) && isEnable)
            {
                if (GameUserModel.Instance.mGameUserGold < (ulong)mGoodInfo.RLGoldPrice)
                {
                    MsgInfoManager.Instance.ShowMsg(-130902);
                    return;
                } 
            }
            MarketHeroListCtrl.Instance.MarketHeroAskBuyGoods(MarketHeroListCtrl.Instance.GetGoodsSelect(), GameDefine.ConsumeType.TypeGold);
        }

        void OnBuyByDiamond()
        {
            MarketGoodsInfo mGoodInfo;
            int goodsId = MarketHeroListCtrl.Instance.GetGoodsSelect();
            bool isEnable = GameUserModel.Instance.OwnedHeroInfoDict.ContainsKey(goodsId) ? false : true;
            if (MarketHeroListModel.Instance.HeroListDict.TryGetValue(goodsId, out mGoodInfo) && isEnable)
            {
                if (GameUserModel.Instance.mGameUserDmd < (ulong)mGoodInfo.RLDiamondPrice)
                {
                    MsgInfoManager.Instance.ShowMsg(-130771);
                    return;
                }
            }
            MarketHeroListCtrl.Instance.MarketHeroAskBuyGoods(MarketHeroListCtrl.Instance.GetGoodsSelect(), GameDefine.ConsumeType.TypeDiamond);
        }

        public void OnSkillSelect()
        {
            if (m_SelectToggle.value)
            {
                mSpriteSkill_1.spriteName = mHeroInfo.HeroSelectSkill1;
                mSpriteSkill_2.spriteName = mHeroInfo.HeroSelectSkill2;
                mSpriteSkill_3.spriteName = mHeroInfo.HeroSkillPass;
                mLabelSkill_1.text = mHeroInfo.HeroSelectDes;
                mLabelSkill_2.text = mHeroInfo.HeroSelectDes2;
                mLabelSkill_3.text = mHeroInfo.HeroPassDes;
                mNameHead.text = mHeroInfo.HeroSelectNameCh;
            }
        }

        public void OnStroySelect()
        {
            if (mBgStroySelect.value)
            {
                mLabelStroy.text = mHeroInfo.HeroSelectDesSkill;
            }
        }

        public void OnUseWaySelect()
        {
            if (mUseWaySelect.value)
            {
                mHeroTrickInfo.text = mHeroInfo.HeroSelectBuyDes;
            }
        }

        public void OnSkinSelect()
        {
            if (mSkinSelect.value)
            {

            }
        }

        public void OnCloseInterface()
        {
            MarketHeroInfoCtrl.Instance.Exit();
        }

        HeroSelectConfigInfo mHeroInfo;

        UIToggle m_SelectToggle;
        UIToggle mBgStroySelect;
        UIToggle mUseWaySelect;
        UIToggle mSkinSelect;

        UISprite mSpriteHead;
        UILabel mNameHead;

        UISprite mSpriteSkill_1;
        UISprite mSpriteSkill_2;
        UISprite mSpriteSkill_3;
        UILabel mLabelSkill_1;
        UILabel mLabelSkill_2;
        UILabel mLabelSkill_3;
        UILabel mLabelStroy;
        UILabel mLabelUseInfo;
        UILabel mLabelGold;
        UILabel mLabelDiamond;

        UIButton mBtnClose;
        UIButton mBtnGold;
        UIButton mBtnDiamond;
        UILabel mHeroTrickInfo;

    }

}

