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
    public class RuneEquipWindow : BaseWindow
    {
        public RuneEquipWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadRuneEquipWindowRes;

            m_CurPageNum = 0;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_RuneEquipWindowEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_RuneEquipWindowExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_HomePageExit, ParentClose);
        }

        void ParentClose()
        {
            mResident = false;
            Hide();
        }

        private void DestoryResGO1(int i)
        {
            if (m_Slot_Sprite_Array[i].resGO1 != null)
            {
                MonoBehaviour.DestroyImmediate(m_Slot_Sprite_Array[i].resGO1);
                m_Slot_Sprite_Array[i].resGO1 = null;
            }
        }
        private void DestoryResGO2(int i)
        {
            if (m_Slot_Sprite_Array[i].resGO2 != null)
            {
                MonoBehaviour.DestroyImmediate(m_Slot_Sprite_Array[i].resGO2);
                m_Slot_Sprite_Array[i].resGO2 = null;
            }
        }
        //窗口控件释放
        protected override void RealseWidget()
        {
            runeSlotListObjects.Clear();
            effectDict.Clear();
            m_CurLevel = 0;
            ifFirstLoad = true;
            for (int i = 0; i < GameDefine.GameConstDefine.MaxRuneSlot; ++i)
            {
                DestoryResGO1(i);
                DestoryResGO2(i);
            }
            addFlashMap.Clear();
            nowAllRuneGO.Clear();
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_RuneEquipWindowEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_RuneEquipWindowExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_HomePageExit, ParentClose);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            var userLevel = GameUserModel.Instance.UserLevel;
            var unlockSlotNum = userLevel / 3;

            m_CloseBtn = mRoot.Find("CloseBtn").GetComponent<UIButton>();
            EventDelegate.Add(m_CloseBtn.onClick, OnCloseInterface);

            mPopupListBtn = mRoot.Find("RuneSelect/PopupList").GetComponent<UIButton>();
            EventDelegate.Add(mPopupListBtn.onClick, OnShowLevel);

            m_RuneLevelGo = mRoot.Find("RuneSelect/RuneLevel").gameObject;

            m_PopList_Label = mRoot.Find("RuneSelect/PopupList/Label").GetComponent<UILabel>();

            mScroll = mRoot.Find("Package").GetComponent<UIScrollView>();
            mGrid = mRoot.Find("Package/Grid").GetComponent<UIGrid>();

            m_StatusLabel = mRoot.Find("InfoWindow/Staus").GetComponent<UILabel>();
            m_StatusLabel.text = "";

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

            m_Page1Btn = mRoot.Find("PagesList/Grid/RunePages").GetComponent<UIButton>();
            EventDelegate.Add(m_Page1Btn.onClick, onClickPage);

            m_AddPageBtn = mRoot.Find("PagesList/AddButton").GetComponent<UIButton>();
            EventDelegate.Add(m_AddPageBtn.onClick, onAddPageClick);

            string slotFilePre = "EquipBlank/Slot";
            for (int i = 0; i < GameDefine.GameConstDefine.MaxRuneSlot; ++i)
            {
                string indexName = Convert.ToString(i + 1);
                string slotColliderFile = slotFilePre + indexName;

                GameObject slotGo = mRoot.Find(slotColliderFile).gameObject;
                UIEventListener.Get(slotGo).onClick += onClickSlot;

                string curFileStr = slotColliderFile + "/Icon";
                m_Slot_Sprite_Array[i] = new SlotInfo();
                m_Slot_Sprite_Array[i].sprite = new UISprite();
                m_Slot_Sprite_Array[i].runeid = new uint();
                var tempSprite = mRoot.Find(curFileStr).GetComponent<UISprite>();
                m_Slot_Sprite_Array[i].sprite = tempSprite;
                m_Slot_Sprite_Array[i].sprite.name = indexName;
                m_Slot_Sprite_Array[i].sprite.spriteName = "";
                m_Slot_Sprite_Array[i].lockGO = mRoot.Find(slotColliderFile + "/Lock").gameObject;

                if (i < unlockSlotNum)
                {
                    m_Slot_Sprite_Array[i].lockGO.SetActive(false);
                    m_Slot_Sprite_Array[i].ifCanEquip = true;
                }

                slotGo.name = Convert.ToString(i);
            }

            m_CombineBtn = mRoot.Find("CombineBtn").GetComponent<UIButton>();
            EventDelegate.Add(m_CombineBtn.onClick, onClickCombine);

            m_WashBtn = mRoot.Find("WashBtn").GetComponent<UIButton>();
            EventDelegate.Add(m_WashBtn.onClick, onClickWash);

            m_CurPageNum = 0;

            ifFirstLoad = true;
            LoadRunes();

            LoadEquip();
        }

        static public int MyCustomSort(Transform a, Transform b)
        {
            Int64 c1 = Convert.ToInt64(a.name);
            Int64 c2 = Convert.ToInt64(b.name);
            return c2.CompareTo(c1);
        }

        public void onClickCombine()
        {
            RuneCombineCtrl.Instance.Enter();
        }

        public void onClickWash()
        {
            EventCenter.Broadcast(EGameEvent.eGameEvent_RuneRefeshWindowEnter);
        }

        public void onClickSlot(GameObject go)
        {
            var pos = Convert.ToInt32(go.name);
            if (m_Slot_Sprite_Array[pos].lockGO.active)
            {
                MsgInfoManager.Instance.ShowMsg(10035);
                return;
            }

            if (m_Slot_Sprite_Array[pos].runeid < 1)
            {
                return;
            }
            CGLCtrl_GameLogic.Instance.SendUnloadRune(m_CurPageNum, pos);
        }

        public void onAddPageClick()
        {
            MsgInfoManager.Instance.ShowMsg(10043);
        }

        public void onClickPage()
        {

        }

        public void GetPageAndPos(int nowPos, out int pages, out int pos)
        {
            pages = nowPos / GameDefine.GameConstDefine.MaxRuneSlot;
            pos = nowPos - pages * GameDefine.GameConstDefine.MaxRuneSlot;
        }

        protected void LoadEquip()
        {
            var dict = RuneEquipModel.Instance.GetrunePos2RuneidMap();
            foreach (var kv in dict)
            {
                int page = 0;
                int pos = 0;
                GetPageAndPos(kv.Key, out page, out pos);
                RefeshEquipRuneInfoWhenInit((uint)kv.Value, page, pos);
            }

            UpdateStatusText();
        }

        private void UpdateStatusText()
        {
            m_StatusLabel.text = "";
            foreach (var kv in effectDict)
            {
                m_StatusLabel.text += kv.Value.des;
                m_StatusLabel.text += "\n";
            }
            m_StatusLabel.Update();
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

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener<uint, int, int>(EGameEvent.eGameEvent_RuneQuipUpdate, RefeshEquipRuneInfo);
            EventCenter.AddListener<int, int>(EGameEvent.eGameEvent_RuneQuipUnload, UnloadRune);

            EventCenter.AddListener<uint, int, long>(EGameEvent.eGameEvent_RuneBagUpdate, RefeshRuneBagInfo);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide); 
        }

        public void RefeshRuneBagInfo(uint runeId, int num, long gottime)
        {
            if (num == 0)
            {
                var cache = nowAllRuneGO[runeId];
                cache.go.transform.parent = null;
                MonoBehaviour.DestroyImmediate(cache.go);
                nowAllRuneGO.Remove(runeId);
                mGrid.Reposition();
                mScroll.ResetPosition();
                return;
            }

            var cfg = ConfigReader.GetRuneFromID(runeId);
            if (m_CurLevel == cfg.level || m_CurLevel == 0)
            {
                LoadOneRune(runeId, num);
                if (num == 1)
                {
                    mGrid.Reposition();
                    mScroll.ResetPosition();
                }
            }
        }

        private void AddRes(int i)
        {
            {
                var transFrom = m_Slot_Sprite_Array[i].sprite.gameObject.transform;
                GameObject objLoad = null;

                ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.RuneSlotAddFlash, ResourceType.PREFAB);
                objLoad = GameObject.Instantiate(objUnit.Asset) as GameObject;
                objLoad.transform.parent = transFrom;
                objLoad.transform.localScale = Vector3.one;
                objLoad.transform.localPosition = Vector3.zero;

                m_Slot_Sprite_Array[i].resGO1 = objLoad;
            }

            addFlashMap[i] = Time.time;
        }

        private void RemoveRes(int i)
        {
            if (addFlashMap.ContainsKey(i))
            {
                if (m_Slot_Sprite_Array[i].resGO1)
                {
                    m_Slot_Sprite_Array[i].resGO1.transform.parent = null;
                    MonoBehaviour.DestroyImmediate(m_Slot_Sprite_Array[i].resGO1);
                    m_Slot_Sprite_Array[i].resGO1 = null;
                }

                addFlashMap.Remove(i);
            }
            else
            {
                if (m_Slot_Sprite_Array[i].resGO2)
                {
                    m_Slot_Sprite_Array[i].resGO2.transform.parent = null;
                    MonoBehaviour.DestroyImmediate(m_Slot_Sprite_Array[i].resGO2);
                    m_Slot_Sprite_Array[i].resGO2 = null;
                }
            }
        }
        public void UnloadRune(int page, int pos)
        {
            Debug.Log("page " + page + " pos:" + pos);
            RemoveRes(pos);

            addFlashMap.Remove(pos);

            m_Slot_Sprite_Array[pos].sprite.gameObject.SetActive(false);
            m_Slot_Sprite_Array[pos].sprite.spriteName = "";

            UpdateStatus(m_Slot_Sprite_Array[pos].runeid, false);
            UpdateStatusText();
            AudioManager.Instance.PlatUnloadRuneAudio();

            m_Slot_Sprite_Array[pos].runeid = 0;
        }

        public void RefeshEquipRuneInfo(uint runeid, int sPage, int slotPos)
        {
            if (slotPos > m_Slot_Sprite_Array.Count())
            {
                Debug.LogError("");
                return;
            }

            RuneConfigInfo sRuneConfigInfo = ConfigReader.GetRuneFromID(runeid);
            if (null == sRuneConfigInfo)
            {
                Debug.LogError("");
                return;
            }

            var slot = m_Slot_Sprite_Array[slotPos];
            slot.sprite.gameObject.SetActive(true);
            slot.sprite.spriteName = sRuneConfigInfo.Icon;
            slot.runeid = runeid;

            AddRes(slotPos);

            UpdateStatus(runeid, true);
            UpdateStatusText();
        }

        public void RefeshEquipRuneInfoWhenInit(uint runeid, int sPage, int slotPos)
        {
            if (slotPos > m_Slot_Sprite_Array.Count())
            {
                Debug.LogError("");
                return;
            }

            RuneConfigInfo sRuneConfigInfo = ConfigReader.GetRuneFromID(runeid);
            if (null == sRuneConfigInfo)
            {
                Debug.LogError("");
                return;
            }

            var slot = m_Slot_Sprite_Array[slotPos];
            slot.sprite.gameObject.SetActive(true);
            slot.sprite.spriteName = sRuneConfigInfo.Icon;
            slot.runeid = runeid;

            {
                var transFrom = m_Slot_Sprite_Array[slotPos].sprite.gameObject.transform;
                GameObject objLoad = null;

                ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.RuneSlotEquipLight, ResourceType.PREFAB);
                objLoad = GameObject.Instantiate(objUnit.Asset) as GameObject;
                objLoad.transform.parent = transFrom;
                objLoad.transform.localScale = Vector3.one;
                objLoad.transform.localPosition = Vector3.zero;

                m_Slot_Sprite_Array[slotPos].resGO2 = objLoad;
            }

            UpdateStatus(runeid, true);
            UpdateStatusText();
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener<uint, int, int>(EGameEvent.eGameEvent_RuneQuipUpdate, RefeshEquipRuneInfo);
            EventCenter.RemoveListener<int, int>(EGameEvent.eGameEvent_RuneQuipUnload, UnloadRune);

            EventCenter.RemoveListener<uint, int, long>(EGameEvent.eGameEvent_RuneBagUpdate, RefeshRuneBagInfo);
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
                        if (val.Value.num > 0)
                        {
                            LoadOneRune(val.Key, val.Value.num);
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
                            if (kv.Value.num > 0)
                            {
                                LoadOneRune(kv.Key, kv.Value.num);
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

        public void LoadRunes()
        {
            var runeList = MarketRuneListModel.Instance.GetBuyedRuneDict();
            foreach (var kv in runeList)
            {
                LoadOneRune(kv.Key, kv.Value.num);
            }

            mGrid.enabled = true;

            mGrid.Reposition();

            mScroll.ResetPosition();
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

        public void onClickRuneCard(GameObject go)
        {
            var runeID = Convert.ToUInt32(go.name);
            var userLevel = GameUserModel.Instance.UserLevel;
            int maxCanEquipNum = userLevel / GameDefine.GameConstDefine.MaxUnlockOfOneLevel;
            for (int i = 0; i < GameDefine.GameConstDefine.MaxRuneSlot; ++i)
            {
                if (m_Slot_Sprite_Array[i].ifCanEquip && m_Slot_Sprite_Array[i].runeid == 0)
                {
                    CGLCtrl_GameLogic.Instance.AskEquipRune(runeID, 0);

                    return;
                }
            }

            if (maxCanEquipNum < 10)
                MsgInfoManager.Instance.ShowMsg(10035);
            else
                MsgInfoManager.Instance.ShowMsg(10034);
        }

        public override void Update(float deltaTime)
        {
            if (ifFirstLoad)
            {
                mScroll.UpdatePosition();
                ifFirstLoad = false;
            }

            List<int> removeList = new List<int>();
            foreach (var kv in addFlashMap)
            {
                if (Time.time - kv.Value >= 0.75)
                {
                    m_Slot_Sprite_Array[kv.Key].resGO1.transform.parent = null;
                    MonoBehaviour.DestroyImmediate(m_Slot_Sprite_Array[kv.Key].resGO1);
                    m_Slot_Sprite_Array[kv.Key].resGO1 = null;
                    removeList.Add(kv.Key);

                    {
                        var transFrom = m_Slot_Sprite_Array[kv.Key].sprite.gameObject.transform;
                        GameObject objLoad = null;

                        ResourceUnit objUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.RuneSlotEquipLight, ResourceType.PREFAB);
                        objLoad = GameObject.Instantiate(objUnit.Asset) as GameObject;
                        objLoad.transform.parent = transFrom;
                        objLoad.transform.localScale = Vector3.one;
                        objLoad.transform.localPosition = Vector3.zero;

                        m_Slot_Sprite_Array[kv.Key].resGO2 = objLoad;
                    }
                }
            }

            foreach (var kv in removeList)
            {
                addFlashMap.Remove(kv);
            }
        }

        const double SmallData = 0.00000001;

        private uint GetEffectKey(uint runeID)
        {
            return runeID % 100;
        }

        private void UpdateStatus(uint id, bool ifAdd)
        {
            RuneConfigInfo sRuneConfigInfo = ConfigReader.GetRuneFromID(id);
            var key = GetEffectKey(id);

            if (effectDict.ContainsKey(key))
            {
                var effectVel = effectDict[key];
                var per = effectVel.per;
                var val = effectVel.val;
                if (ifAdd)
                {
                    per += sRuneConfigInfo.effectPer;
                    val += sRuneConfigInfo.effectVal;
                }
                else
                {
                    per -= sRuneConfigInfo.effectPer;
                    val -= sRuneConfigInfo.effectVal;

                    if (Math.Abs(per) < SmallData && Math.Abs(val) < SmallData)
                    {
                        effectDict.Remove(key);
                        return;
                    }
                }

                effectVel.des = RuneConfig.ModifyDes(val, per, sRuneConfigInfo).ToString();
                effectVel.per = per;
                effectVel.val = val;

                effectDict[key] = effectVel;
            }
            else
            {
                if (!ifAdd)
                {
                    Debug.LogError("");
                    return;
                }
                EffectInfo sinfo = new EffectInfo();
                sinfo.per = sRuneConfigInfo.effectPer;
                sinfo.val = sRuneConfigInfo.effectVal;
                sinfo.des = RuneConfig.ModifyDes(sinfo.val, sinfo.per, sRuneConfigInfo).ToString();
                effectDict.Add(key, sinfo);
            }
        }
        public class RuneSlotInfo
        {
            public int slotPos
            {
                set;
                get;
            }
            public int runeID
            {
                set;
                get;
            }
        }

        UIButton mPopupListBtn;
        UILabel m_PopList_Label;
        UIButton m_CloseBtn;
        UIScrollView mScroll;
        UIGrid mGrid;
        GameObject m_RuneLevelGo;
        int m_CurPageNum;
        UIButton m_Page1Btn;
        UIButton m_AddPageBtn;
        UIButton m_CombineBtn;
        UIButton m_WashBtn;
        UILabel m_StatusLabel;
        GameObject[] m_LevelLabelArray;
        SlotInfo[] m_Slot_Sprite_Array = new SlotInfo[GameDefine.GameConstDefine.MaxRuneSlot];
        int m_CurLevel = 0;
        Dictionary<int, float> addFlashMap = new Dictionary<int, float>();

        public Dictionary<uint, nowAllRuneGOCache> nowAllRuneGO = new Dictionary<uint, nowAllRuneGOCache>();//当前等级存在的所有符文!等级不符合的全部删除！！

        public Dictionary<UISprite, RuneSlotInfo> runeSlotListObjects = new Dictionary<UISprite, RuneSlotInfo>();
        Dictionary<uint, EffectInfo> effectDict = new Dictionary<uint, EffectInfo>();//属性页描述
        bool ifFirstLoad = true;
        public class SlotInfo
        {
            public bool ifCanEquip
            {
                set;
                get;
            }
            public UISprite sprite
            {
                set;
                get;
            }
            public uint runeid
            {
                set;
                get;
            }
            public GameObject lockGO
            {
                set;
                get;
            }

            public GameObject resGO1
            {
                set;
                get;
            }
            public GameObject resGO2
            {
                set;
                get;
            }
        }
    }
}
