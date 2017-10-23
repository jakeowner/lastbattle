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
using System.Linq;
using BlGame.Model;


namespace BlGame.View
{
    public enum EPurchaseType
    {
        EPT_Hero,
        EPT_Rune,
        EPT_Card,
        EPT_Scroll,
        EPT_TrialCard,
        EPT_Gold,
        EPT_Crystal,
    }

    public class PurchaseSuccessWindow : BaseWindow
    {
        public class OneUIData
        {
            public GameObject ChildRoot;
            public UILabel NameLabel;
            public UILabel NumLabel;
            public UISprite IconSprite;
        }

        public Dictionary<EPurchaseType,OneUIData> mChildren = new Dictionary<EPurchaseType,OneUIData>();
        GameObject m_CloseGO;

        public PurchaseSuccessWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.PurchaseSuccessTipRes;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_PurchaseSuccessWindowEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_PurchaseSuccessWindowExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_HomePageExit, ParentClose);
        }

        void ParentClose()
        {
            mResident = false;
            Hide();
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_PurchaseSuccessWindowEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_PurchaseSuccessWindowExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_HomePageExit, ParentClose);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            ClearWindowData();
            mChildren.Clear();
            {//EPT_Hero
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Hero").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                //one.NumLabel = one.ChildRoot.transform.FindChild("NumLabel").GetComponent<UILabel>();
                one.IconSprite = one.ChildRoot.transform.GetComponent<UISprite>();
                mChildren.Add(EPurchaseType.EPT_Hero, one);
            }
            {//EPT_Rune
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Rune").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                one.NumLabel = one.ChildRoot.transform.Find("NumLabel").GetComponent<UILabel>();
                one.IconSprite = one.ChildRoot.transform.GetComponent<UISprite>();
                mChildren.Add(EPurchaseType.EPT_Rune, one);
            }
            {//EPT_Card
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Card").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                //one.NumLabel = one.ChildRoot.transform.FindChild("NumLabel").GetComponent<UILabel>();
                mChildren.Add(EPurchaseType.EPT_Card, one);
            }
            {//EPT_Scroll
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Scroll").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                //one.NumLabel = one.ChildRoot.transform.FindChild("NumLabel").GetComponent<UILabel>();
                mChildren.Add(EPurchaseType.EPT_Scroll, one);
            }
            {//EPT_TrialCard
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/TrialCard").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                //one.NumLabel = one.ChildRoot.transform.FindChild("NumLabel").GetComponent<UILabel>();
                mChildren.Add(EPurchaseType.EPT_TrialCard, one);
            }
            {//EPT_Gold
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Gold").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                one.NumLabel = one.ChildRoot.transform.Find("NumLabel").GetComponent<UILabel>();
                mChildren.Add(EPurchaseType.EPT_Gold, one);
            }
            {//EPT_Crystal
                OneUIData one = new OneUIData();
                one.ChildRoot = mRoot.Find("Position/Crystal").gameObject;
                one.NameLabel = one.ChildRoot.transform.Find("NameLabel").GetComponent<UILabel>();
                one.NumLabel = one.ChildRoot.transform.Find("NumLabel").GetComponent<UILabel>();
                mChildren.Add(EPurchaseType.EPT_Crystal, one);
            }

            m_CloseGO = mRoot.Find("Black").gameObject;
            UIEventListener.Get(m_CloseGO).onClick += onClickClose;
        }

        private void onClickClose(GameObject go)
        {
            Hide();
        }

        public void UpdatePurchase(EPurchaseType type, string name, string icon, int count)
        {
            foreach (KeyValuePair<EPurchaseType, OneUIData> one in mChildren)
            {
                one.Value.ChildRoot.SetActive(false);
            }

            OneUIData current = mChildren[type];
            current.ChildRoot.SetActive(true);
            current.NameLabel.text = name;
            if (current.IconSprite!=null)
            {
                current.IconSprite.spriteName = icon;
            }
            if (current.NumLabel!=null)
            {
                current.NumLabel.text = "x" + count.ToString();
            }
        }

        public void OnCloseInterface()
        {
            Hide();
        }

        private void ClearWindowData()
        {
        }

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener<EPurchaseType, string, string, int>(EGameEvent.eGameEvent_PurchaseRuneSuccessWin, UpdatePurchase);
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener<EPurchaseType, string, string, int>(EGameEvent.eGameEvent_PurchaseRuneSuccessWin, UpdatePurchase);
        }

        //显示
        public override void OnEnable()
        {
        }

        //隐藏
        public override void OnDisable()
        {
        }


    }
}
