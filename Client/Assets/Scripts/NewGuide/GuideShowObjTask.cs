using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using GameDefine;

namespace BlGame.GuideDate

{
    public class GuideShowObjTask : GuideTaskBase
    {
        private CGameObjectShowTask showTask = null;
        private GameObject objShow = null;

        public GuideShowObjTask(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        { 
            //读取数据
            showTask = ConfigReader.GetObjShowTaskInfo(task);
            if (showTask == null) 
            {
                Debug.LogError("GuideShowObjTask 找不到任務 Id" + task);
            }
            Transform btnParent = null;
            switch (showTask.PathType)
            {
                case UIPathType.UIGuideType:
                    if (UINewsGuide.Instance != null)
                    {
                        btnParent = UINewsGuide.Instance.transform;
                    }
                    break;
                case UIPathType.UIPlayType:
//                     if (UIPlay.Instance != null)
//                     {
//                         btnParent = UIPlay.Instance.transform;
//                     }
                    break;
            }
            if (btnParent == null)
            {
                Debug.LogError("GuideShowObjTask = " + task + "挂点不存在");
            }
            objShow = btnParent.Find(showTask.Path).gameObject;
            if (objShow == null) {
                Debug.LogError("GuideShowObjTask 找不到物體 Id" + task);
            }
        }        

        public override void EnterTask()
        {
            objShow.SetActive(showTask.StartShow == 1 ? true:false);
        }
 
        public override void ExcuseTask()
        { 

        }

        public override void ClearTask()
        {
            if (objShow != null)
            {
                objShow.SetActive(showTask.EndShow == 1 ? true : false);
            }
            base.ClearTask();
        }
       
    }


}
