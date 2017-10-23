using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using GameDefine;
using BlGame.GameEntity;
using BlGame.Resource;

namespace BlGame.GuideDate

{
    public class GuidePopTipTask : GuideTaskBase
    {
        private PopTipskInfo PopTask = null;
        private GameObject mObjTip = null;

        public GuidePopTipTask(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        { 
       
        }

        //private const int rewardTask = 1001;
        public override void EnterTask()
        {
            if (!ConfigReader.GuidePopTipTaskXmlDict.TryGetValue(this.mTaskId, out PopTask))
            {
                this.FinishTask();
                return;
            }
            ResourceUnit objTipUnit = ResourcesManager.Instance.loadImmediate(PopTask.mResPath, ResourceType.PREFAB);
            mObjTip = GameObject.Instantiate(objTipUnit.Asset) as GameObject;
            mObjTip.transform.parent = GameMethod.GetUiCamera.transform;
            mObjTip.transform.localPosition = PopTask.mSitePos;
            mObjTip.transform.localScale = Vector3.one;
            UILabel labelTip = mObjTip.transform.Find("Text").GetComponent<UILabel>();
            labelTip.text = PopTask.mTip;
            TweenScale.Begin(mObjTip, PopTask.mTime, PopTask.mRate).method = UITweener.Method.BounceIn;
        }
         
        public override void ExcuseTask()
        {
            
        }

        public override void ClearTask()
        {
            base.ClearTask();
            if (mObjTip != null)
            {
                GameObject.DestroyObject(mObjTip);
            }
            PopTask = null;
        }
       
    }


}

