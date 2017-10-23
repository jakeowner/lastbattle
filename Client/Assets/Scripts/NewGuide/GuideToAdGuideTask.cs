using System;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using BlGame.GameEntity;
using JT.FWW.Tools;
using BlGame.Resource;

namespace BlGame.GuideDate
{
    ////////////////////////////////////////////////////////////引导进入次级引导/////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////引导进入次级引导/////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////引导进入次级引导/////////////////////////////////////////////////////////

    public class GuideToAdGuideTask : GuideTaskBase
    {
        private GameObject mToAdGuide;
        private UIButton mGoButton;
        private UIButton mCancelButton;

        public GuideToAdGuideTask(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        {
  
        }

        /// <summary>
        /// 到时广播消息
        /// </summary>
        public override void EnterTask()
        {
            ResourceUnit Unit = ResourcesManager.Instance.loadImmediate("Guide/UIGuideTwoBtn" , ResourceType.PREFAB);
            mToAdGuide = GameObject.Instantiate(Unit.Asset) as GameObject;
            mToAdGuide.transform.parent = mRoot.transform;
            mToAdGuide.transform.localScale = Vector3.one;
            mToAdGuide.transform.localPosition = Vector3.zero;
            mGoButton = mToAdGuide.transform.Find("GoBtn").GetComponent<UIButton>();
            mCancelButton = mToAdGuide.transform.Find("CancelBtn").GetComponent<UIButton>();
            EventDelegate.Add(mGoButton.onClick, OnGoButtonClick);
            EventDelegate.Add(mCancelButton.onClick, OnCancelButtonClick);
        }

        public override void ExcuseTask()
        {

        }

        /// <summary>
        /// 前往战场按钮点击
        /// </summary>
        private void OnGoButtonClick()
        {
            JxBlGame.Instance.IsQuickBattle = true;
            BlGame.Ctrl.BattleCtrl.Instance.AskCreateGuideBattle(1001, GCToCS.AskCSCreateGuideBattle.guidetype.second);
            this.FinishTask();
        }

        /// <summary>
        /// 取消按钮点击
        /// </summary>
        private void OnCancelButtonClick()
        {
            if (mToAdGuide != null)
            {
                GameObject.Destroy(mToAdGuide);
            }
            this.FinishTask();
        }

        public override void ClearTask()
        {
            if (mToAdGuide != null)
            {
                GameObject.Destroy(mToAdGuide);
            }
            if (mCancelButton != null)
            {
                GameObject.Destroy(mCancelButton);
            }
            if (mGoButton != null)
            {
                GameObject.Destroy(mGoButton);
            }
            base.ClearTask();
        }

    }


}
