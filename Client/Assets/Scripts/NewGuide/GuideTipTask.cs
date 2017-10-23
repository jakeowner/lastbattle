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
    public class GuideTipTask : GuideTaskBase
    {
        public GuideTipTask(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        {

        }

        public override void EnterTask()
        {
            GuideTitleInfo mInfo;
            if (!ConfigReader.GuideTitleInfoXmlInfoDict.TryGetValue(this.mTaskId, out mInfo))
            {
                this.FinishTask();
            }
            ResourceUnit objTipUnit = ResourcesManager.Instance.loadImmediate(mInfo.mSite, ResourceType.PREFAB);
            mObjTip = GameObject.Instantiate(objTipUnit.Asset) as GameObject;
            mObjTip.transform.parent = GameMethod.GetUiCamera.transform;

            mObjTip.transform.localPosition = mInfo.LabelPos;
            mObjTip.transform.localScale = Vector3.one;
            UILabel lb = mObjTip.transform.Find("Label").GetComponent<UILabel>();
            lb.text = mInfo.mContent;
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
        }

        GameObject mObjTip = null;
    }
}
