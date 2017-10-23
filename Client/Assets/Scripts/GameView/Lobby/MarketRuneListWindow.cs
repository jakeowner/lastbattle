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
    public class MarketRuneListWindow : BaseWindow
    {
        public MarketRuneListWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameMarketRuneListUI;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketRuneListEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketRuneListExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_MarketExit, ParentClose);
        }

        void ParentClose()
        {
            mResident = false;
            Hide();
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
            runeListObjects.Clear();
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketRuneListEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketRuneListExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_MarketExit, ParentClose);
            runeListObjects.Clear();
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            this.ClearWindowData();

            mPopupListBtn = mRoot.Find("SelectKind/PopupList").GetComponent<UIButton>();

            mScroll = mRoot.Find("RuneScrollView").GetComponent<UIScrollView>();
            mGrid = mRoot.Find("RuneScrollView/OptionItems").GetComponent<UIGrid>();
            m_PopList_Label = mRoot.Find("SelectKind/PopupList/Label").GetComponent<UILabel>();
            
            EventDelegate.Add(mPopupListBtn.onClick, OnShowLevel);

            if (!mScroll.gameObject.activeInHierarchy)
            {
                mScroll.gameObject.SetActive(true);
            }

            m_RuneLevelGo = mRoot.Find("SelectKind/RuneLevel").gameObject;

            mResident = true;

            m_LevelLabelArray = new GameObject[GameDefine.GameConstDefine.MaxRuneLevel];
            string resFile = "SelectKind/RuneLevel/Level";
            for (int i = 0; i < m_LevelLabelArray.GetLength(0); ++i)
            {
                string indexStr = Convert.ToString(i);
                string filename = resFile + indexStr;

                GameObject levelGo = mRoot.Find(filename).gameObject;
                levelGo.name = indexStr;
                UIEventListener.Get(levelGo).onClick += onClickLevel;
                m_LevelLabelArray[i] = levelGo;
            }

            LoadRunes();
        }

        private void ClearWindowData()
        {
        }

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        //显示
        public override void OnEnable()
        {
        }

        //隐藏
        public override void OnDisable()
        {
        }

        void OnSelectRune(GameObject obj)
        {
        }

        protected void OnShowLevel()
        {
            m_RuneLevelGo.SetActive(true);
        }

        protected void OnMagicKind()
        {
        }

        public void onClickRuneCard(GameObject go)
        {
            MarketRuneInfoCtrl.Instance.Enter(go);
        }

        public void onClickLevel(GameObject go)
        {
            int level = Convert.ToInt32(go.name);
            if (level == 0)
            {
                m_PopList_Label.text = "全部";
            }
            else
            {
                m_PopList_Label.text = "等级" + go.name;
            }

            foreach (var tempGo in runeListObjects)
            {
                MonoBehaviour.DestroyImmediate(tempGo);
            }
            runeListObjects.Clear();

            Dictionary<int, RuneGoodsInfo> runesDict = MarketRuneListModel.Instance.GetRuneCfgListDict();
            foreach (RuneGoodsInfo val in runesDict.Values)
            {
                RuneConfigInfo sRuneConfigInfo;
                if (!ConfigReader.runeXmlInfoDict.TryGetValue((UInt32)val.mGoodsId, out sRuneConfigInfo))
                {
                    Debug.LogError("null cfg with runeid " + val.mGoodsId);
                    continue;
                }

                if (level == 0 || sRuneConfigInfo.level == level)
                {
                    LoadOneRune(val, sRuneConfigInfo);
                }
            }

            mGrid.enabled = true;
            mGrid.Reposition();

            mScroll.ResetPosition();

            m_RuneLevelGo.SetActive(false);
        }

        public void LoadRunes()
        {
            runeListObjects.Clear();
            Dictionary<int, RuneGoodsInfo> runesDict = MarketRuneListModel.Instance.GetRuneCfgListDict();
            foreach (RuneGoodsInfo val in runesDict.Values)
            {
                RuneConfigInfo sRuneConfigInfo;
                if (!ConfigReader.runeXmlInfoDict.TryGetValue((UInt32)val.mGoodsId, out sRuneConfigInfo))
                {
                    Debug.LogError("null cfg with runeid " + val.mGoodsId);
                    continue;
                }

                LoadOneRune(val, sRuneConfigInfo);
            }
        }

        private void LoadOneRune(RuneGoodsInfo val, RuneConfigInfo sRuneConfigInfo)
        {
            ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadGameMarketRuneTag, ResourceType.PREFAB);
            GameObject obj = GameObject.Instantiate(objUnit.Asset) as GameObject;

            UIEventListener.Get(obj).onClick += onClickRuneCard;

            obj.transform.parent = mGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.name = (val.mGoodsId).ToString();

            UISprite icon = obj.transform.Find("Icon").GetComponent<UISprite>();
            icon.spriteName = sRuneConfigInfo.Icon;

            UILabel objName = obj.transform.Find("NameLabel").GetComponent<UILabel>();

            objName.text = sRuneConfigInfo.RuneName;

            if (val.RLGoldPrice >= 0)
            {
                Transform goldTransform = obj.transform.Find("Cost/Gold").GetComponent<Transform>();
                goldTransform.gameObject.SetActive(true);
                UILabel goldLabel = goldTransform.Find("LabelMoney").GetComponent<UILabel>();
                goldLabel.text = Convert.ToString(val.RLGoldPrice);
            }
            else if (val.RLDiamondPrice >= 0)
            {
                Transform crystralTransform = obj.transform.Find("Cost/Crystal").GetComponent<Transform>();
                crystralTransform.gameObject.SetActive(true);
                UILabel crystalLabel = crystralTransform.Find("LabelMoney").GetComponent<UILabel>();
                crystalLabel.text = Convert.ToString(val.RLDiamondPrice);
            }

            runeListObjects.Add(obj);
        }

        UIButton mPopupListBtn;
        UILabel m_PopList_Label;
        UIScrollView mScroll;
        UIGrid mGrid;
        GameObject m_RuneLevelGo;
        GameObject[] m_LevelLabelArray;

        public List<GameObject> runeListObjects = new List<GameObject>();//当前显示的符文
    }

}
