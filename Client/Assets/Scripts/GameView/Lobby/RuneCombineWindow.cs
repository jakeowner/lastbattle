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
using System.Timers;

namespace BlGame.View
{
    public class nowAllRuneGOCache
    {
        public GameObject go
        {
            set;
            get;
        }
        public int level
        {
            set;
            get;
        }
        public uint id
        {
            set;
            get;
        }
    }

    public class SlotInfo
    {
        public GameObject runeInfo
        {
            set;
            get;
        }
        public UISprite sprite
        {
            set;
            get;
        }
        public uint runeID
        {
            set;
            get;
        }
    };

    public class RuneCombineWindow : BaseWindow
    {
        UIButton m_CombineBtn;
        UISprite[] m_Slot_SpriteArray;
        UIButton m_Close_Btn;
        UIButton mPopupListBtn;
        UILabel m_PopList_Label;
        GameObject m_RuneLevelGo;
        GameObject[] m_LevelLabelArray;
        UIScrollView mScroll;
        UIGrid mGrid;
        string m_SlotSpriteFileStr;
        uint m_CombinedID = 0;
        int m_CurLevel = 0;
        int m_tryToCombineRuneLevel = 0;
        GameObject m_CostGO;
        UILabel m_CostLabel;
        public Dictionary<string, SlotInfo> slot2RuneGoDict = new Dictionary<string, SlotInfo>();   // 符文槽上对应的符文
        public Dictionary<uint, nowAllRuneGOCache> nowAllRuneGO = new Dictionary<uint, nowAllRuneGOCache>();//当前等级存在的所有符文!等级不符合的全部删除！！
        public Dictionary<uint, int> runeOnTempSlotDic = new Dictionary<uint, int>();//暂时装在符文槽的符文
        private GameObject m_CombineSuccessFlash1GO;
        private enum RuneComposeState
        {
            stepNone = 0,
            step0,
            step1,
            step2,
            step3,
        }
        private RuneComposeState m_RuneComposeState = RuneComposeState.stepNone;
        private float m_ComposeTime = 0;

        public RuneCombineWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadRuneCombineWindowRes;
        }
        static int[] CombineCost = {0,10,20,100,300,1200}; 
        private void IsCanCombine(bool flag)
        {
            m_CombineBtn.isEnabled = flag;
            if (flag)
            {
                m_CostGO.SetActive(true);
                int totGold = 0;
                foreach (var kv in slot2RuneGoDict)
                {
                    var cfg = ConfigReader.GetRuneFromID(kv.Value.runeID);
                    totGold = CombineCost[cfg.level];
                    break;
                }
                m_CostLabel.text = totGold.ToString();
            }
            else
            {
                m_CostGO.SetActive(false);
            }
        }

