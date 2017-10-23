using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameDefine;
using BlGame.Ctrl;
using BlGame.Model;

namespace BlGame.View
{
    public class UIGuideWindow : BaseWindow
    {
        ///////新手指引UI引导界面
        ///////新手指引UI引导界面
        ///////新手指引UI引导界面
        public UIGuideWindow()
        {
            //mScenesType = EScenesType.EST_Login;
            //mResName = GameConstDefine.UIGuideRestPath;
            //mResident = false;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            //EventCenter.AddListener(EGameEvent.eGameEvent_UIGuideEnter, Show);
            //EventCenter.AddListener(EGameEvent.eGameEvent_UIGuideExit, Hide);
            
        }

        //类对象释放
        public override void Realse()
        {
            //EventCenter.RemoveListener(EGameEvent.eGameEvent_UIGuideEnter, Show);
            //EventCenter.RemoveListener(EGameEvent.eGameEvent_UIGuideExit, Hide);
            
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            GameObject bk = mRoot.Find("Black").gameObject;
            GameObject chk = mRoot.Find("UIGuideBtn").gameObject;
            mTypeGameObject.Add(UIGuideType.BackGroundBtn, bk);
            mTypeGameObject.Add(UIGuideType.TypeCheckBox, chk);

            InitGuideTask();
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
            mTypeGameObject.Clear();
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

        }

        //隐藏
        public override void OnDisable()
        {

        }

        private void OnjoinPrimaryGuide()
        {

        }

        /// <summary>
        /// 界面打开需要表现引导
        /// </summary>
        /// <param name="gobj"></param>
        private void OnUiGuideAddButtonEvent(GameObject gobj)
        {
            if (!ConfigReader.GuideTaskXmlInfoDict.ContainsKey(mCurrentTaskId))
            {
                return;
            }
            if (ConfigReader.GuideTaskXmlInfoDict[mCurrentTaskId].BtnName == gobj.name)
            {
                DeltGuideInfos(mCurrentTaskId);
            }
        }

        /// <summary>
        /// 新手引导事件触发
        /// </summary>
        /// <param name="gobj"></param>
        private void OnUiGuideBtnFinishEvent(int taskId, bool presses)
        {
            //if (GuideEventButton == null)
            //{
            //    return;
            //}
            //Debug.Log(taskId);
            //GuideEventButton.transform.parent = LocalParent.transform;
            //GuideEventButton.SetActive(false);
            //GuideEventButton.SetActive(true);
            //TaskIdList.Remove(taskId);
            //if (TaskIdList.Count <= 0)
            //{
            //    return;
            //}
            //mCurrentTaskId = TaskIdList[0];
            //DeltGuideInfos(mCurrentTaskId);
        }

        /// <summary>
        /// 初始化引导信息
        /// </summary>
        private void InitGuideTask()
        {
            //List<int> guideModel = UIGuideModel.Instance.GetTaskModelList();
            //if (guideModel.Count == 0)
            //{
            //    return;
            //}
            //int mdId = guideModel[0];
            //TaskIdList = UIGuideModel.Instance.GetTaskIdList(UIGuideModel.Instance.mCurrentTaskModelId);
            mCurrentTaskId = TaskIdList[0];

            DeltGuideInfos(mCurrentTaskId);
        }

        ////////////////////进入新手引导   界面引导/////////////////
        private void InitjoinPrimaryGuide(GameObject oneBtn)
        { 
            UIButton btn = oneBtn.transform.Find("Button").GetComponent<UIButton>();
            EventDelegate.Add(btn.onClick, OnOneBtnClick);
        }

        /////////////////////
        private void OnOneBtnClick()
        {
            UIGuideCtrl.Instance.UIGuideAskEnterPrimaryGuide();
            Debug.Log("OnOneBtnClick");
        }
        ///////////////////////--------------------------------End---------------------------------------

        
        /// <summary>
        /// 显示当前要出现的UI引导
        /// </summary>
        /// <param name="taskId"></param>
        private void DeltGuideInfos(int taskId)
        {
            UIGuideType type = (UIGuideType)ConfigReader.GuideTaskXmlInfoDict[taskId].GuideType;
            foreach (var item in mTypeGameObject)
            {
                if (item.Key == type)
                {
                    item.Value.SetActive(true);
                    continue;
                }
                item.Value.SetActive(false);
            }
            switch (type)
            {
                case UIGuideType.BackGroundBtn: InitGuideGroundBtn(taskId); break;
            }
        }

        /// <summary>
        /// 为按钮添加事件
        /// </summary>
        private void AddUiGuideBtnEvnet(GameObject gobj , int taskId)
        {

        }

        /// <summary>
        /// 初始化有强制的UI引导
        /// </summary>
        private void InitGuideGroundBtn(int taskId)
        {
            GuideTaskInfo info = ConfigReader.GuideTaskXmlInfoDict[taskId];
            string name = info.BtnName;
            GuideEventButton = UIGuideModel.Instance.GetUiGuideButtonGameObject(name);
            if (GuideEventButton == null)
            {
                return;
            }
            LocalParent = GuideEventButton.transform.parent.gameObject;
            GuideEventButton.transform.parent = mRoot.transform;
            GuideEventButton.SetActive(false);
            GuideEventButton.SetActive(true);

            GameObject obj = LoadUiResource.LoadRes(mRoot, "Guide/" + info.PrefabName);
            obj.transform.Find("Label").GetComponent<UILabel>().text = info.Text;
            obj.transform.localPosition = info.PosXYZ;
            ButtonOnPress ck = GuideEventButton.AddComponent<ButtonOnPress>();
            ck.AddListener(taskId, OnUiGuideBtnFinishEvent, ButtonOnPress.EventType.ClickType);
        }

        private Dictionary<UIGuideType, GameObject> mTypeGameObject = new Dictionary<UIGuideType, GameObject>();

        private GameObject GuideEventButton;
        private GameObject LocalParent;
        private int mCurrentTaskId;
        private List<int> TaskIdList = new List<int>();
        public enum UIGuideType
        { 
            BackGroundBtn = 1,
            TypeCheckBox,
            TypeBubble,
        }
        //private int CurrentTaskId;
    }
}