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
    public class BattleWindow : BaseWindow
    {
        public BattleWindow()
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameBattleUI;
            mResident = true;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_BattleEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_BattleExit, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_BattleEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_BattleExit, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LobbyExit, Hide);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            //创建房间
            mCustomToggle = mRoot.Find("ModeSelect/Custom").GetComponent<UIToggle>();
            mCustomMapGrid = mRoot.Find("CustomInterface/CreateBattles/Mapselect/Grid");
            mRoomPassword = mRoot.Find("CustomInterface/CreateBattles/Password").GetComponent<UIInput>();
            mCreateRoom = mRoot.Find("CustomInterface/CreateBattles/CreateButton").gameObject;

            GameObject tranning = mRoot.Find("ModeSelect/Training").gameObject;
            mTranningToggle = tranning.GetComponent<UIToggle>();
            UIGuideCtrl.Instance.AddUiGuideEventBtn(tranning);

            EventDelegate.Add(mCustomToggle.onChange, OnCustom);
            EventDelegate.Add(mTranningToggle.onChange, OnTranning);
            UIEventListener.Get(mCreateRoom).onClick += OnCreateRoom;
            
            //加入房间
            mAddRoomTG = mRoot.Find("CustomInterface/JoinBtn").GetComponent<UIToggle>();
            mRoomListGrid = mRoot.Find("CustomInterface/JoinBattles/RoomList/Grid");
            mJoinBt = mRoot.Find("CustomInterface/JoinBattles/Button/Join").GetComponent<UIButton>();
            mRefreshBt = mRoot.Find("CustomInterface/JoinBattles/Button/Refresh").GetComponent<UIButton>();
            mSearchBt = mRoot.Find("CustomInterface/JoinBattles/Button/Search").GetComponent<UIButton>();

            mPasswordPanel = mRoot.Find("CustomInterface/JoinBattles/PasswordInput");
            mPasswordInput = mRoot.Find("CustomInterface/JoinBattles/PasswordInput/Password").GetComponent<UIInput>();
            mAddJoin = mRoot.Find("CustomInterface/JoinBattles/PasswordInput/Join").GetComponent<UIButton>();
            mAddBack = mRoot.Find("CustomInterface/JoinBattles/PasswordInput/Back").GetComponent<UIButton>();
            

            EventDelegate.Add(mAddRoomTG.onChange, OnShowAddRoomPanel);
            EventDelegate.Add(mJoinBt.onClick, OnJoin);
            EventDelegate.Add(mRefreshBt.onClick, OnRefresh);
            EventDelegate.Add(mSearchBt.onClick, OnSearch);

            EventDelegate.Add(mAddJoin.onClick, OnAddJoinRoom);
            EventDelegate.Add(mAddBack.onClick, OnAddBack);

            //查找房间
            mSearchInterface = mRoot.Find("CustomInterface/JoinBattles/SearchInterface");
            mSearchRoomIDInput = mRoot.Find("CustomInterface/JoinBattles/SearchInterface/RoomID").GetComponent<UIInput>();
            mSearchPassWordInput = mRoot.Find("CustomInterface/JoinBattles/SearchInterface/Password").GetComponent<UIInput>();
            mSearchAddBt = mRoot.Find("CustomInterface/JoinBattles/SearchInterface/Join").GetComponent<UIButton>();
            mSearchBackBt = mRoot.Find("CustomInterface/JoinBattles/SearchInterface/Back").GetComponent<UIButton>(); ;

            EventDelegate.Add(mSearchAddBt.onClick, OnSearchJoinRoom);
            EventDelegate.Add(mSearchBackBt.onClick, OnSearchBack);

            //匹配
            mMatchToggle = mRoot.Find("ModeSelect/Match").GetComponent<UIToggle>();
            mMatChNormal = mRoot.Find("MatchInterface/BattleMode/Normal").GetComponent<UIToggle>();
            mMatChAi = mRoot.Find("MatchInterface/BattleMode/AI").GetComponent<UIToggle>();
            mMatchRank = mRoot.Find("MatchInterface/BattleMode/RankUp").GetComponent<UIToggle>();
            mMatchMapGrid = mRoot.Find("MatchInterface/MapSelect/Grid");

            //新手引导
            mMap1 = mRoot.Find("TrainingInterface/MapSelect/Grid/1001").gameObject;
            mMap2 = mRoot.Find("TrainingInterface/MapSelect/Grid/1002").gameObject;
            UIGuideCtrl.Instance.AddUiGuideEventBtn(mMap1);
            //UIGuideCtrl.Instance.AddUiGuideEventBtn(mMap2);

            EventDelegate.Add(mMatChNormal.onChange, OnMatchModelChange);
            EventDelegate.Add(mMatChAi.onChange, OnMatchModelChange);
            EventDelegate.Add(mMatchRank.onChange, OnMatchModelChange);

            mRoomID = 0;
            mMatchType = EBattleMatchType.EBMT_Normal;
            mJoinBt.isEnabled = false;
        }

        //窗口控件释放
        protected override void RealseWidget()
        {

        }

     
        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_BattleUpdateRoomList, UpdateRoomList);
            EventCenter.AddListener<EErrorCode>(EGameEvent.eGameEvent_AskFriendEorr, EventError);
            EventCenter.AddListener<EErrorCode>(EGameEvent.eGameEvent_AskAddInBattle, EventError);
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_BattleUpdateRoomList, UpdateRoomList);
            EventCenter.RemoveListener<EErrorCode>(EGameEvent.eGameEvent_AskFriendEorr, EventError);
            EventCenter.AddListener<EErrorCode>(EGameEvent.eGameEvent_AskAddInBattle, EventError);
            
        }

        //显示
        public override void OnEnable()
        {
        }

        //隐藏
        public override void OnDisable()
        {
            //因为常驻，关闭时重置选项
            mMapId = 0;
            OnCustomMapRefresh();
            mSearchInterface.gameObject.SetActive(false);
            mPasswordPanel.gameObject.SetActive(false);
        }

        private void EventError(EErrorCode arg1)
        {
            MsgInfoManager.Instance.ShowMsg((int)arg1);
        }

        public void OnTranning()
        {
            if (mTranningToggle.value)
            {
                OnTranningMapRefresh();
            }
        }  

        public void OnCustom()
        {
            if(mCustomToggle.value)
            {
                OnCustomMapRefresh();
            }
        }   

        public void OnTranningMapRefresh()
        {
            UIEventListener.Get(mMap1).onClick += OnGuideSelectMap;
            UIEventListener.Get(mMap2).onClick += OnGuideSelectMap;
        }

        //刷新自定义地图列表
        public void OnCustomMapRefresh()
        {
            if (mCustomToggle.value)
            {
                LoadUiResource.ClearAllChild(mCustomMapGrid);

                List<MapInfo> mapList = MapLoadConfig.Instance.GetMapList(EBattleMatchType.EBMT_None);

                for (int i = 0; i < mapList.Count; i++)
                {
                    if (mapList[i].mIsTrain == false)
                    {
                        AddCustomMapItem(mapList[i]);
                    }
                }

                mMapId = 0;
                mRoomPassword.value = "";
            }

            mCustomMapGrid.GetComponent<UIGrid>().Reposition();
        }

        //增加自定义地图选项
        private void AddCustomMapItem(MapInfo info)
        {
            GameObject obj = LoadUiResource.AddChildObject(mCustomMapGrid, GameConstDefine.LoadGameLobbyCustomMapItem);
            if (obj != null)
            {
                obj.name = info.mId.ToString();
                obj.transform.Find("tSprite").GetComponent<UISprite>().spriteName = info.mShowPic;
                UIEventListener.Get(obj).onClick += OnCustomSelectMap;
            }
        }

        //自定义选择地图
        private void OnCustomSelectMap(GameObject obj)
        {
            mMapId = Convert.ToInt32(obj.name);
        }

        //新建房间
        public void OnCreateRoom(GameObject go)
        {
            if (mMapId <= 0)
            {
                MsgInfoManager.Instance.ShowMsg(40051);
                return;
            }
            BattleCtrl.Instance.CreateRoom(mMapId, mRoomPassword.value);
        }

        //显示房间列表
        public void OnShowAddRoomPanel()
        {
            mRoomPassword.value = "";
            OnRefresh();
        }

        //打开加入面板
        public void OnJoin()
        {
            if (mCurRoomItem == null || mRoomID == 0)
            {
                return;
            }

            RoomItem room = BattleCtrl.Instance.GetRoomInfo(mRoomID);
            if (room!=null && !room.mIsPassWord)
            {
                BattleCtrl.Instance.AskAddRoom(mRoomID.ToString(), "0");
            }
            else
            {
                mPasswordPanel.gameObject.SetActive(true);
                mPasswordInput.value = "";
            }
         }

        //刷新房间列表
        public void OnRefresh()
        {
             BattleCtrl.Instance.AskRoomList();
        }

        //加入房间
        public void OnAddJoinRoom()
        {
            BattleCtrl.Instance.AskAddRoom(mRoomID.ToString(), mPasswordInput.text);
        }

        //加入房间返回
        public void OnAddBack()
        {
            mPasswordPanel.gameObject.SetActive(false);
        }

        //更新房间列表
        private void UpdateRoomList()
        {
            //清空原来的列表
            LoadUiResource.ClearAllChild(mRoomListGrid);

            mCurRoomItem = null;
            mRoomID = 0;
            mJoinBt.isEnabled = false;

            //加入新的列表
            Dictionary<UInt64, RoomItem> RoomList = BattleCtrl.Instance.GetRoomList();
            foreach (RoomItem roomInfo in RoomList.Values)
            {
                AddRoomItem(roomInfo);
            }

            //默认选择第一个
            if (mRoomListGrid.childCount > 0)
            {
                mRoomListGrid.GetChild(0).GetComponent<UIToggle>().value = true;
            }

            mRoomListGrid.GetComponent<UIGrid>().Reposition();
        }

        //加入一个房间项
        private void AddRoomItem(RoomItem roomInfo)
        {
            GameObject gameObj = LoadUiResource.AddChildObject(mRoomListGrid, GameConstDefine.LoadGameLobbyRoomItemUI);

            if (gameObj == null)
                return;

            Transform obj = gameObj.transform;

            if (obj != null)
            {
                mRoomIDLabel = obj.Find("RoomID").GetComponent<UILabel>();
                mRoomIDLabel.text = roomInfo.mRoomId.ToString();

                mMapName = obj.Find("MapName").GetComponent<UILabel>();
                MapInfo mapInfo = MapLoadConfig.Instance.GetMapInfo(roomInfo.mMapId);
                if (mapInfo == null)
                {
                    Debug.LogError("mapLoadConfig Not find");
                    return;
                }
                mMapName.text = mapInfo.mName;

                mCreator = obj.Find("Creator").GetComponent<UILabel>();
                mCreator.text = roomInfo.mOwer;

                mStatus = obj.Find("Status").GetComponent<UILabel>();
                mStatus.text = roomInfo.mCurNum.ToString() + "/" + roomInfo.mMaxNum.ToString();
                
                mLock =   obj.Find("Lock").gameObject;
                mLock.SetActive(roomInfo.mIsPassWord);

                mHighlight = obj.Find("Highlight").gameObject;
                mHighlight.SetActive(false);
            }

            UIToggle item = obj.GetComponent<UIToggle>();
            EventDelegate.Add(item.onChange, OnRoomItemToggle);
        }

        //选择房间
        public void OnRoomItemToggle()
        {
            if (UIToggle.current != null && UIToggle.current.value)
            {
                if (mCurRoomItem != null)
                {
                    mHighlight = mCurRoomItem.Find("Highlight").gameObject;
                    mHighlight.SetActive(false);
                }

                mCurRoomItem = UIToggle.current.transform;

                mHighlight = mCurRoomItem.Find("Highlight").gameObject;
                mHighlight.SetActive(true);

                mRoomIDLabel = mCurRoomItem.Find("RoomID").GetComponent<UILabel>();
                mRoomID = UInt32.Parse(mRoomIDLabel.text);

                mJoinBt.isEnabled = true;
            }
        }

        //打开查找界面
        public void OnSearch()
        {
            mSearchInterface.gameObject.SetActive(true);
            mSearchRoomIDInput.value = "";
            mSearchPassWordInput.value = "";
        }

        //加入房间（通过查找)
        public void OnSearchJoinRoom()
        {
            BattleCtrl.Instance.AskAddRoom(mSearchRoomIDInput.text,mSearchPassWordInput.text);
        }

        //查找返回
        public void OnSearchBack()
        {
             mSearchInterface.gameObject.SetActive(false);
        }

        //匹配模式切换
        public void OnMatchModelChange()
        {
            if (UIToggle.current != null && UIToggle.current.isChecked)
            {
                LoadUiResource.ClearAllChild(mMatchMapGrid);

                List<MapInfo> mapList = new List<MapInfo>();
                if (UIToggle.current == mMatChNormal)
                {
                    mapList = MapLoadConfig.Instance.GetMapList(EBattleMatchType.EBMT_Normal);
                    mMatchType = EBattleMatchType.EBMT_Normal;
                }
                else if (UIToggle.current == mMatChAi)
                {
                    mapList = MapLoadConfig.Instance.GetMapList(EBattleMatchType.EBMT_Ai);
                    mMatchType = EBattleMatchType.EBMT_Ai;
                }
                else if (UIToggle.current == mMatchRank)
                {
                    mapList = MapLoadConfig.Instance.GetMapList(EBattleMatchType.EBMT_Rank);
                    mMatchType = EBattleMatchType.EBMT_Rank;
                }

                for (int i = 0; i < mapList.Count; i++)
                {
                    AddMatchMapItem(mapList[i]);
                }
            }

            mMatchMapGrid.GetComponent<UIGrid>().Reposition();
        }

        //增加匹配地图选项
        private void AddMatchMapItem(MapInfo info)
        {
            GameObject obj = LoadUiResource.AddChildObject(mMatchMapGrid, GameConstDefine.LoadGameLobbyMatchMapItem);
            if (obj != null)
            {
                obj.name = info.mId.ToString(); 
                obj.transform.Find("tSprite").GetComponent<UISprite>().spriteName = info.mShowPic;
                UIEventListener.Get(obj).onClick += OnMatchSelectMap;
             }
        }

        //新手训练选择地图
        private void OnGuideSelectMap(GameObject obj)
        {
            if (obj == mMap1)
            {
                BattleCtrl.Instance.AskCreateGuideBattle(1000, GCToCS.AskCSCreateGuideBattle.guidetype.first);
            }
            else
            {
                JxBlGame.Instance.IsQuickBattle = true;
                BattleCtrl.Instance.AskCreateGuideBattle(1001, GCToCS.AskCSCreateGuideBattle.guidetype.second);
            }
            
        }

        //匹配选择地图
        private void OnMatchSelectMap(GameObject obj)
        {
            BattleCtrl.Instance.AskMatchBattle(Convert.ToInt32(obj.name), mMatchType);
        }
        
        //创建房间
        Transform mCustomMapGrid;    //地图列表
        UIToggle mMatchToggle;
        UIToggle mCustomToggle;
        UIToggle mTranningToggle;
        UIInput mRoomPassword;
        GameObject mCreateRoom;
        Int32 mMapId;

        //加入房间
        UIToggle mAddRoomTG;
        Transform mRoomListGrid;
        Transform mCurRoomItem;

        UILabel mRoomIDLabel;
        UILabel mMapName;
        UILabel mCreator;
        UILabel mStatus;
        GameObject mLock;
        GameObject mHighlight;

        UIButton mJoinBt;
        UIButton mRefreshBt;
        UIButton mSearchBt;

        Transform mPasswordPanel;
        UIInput mPasswordInput;
        UIButton mAddJoin;
        UIButton mAddBack;

        //查找
        Transform mSearchInterface;
        UIInput mSearchRoomIDInput;
        UIInput mSearchPassWordInput;
        UIButton mSearchAddBt;
        UIButton mSearchBackBt;

        //匹配
        UIToggle mMatChNormal;   //普通
        UIToggle mMatChAi;       //人机
        UIToggle mMatchRank;     //天梯
        Transform mMatchMapGrid;    //地图列表

        //新手引导
        GameObject mMap1;
        GameObject mMap2;

        //数据
        UInt32 mRoomID;    //房间ID
        EBattleMatchType mMatchType;  // 匹配模式

}

}