        string effectName1 = "effectName1";
        string effectName2 = "effectName2";
        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_RuneCombineWindowEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_RuneCombineWindowExit, Hide);
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
            m_RuneComposeState = RuneComposeState.stepNone;
        }

        private bool IfCanShow(int level)
        {
            return m_CurLevel == 0 || m_CurLevel == level;
        }
        float[] ComposeStateWaitTime = {0.0f, 0.5f, 1f, 1f };
        public override void Update(float deltaTime)
        {
            switch (m_RuneComposeState)
            {
                case RuneComposeState.step0:
                    {
                        var curTime = Time.time;
                        if (curTime - m_ComposeTime >= ComposeStateWaitTime[(int)m_RuneComposeState])
                        {
                            m_RuneComposeState = RuneComposeState.step1;
                            foreach (var kv in slot2RuneGoDict)
                            {
                                kv.Value.sprite.spriteName = "";
                                AudioManager.Instance.PlatUnloadRuneAudio();
                            }

                            slot2RuneGoDict.Clear();
                            runeOnTempSlotDic.Clear();
                        }
                    }
                    break;
                case RuneComposeState.step1:
                    {
                        var curTime = Time.time;

                        if (curTime - m_ComposeTime >= ComposeStateWaitTime[(int)m_RuneComposeState])
                        {
                            LoadUiResource.ClearOneChild(m_CombineSuccessFlash1GO.transform, effectName1);

                            m_RuneComposeState = RuneComposeState.step2;
                            var cfg = ConfigReader.GetRuneFromID(m_CombinedID);

                            m_Slot_SpriteArray[GameDefine.GameConstDefine.MaxCombineSlotNum - 1].gameObject.SetActive(true);
                            m_Slot_SpriteArray[GameDefine.GameConstDefine.MaxCombineSlotNum - 1].spriteName = cfg.Icon;

                            m_ComposeTime = curTime;

                            var go = LoadUiResource.AddChildObject(m_CombineSuccessFlash1GO.transform, GameConstDefine.RuneDisappearFlash);
                            go.name = effectName2;
                        }
                    }

                    break;
                case RuneComposeState.step2:
                    {
                        var curTime = Time.time;
                        if (curTime - m_ComposeTime >= ComposeStateWaitTime[(int)m_RuneComposeState])
                        {
                            LoadUiResource.ClearOneChild(m_CombineSuccessFlash1GO.transform, effectName2);
                            m_RuneComposeState = RuneComposeState.step3;

                            m_Slot_SpriteArray[GameDefine.GameConstDefine.MaxCombineSlotNum - 1].spriteName = "";

                            m_ComposeTime = curTime;

                            RuneConfigInfo sRuneConfigInfo = ConfigReader.GetRuneFromID(m_CombinedID);

                            var cfg = ConfigReader.GetRuneFromID(m_CombinedID);

                            if (IfCanShow(sRuneConfigInfo.level))
                            {
                                var oneRuneInfo = MarketRuneListModel.Instance.GetOneRuneInfo(m_CombinedID);
                                LoadOneRune(m_CombinedID, oneRuneInfo.num);
                                mGrid.Reposition();
                                mScroll.ResetPosition();
                            }

                            EventCenter.Broadcast(EGameEvent.eGameEvent_PurchaseSuccessWindowEnter);
                            EventCenter.Broadcast<EPurchaseType, string, string, int>(EGameEvent.eGameEvent_PurchaseRuneSuccessWin, EPurchaseType.EPT_Rune, cfg.RuneName, cfg.Icon, 1);

                            m_RuneComposeState = RuneComposeState.stepNone;
                        }
                    }
 
                    break;
            }
        }
        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_RuneCombineWindowEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_RuneCombineWindowExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_HomePageExit, ParentClose);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            this.ClearWindowData();
            m_Slot_SpriteArray = new UISprite[GameDefine.GameConstDefine.MaxCombineSlotNum];
            string slotPerStr = "CombineSlot/Slot";
            for (int i = 0; i < GameDefine.GameConstDefine.MaxCombineSlotNum; ++i)
            {
                string index = Convert.ToString(i+1);
                string fileStr = slotPerStr + index;
                m_Slot_SpriteArray[i] = mRoot.Find(fileStr).GetComponent<UISprite>();
                m_SlotSpriteFileStr = m_Slot_SpriteArray[i].GetAtlasSprite().name;
                m_Slot_SpriteArray[i].name = Convert.ToString(i);
                m_Slot_SpriteArray[i].spriteName = "";
                UIEventListener.Get(m_Slot_SpriteArray[i].gameObject).onClick += onClickSlot;
            }

            m_Close_Btn = mRoot.Find("CloseBtn").GetComponent<UIButton>();
            EventDelegate.Add(m_Close_Btn.onClick, Hide);

            mPopupListBtn = mRoot.Find("RuneSelect/PopupList").GetComponent<UIButton>();
            EventDelegate.Add(mPopupListBtn.onClick, OnShowLevel);

            m_RuneLevelGo = mRoot.Find("RuneSelect/RuneLevel").gameObject;

            m_LevelLabelArray = new GameObject[GameDefine.GameConstDefine.MaxRuneLevel];
            string resFile = "RuneSelect/RuneLevel/Level";
            for (int i = 0; i < m_LevelLabelArray.GetLength(0); ++i)
            {
                string indexStr = Convert.ToString(i);
                string filename = resFile + indexStr;

                GameObject levelGo = mRoot.Find(filename).gameObject;
                levelGo.name = indexStr;
                UIEventListener.Get(levelGo).onClick += onClickLevel;
                m_LevelLabelArray[i] = levelGo;
            }

            m_CombineBtn = mRoot.Find("CombineBtn").GetComponent<UIButton>();
            EventDelegate.Add(m_CombineBtn.onClick, onClickCombine);

            m_PopList_Label = mRoot.Find("RuneSelect/PopupList/Label").GetComponent<UILabel>();

            mScroll = mRoot.Find("Package").GetComponent<UIScrollView>();
            mGrid = mRoot.Find("Package/Grid").GetComponent<UIGrid>();

            mGrid.sorting = UIGrid.Sorting.Alphabetic;

            m_CombineSuccessFlash1GO = mRoot.Find("Background/Texture5").gameObject;

            m_CostGO = mRoot.Find("Cost").gameObject;
            m_CostLabel = m_CostGO.transform.Find("CostLabel").GetComponent<UILabel>();

            IsCanCombine(false);

            LoadRunes();
        }

        public void onClickSlot(GameObject go)
        {
            if (m_RuneComposeState != RuneComposeState.stepNone)
            {
                return;
            }

            SlotInfo sSlotInfo = null;
            slot2RuneGoDict.TryGetValue(go.transform.name, out sSlotInfo);
            if (sSlotInfo != null)
            {
                sSlotInfo.sprite.spriteName = "";

                var num = updateRuneOnTempSlotDic(sSlotInfo.runeID, -1);
                var oneRuneInfo = MarketRuneListModel.Instance.GetOneRuneInfo(sSlotInfo.runeID);
                var leftNum = oneRuneInfo.num - num;
                var cfg = ConfigReader.GetRuneFromID(sSlotInfo.runeID);
                if (IfCanShow(cfg.level))
                {
                    LoadOneRune(sSlotInfo.runeID, leftNum);
                    mGrid.Reposition();

                    mScroll.ResetPosition();
                }

                slot2RuneGoDict.Remove(go.name);

                go.SetActive(false);

                AudioManager.Instance.PlatUnloadRuneAudio();
            }
        }

        //点击合成符文
        public void onClickCombine()
        {
            if (m_RuneComposeState != RuneComposeState.stepNone)
            {
                return;
            }

            //判断符文消耗是否合法
            long needGold = GameDefine.GameConstDefine.CombineCost[m_tryToCombineRuneLevel-1];
            if (needGold > (long)GameUserModel.Instance.mGameUserGold)
            {
                MsgInfoManager.Instance.ShowMsg(GameDefine.GameConstDefine.LackGoldErrorCode);
                return;
            }

            List<uint> runeList = new List<uint>();
            foreach (var kv in slot2RuneGoDict)
            {
                var runeID = Convert.ToUInt32(kv.Value.runeID);
                var cfg = ConfigReader.GetRuneFromID(runeID);
                if (cfg.level == 5)
                {
                    MsgInfoManager.Instance.ShowMsg(10042);
                    return;
                }
                runeList.Add(runeID);
            }

            CGLCtrl_GameLogic.Instance.AskCombine(runeList);
        }

        private int getLeftShowNum(uint id)
        {
            var oneRuneInfo = MarketRuneListModel.Instance.GetOneRuneInfo(id);
            int leftNum = oneRuneInfo.num;

            if (runeOnTempSlotDic.ContainsKey(id))
            {
                return leftNum - runeOnTempSlotDic[id];
            }

            return leftNum;
        }

        //符文等级过滤
        public void onClickLevel(GameObject go)
        {
            m_CurLevel = Convert.ToInt32(go.name);
            var allRuneList = MarketRuneListModel.Instance.GetBuyedRuneDict();
            if (m_CurLevel == 0)
            {
                m_PopList_Label.text = "全部";
                foreach (var val in allRuneList)
                {
                    if (!nowAllRuneGO.ContainsKey(val.Key))
                    {
                        int num = getLeftShowNum(val.Key);
                        if (num > 0)
                        {
                            LoadOneRune(val.Key, num);
                        }
                    }
                }
            }
            else
            {
                m_PopList_Label.text = "等级" + go.name;
                foreach (var kv in allRuneList)
                {
                    bool ifContine = nowAllRuneGO.ContainsKey(kv.Key);
                    var cfg = ConfigReader.GetRuneFromID(kv.Key);
                    if (cfg.level == m_CurLevel)
                    {
                        if (!ifContine)
                        {
                            int num = getLeftShowNum(kv.Key);
                            if (num > 0)
                            {
                                LoadOneRune(kv.Key, num);
                                mGrid.Reposition();
                            }
                        }
                    }
                    else
                    {
                        if (ifContine)
                        {
                            var info = nowAllRuneGO[kv.Key];
                            info.go.transform.parent = null;
                            mGrid.RemoveChild(info.go.transform);
                            nowAllRuneGO.Remove(kv.Key);
                            MonoBehaviour.DestroyImmediate(info.go);
                        }
                    }
                }
            }

            mGrid.Reposition();

            mScroll.ResetPosition();

            m_RuneLevelGo.SetActive(false);
        }

        protected void OnShowLevel()
        {
            if (m_RuneLevelGo.active)
                m_RuneLevelGo.SetActive(false);
            else
                m_RuneLevelGo.SetActive(true);
        }

        public void OnCloseInterface()
        {
            Hide();
        }

        private void ClearWindowData()
        {
            slot2RuneGoDict.Clear();
            if (m_CombineBtn != null)
                IsCanCombine(false);
            m_CurLevel = 0;
            runeOnTempSlotDic.Clear();
            nowAllRuneGO.Clear();
        }

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener<uint, int, long>(EGameEvent.eGameEvent_RuneBagUpdate, RefeshRuneBagInfo);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener<uint, int, long>(EGameEvent.eGameEvent_RuneBagUpdate, RefeshRuneBagInfo);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        public void RefeshRuneBagInfo(uint runeId, int num, long gottime)
        {
            //如果在临时合成槽里，则删除对应图标
            if (!runeOnTempSlotDic.ContainsKey(runeId))
            {
                //生成新符文
                AddTempCombineRune(runeId, gottime, num);
                IsCanCombine(false);
            }
        }

        private void playCombineSuccessFlash()
        {
            var go = LoadUiResource.AddChildObject(m_CombineSuccessFlash1GO.transform, GameConstDefine.RuneCombineFlash);
            go.name = effectName1;
            m_ComposeTime = Time.time;
            m_RuneComposeState = RuneComposeState.step0;
        }

        private void AddTempCombineRune(uint runeId, long gottime, int num)
        {
            m_CombinedID = runeId;
            
            playCombineSuccessFlash();
        }

        //显示
        public override void OnEnable()
        {
        }

        //隐藏
        public override void OnDisable()
        {
        }

        public void LoadRunes()
        {
            var runeList = MarketRuneListModel.Instance.GetBuyedRuneDict();
            foreach (var kv in runeList)
            {
                if (kv.Value.num == 0)
                {
                    continue;
                }

                RuneConfigInfo sRuneConfigInfo;
                if (!ConfigReader.runeXmlInfoDict.TryGetValue((uint)kv.Key, out sRuneConfigInfo))
                {
                    return;
                }

                LoadOneRune((UInt32)kv.Key, kv.Value.num);
            }

            mGrid.Reposition();

            mScroll.ResetPosition();
        }

        private int updateRuneOnTempSlotDic(uint id, int num)
        {
            if (num < 0)
            {
                IsCanCombine(false);
            }

            if (runeOnTempSlotDic.ContainsKey(id))
            {
                int val = runeOnTempSlotDic[id];
                val += num;
                if (val == 0)
                {
                    runeOnTempSlotDic.Remove(id);
                }
                else
                {
                    runeOnTempSlotDic[id] = val;
                }

                return val;
            }
            else
            {
                runeOnTempSlotDic[id] = num;
                return num;
            }
        }

        //点击左侧背包，准备合成
        public void onClickRuneCard(GameObject go)
        {
            if (m_RuneComposeState != RuneComposeState.stepNone)
            {
                return;
            }

            uint runeID = Convert.ToUInt32(go.name);
            var oneRuneInfo = MarketRuneListModel.Instance.GetOneRuneInfo(runeID);
            var cfg = ConfigReader.GetRuneFromID(runeID);

            for (int i = 0; i < GameDefine.GameConstDefine.MaxCombineSlotNum - 1; ++i)
            {
                if (!slot2RuneGoDict.ContainsKey(m_Slot_SpriteArray[i].gameObject.name))
                {
                    m_Slot_SpriteArray[i].gameObject.SetActive(true);
                    m_Slot_SpriteArray[i].spriteName = cfg.Icon;

                    SlotInfo SInfo = new SlotInfo();
                    SInfo.sprite = m_Slot_SpriteArray[i];
                    SInfo.runeID = runeID;

                    slot2RuneGoDict[m_Slot_SpriteArray[i].name] = SInfo;

                    var num = updateRuneOnTempSlotDic(runeID, 1);
                    var leftNum = oneRuneInfo.num - num;

                    UILabel levelLabel = go.transform.Find("NumLabel").GetComponent<UILabel>();
                    levelLabel.text = Convert.ToString(leftNum);

                    if (leftNum == 0)
                    {
                        go.transform.parent = null;
                        mGrid.RemoveChild(go.transform);

                        mGrid.Reposition();
                        nowAllRuneGO.Remove(runeID);
                        MonoBehaviour.DestroyImmediate(go);
                    }

                    break;
                }
            }

            if (slot2RuneGoDict.Count == 3)
            {
                int[] level = {0,0,0};
                int index = 0;
                foreach (var kv in slot2RuneGoDict)
                {
                    var tempCfg = ConfigReader.GetRuneFromID(kv.Value.runeID);
                    level[index] = tempCfg.level;
                    ++index;
                }

                bool bIfSameID = runeOnTempSlotDic.Count == 1;
                if (!bIfSameID)
                {
                    bool ifSameLevel = (level[0] == level[1]) && (level[0] == level[2]);
                    if (!ifSameLevel)
                    {
                        return;
                    }
                }

                if (level[0] > 5)
                {
                    return;
                }

                m_tryToCombineRuneLevel = level[0];
                IsCanCombine(true);
            }
        }

        private void LoadOneRune(uint runeid, int num)
        {
            RuneConfigInfo sRuneConfigInfo = ConfigReader.GetRuneFromID(runeid);

            bool isNewRune = !nowAllRuneGO.ContainsKey(runeid);
            if (isNewRune)
            {
                ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadGameMarketRuneItem, ResourceType.PREFAB);
                GameObject obj = GameObject.Instantiate(objUnit.Asset) as GameObject;

                UIEventListener.Get(obj).onClick += onClickRuneCard;

                obj.transform.parent = mGrid.transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

                obj.name = (runeid).ToString();

                UISprite icon = obj.transform.Find("Icon").GetComponent<UISprite>();
                icon.spriteName = sRuneConfigInfo.Icon;

                UILabel levelLabel = obj.transform.Find("NumLabel").GetComponent<UILabel>();
                levelLabel.text = Convert.ToString(num);

                UILabel desptionLabel = obj.transform.Find("StausLabel").GetComponent<UILabel>();
                desptionLabel.text = sRuneConfigInfo.Description;

                nowAllRuneGOCache snowAllRuneGOCache = new nowAllRuneGOCache();
                snowAllRuneGOCache.go = obj;
                snowAllRuneGOCache.id = runeid;
                snowAllRuneGOCache.level = sRuneConfigInfo.level;

                nowAllRuneGO.Add(runeid, snowAllRuneGOCache);
            }
            else
            {
                var obj = nowAllRuneGO[runeid];

                UILabel levelLabel = obj.go.transform.Find("NumLabel").GetComponent<UILabel>();
                levelLabel.text = Convert.ToString(num);
            }
        }
    }
}
