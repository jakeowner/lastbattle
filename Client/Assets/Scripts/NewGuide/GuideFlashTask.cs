using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameDefine;
using BlGame.Resource;

namespace BlGame.GuideDate

{
    public class GuideFlashTask : GuideTaskBase
    { 
        private CFlashTask flashTask = null;
        private GameObject objFlash = null;
        private UITweener tween = null;
        bool orignalActive = false;
        Vector3 orignalPos = new Vector3();

        public GuideFlashTask(int task, GuideTaskType type, GameObject mParent)
            : base(task, type, mParent)
        { 
            //读取数据
            flashTask = ConfigReader.GetFlashTaskInfo(task);
            if (flashTask == null) {
                Debug.LogError("GuideFlashTask 任务id不存在" + task);
            }
            Transform btnParent = null;
            switch (flashTask.PathType)
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
                Debug.LogError("GuideFlashTask = " + task + "挂点不存在");
            }
            if (!string.IsNullOrEmpty(flashTask.UiPath)) {
                objFlash = btnParent.Find(flashTask.UiPath).gameObject;
               
            }
            else 
            {
                //objFlash = GameObject.Instantiate(Resources.Load(flashTask.PrefabPath)) as GameObject;                
                ResourceUnit objFlashUnit = ResourcesManager.Instance.loadImmediate(flashTask.PrefabPath, ResourceType.PREFAB);                               
                objFlash = GameObject.Instantiate(objFlashUnit.Asset) as GameObject;
            }
            if (string.IsNullOrEmpty(flashTask.UiPath))
            {
                objFlash.transform.parent = btnParent;
            }
            objFlash.transform.localPosition = flashTask.StartPos;
            objFlash.transform.localScale = Vector3.one;
            orignalActive = objFlash.activeInHierarchy;
            orignalPos = objFlash.transform.localPosition; 
            objFlash.gameObject.SetActive(true); 
        }        

        public override void EnterTask()
        {
            switch (flashTask.Type) { 
                case FlashType.FlashAlpha:
                    TweenAlpha.Begin(objFlash.gameObject, 0f, Convert.ToInt32(flashTask.StartEffect));
                    tween = TweenAlpha.Begin(objFlash, flashTask.During, Convert.ToInt32(flashTask.TargetEffect));
                    break;
                case FlashType.FlashMove:                    
                    tween = TweenPosition.Begin(objFlash, flashTask.During, GameMethod.ResolveToVector3(flashTask.TargetEffect,';'));
                    break;
                case FlashType.FlashScale:
                    tween = TweenScale.Begin(objFlash, flashTask.During, GameMethod.ResolveToVector3(flashTask.TargetEffect, ';'));
                    break;                 
            }
            tween.method = flashTask.EffectType;
            tween.style = flashTask.Style;
            if (tween.style == UITweener.Style.Once) {
                EventDelegate.Add(tween.onFinished, FlashEnd,true);  
            }
        }
         
        public override void ExcuseTask()
        {
            
        }

        public override void ClearTask()
        {            
            base.ClearTask();
            if (!string.IsNullOrEmpty(flashTask.UiPath))
            {
                if (flashTask.OverReset == 1) {
                    objFlash.SetActive(orignalActive);
                    objFlash.transform.localPosition = orignalPos;
                }
            }
            else {
                if (objFlash != null)
                {
                    GameObject.DestroyObject(objFlash);
                }                
            }
            tween = null;
            flashTask = null;
            objFlash = null;
        }

        private void FlashEnd() {
            if (tween != null) {
                EventDelegate.Remove(tween.onFinished, FlashEnd);
            }
            base.FinishTask();
        }
       
    }


}
