using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using BlGame.Model;
using BlGame.Resource;
using GameDefine;


namespace BlGame.GuideDate
{
    public class GuideForceClick : GuideTaskBase
    {
        private GameObject GuideEventButton;
        private GameObject LocalParent;
        private GameObject mObj;
        private ButtonOnPress Click;
        private GameObject mAnchorObj;
        private GuideTaskInfo mTaskInfo;
        private GameObject mGuideEffect;
        private GameObject mBlack;

        private int mTriggerCount;

        public GuideForceClick(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        {
        }

        public override void EnterTask()
        {
            EventCenter.AddListener<GameObject>(EGameEvent.eGameEvent_UIGuideEvent, OnUiGuideAddButtonEvent);
            DeltTask();
        }

        /// <summary>
        /// 处理任务表现
        /// </summary>
        private void DeltTask()
        {
            if (!ConfigReader.GuideTaskXmlInfoDict.TryGetValue(mTaskId, out mTaskInfo))
            {
                this.FinishTask();
                return;
            }
            string name = mTaskInfo.BtnName;
            GuideEventButton = UIGuideModel.Instance.GetUiGuideButtonGameObject(name);
            if (GuideEventButton == null)
            {
                return;
            }
            LoadGuideEffect();
            LoadGuideFrame();
            DeltTriggerInfo(ResolveEventButton());
        }

        /// <summary>
        /// 界面打开需要表现引导
        /// </summary>
        /// <param name="gobj"></param>
        private void OnUiGuideAddButtonEvent(GameObject gobj)
        {
            if (gobj.name == mTaskInfo.BtnName)
            {
                DeltTask();
            }
        }

        /// <summary>
        /// 处理完成触发信息
        /// </summary>
        /// <param name="anchor"></param>
        private void DeltTriggerInfo(bool anchor)
        {
            if (mTaskInfo.mBtnTriggerType == ButtonTriggerType.mTypeClick)
            {
                Click = GuideEventButton.AddComponent<ButtonOnPress>();
                if (anchor)
                {
                    Click.AddListener(mTaskId, OnPressDown, ButtonOnPress.EventType.PressType);
                }
                else
                {
                    Click.AddListener(mTaskId, OnClick, ButtonOnPress.EventType.ClickType);
                }
            }
            else
            {
                AddDragEventListener();
            }
        }

        /// <summary>
        /// 增加事件监听
        /// </summary>
        private void AddDragEventListener()
        {
            //EventCenter.AddListener<ButtonDragType>(EGameEvent.eGameEvent_PlayGuideDragEvent, OnGameObjectDragEvent);
        }

        /// <summary>
        /// 是否事件触发正确
        /// </summary>
        /// <param name="mType"></param>
        /// <param name="gObj"></param>
        /// <returns></returns>
        public bool IsGuideTrigger(ButtonTriggerType mType, GameObject gObj)
        {
            if (GuideEventButton == null)
            {
                return false;
            }
            if (mTaskInfo.mBtnTriggerType == mType && GuideEventButton == gObj)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 滑动的事件
        /// </summary>
        /// <param name="mType"></param>
        private void OnGameObjectDragEvent(ButtonDragType mType)
        {
            if (mType != this.mTaskInfo.mDragType)
            {
                return;
            }
            mTriggerCount++;
            if (mTriggerCount >= this.mTaskInfo.mTaskTimes)
            {
                this.FinishTask();

            }
        }

        /// <summary>
        /// Load Guide的方框
        /// </summary>
        private void LoadGuideFrame()
        {
            ResourceUnit Unit = ResourcesManager.Instance.loadImmediate("Guide/" + mTaskInfo.PrefabName, ResourceType.PREFAB);
            mObj = GameObject.Instantiate(Unit.Asset) as GameObject;
            mObj.transform.parent = mRoot.transform;
			mObj.transform.localPosition = Vector3.zero;
            mObj.transform.Find("Anchor/Label").GetComponent<UILabel>().text = mTaskInfo.Text;
            mObj.transform.Find("Anchor").localPosition = mTaskInfo.PosXYZ;
            mObj.transform.localScale = Vector3.one;
        }

        private void LoadGuideEffect()
        {
            if (mTaskInfo.GuideEffect == null || mTaskInfo.GuideEffect == "")
            {
                return;
            }
            ResourceUnit Unit = ResourcesManager.Instance.loadImmediate(mTaskInfo.GuideEffect, ResourceType.PREFAB);
            mGuideEffect = GameObject.Instantiate(Unit.Asset) as GameObject;
            mGuideEffect.transform.parent = GuideEventButton.transform;
            mGuideEffect.transform.localPosition = Vector3.zero;
            mGuideEffect.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 处理需要强制点击的按钮的显示
        /// </summary>
        /// <returns></returns>
        private bool ResolveEventButton()
        {
            mBlack = mRoot.transform.Find("Black").gameObject;
            bool mAnc = false;
            LocalParent = GuideEventButton.transform.parent.gameObject;
            UIAnchor mAnchor = LocalParent.GetComponent<UIAnchor>();
            if (mAnchor)
            {
                mAnc = true;
                mAnchorObj = new GameObject("mAnchor");
                mAnchorObj.transform.parent = mRoot.transform;
                mAnchorObj.transform.position = LocalParent.transform.position;
                mAnchorObj.transform.localScale = Vector3.one;
                UIAnchor mAn = mAnchorObj.AddComponent<UIAnchor>();
                mAn.side = UIAnchor.Side.BottomLeft;
                mAn.uiCamera = GameMethod.GetUiCamera;
                GuideEventButton.transform.parent = mAnchorObj.transform;
            }
            else
            {
                GuideEventButton.transform.parent = mBlack.transform;
                if (mTaskInfo.mPos != null && mTaskInfo.mPos != Vector3.zero)
                {
                    GuideEventButton.transform.localScale = Vector3.one;
                    GuideEventButton.transform.localPosition = mTaskInfo.mPos;
                }
            }
            mBlack.SetActive(true);
            GuideEventButton.SetActive(false);
            GuideEventButton.SetActive(true);
            return mAnc;
        }

        /// <summary>
        /// 按钮事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDown"></param>
        private void OnClick(int id, bool isDown)
        {
            if (isDown)
            {
                return;
            }
            mTriggerCount++;
            if (mTriggerCount >= this.mTaskInfo.mTaskTimes)
            {
                this.FinishTask();

            }
        }

        /// <summary>
        /// 按下的事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDown"></param>
        private void OnPressDown(int id, bool isDown)
        {
            if (!isDown)
            {
                return;
            }
            mTriggerCount++;
            if (mTriggerCount >= this.mTaskInfo.mTaskTimes)
            {
                this.FinishTask();
            }
        }

        public override void FinishTask()
        {
            if (LocalParent == null)
            {
                GameObject.Destroy(GuideEventButton);
                base.FinishTask();
                return;
            }
            GuideEventButton.transform.parent = LocalParent.transform;
            LocalParent = null;
            GuideEventButton.SetActive(false);
            GuideEventButton.SetActive(true);
            base.FinishTask();
        }

        public override void ExcuseTask()
        {

        }

        public override void ClearTask()
        {
            if (LocalParent != null)
            {
                GuideEventButton.transform.parent = LocalParent.transform;
            }
            if (Click != null)
            {
                //UIGuideModel.Instance.UiGuideButtonGameObjectList.Remove(Click.gameObject);
                GameObject.Destroy(Click);
            }
            if (mObj != null)
            {
                GameObject.Destroy(mObj);
            }
            if (mAnchorObj != null)
            {
                GameObject.Destroy(mAnchorObj);
            }
            if (mGuideEffect != null)
            {
                GameObject.Destroy(mGuideEffect);
            }
            //if (mTaskInfo.mBtnTriggerType == ButtonTriggerType.mTypeDrag)
            //{
            //    EventCenter.RemoveListener<ButtonDragType>(EGameEvent.eGameEvent_PlayGuideDragEvent, OnGameObjectDragEvent);  
            //}

            mBlack.SetActive(false);
            EventCenter.RemoveListener<GameObject>(EGameEvent.eGameEvent_UIGuideEvent, OnUiGuideAddButtonEvent);
        }
    }
}
