using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using GameDefine;

namespace BlGame.GuideDate

{
    public class GuideTaskReConnectDeal : Singleton<GuideTaskReConnectDeal>
    {
        private List<int> taskList = new List<int>();
        private int reTaskId = 0;
        private const int startTask = 1001;
        private Dictionary<string, ShowObjInfo> showObjDic = new Dictionary<string, ShowObjInfo>();
    //    private Dictionary<int, bool> sendNpcDic = new Dictionary<int, bool>();

        private class ShowObjInfo
        {
            public int TaskId; 
            public bool show;
        }

        public void StartReConnectTask(int taskId) {
            reTaskId = taskId;
            taskList = CTaskBase.managerTaskDic[startTask].TaskList;
            showObjDic.Clear();
        } 

        public void DealReConnectTask() {
            for (int i = 0; i < taskList.IndexOf(reTaskId); i++)
            {
                CTaskManagerData data = CTaskBase.managerTaskDic.ElementAt(i).Value;
                for (int j = 0; j < data.TaskTypeSet.Count; j++) {
                    if (data.TaskTypeSet.ElementAt(j) == GuideTaskType.ObjShowTask)
                    {
                        AddShowObjTaskList(j, data);
                    }
                    //else if(data.TaskTypeSet.ElementAt(j) == GuideTaskType.SenderSoldier){
                    //    AddSendNpcTaskList(j, data);
                    //}
                }
            }
            ShowObj();
         //   SendNpc();
        }

        private void AddShowObjTaskList(int index,CTaskManagerData taskData) {
            int showTaskId = taskData.SonTaskIdSet.ElementAt(index);
            CGameObjectShowTask showTask = ConfigReader.GetObjShowTaskInfo(showTaskId);
            bool show = showTask.EndShow == 1? true:false;
            if (showObjDic.ContainsKey(showTask.Path))
            {
                ShowObjInfo info =  showObjDic[showTask.Path];
                info.show = show;
                showObjDic[showTask.Path] = info;
            }
            else { 
                ShowObjInfo info = new ShowObjInfo();
                info.show = show;
                info.TaskId = showTaskId;
                showObjDic.Add(showTask.Path, info);
            }
        }

        private void ShowObj() {
            for (int i = 0; i < showObjDic.Count; i++) {
                int taskId = showObjDic.ElementAt(i).Value.TaskId;
                CGameObjectShowTask showTask = ConfigReader.GetObjShowTaskInfo(taskId);
                if (showTask == null)
                {
                    Debug.LogError("GuideShowObjTask 找不到任務 Id" + taskId);
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
//                         if (UIPlay.Instance != null)
//                         {
//                             btnParent = UIPlay.Instance.transform;
//                         }
                        break;
                }
                if (btnParent == null)
                {
                    Debug.LogError("GuideShowObjTask = " + taskId + "挂点不存在");
                }
                GameObject objShow = btnParent.Find(showTask.Path).gameObject;
                if (objShow == null)
                {
                    Debug.LogError("GuideShowObjTask 找不到物體 Id" + taskId);
                }
                objShow.SetActive(showObjDic.ElementAt(i).Value.show);
              //  Debug.LogError("objShow = " + objShow.name + "value = " + showObjDic.ElementAt(i).Value.show);
            } 
           
        }



        //private void AddSendNpcTaskList(int index,CTaskManagerData taskData){
        //    int sendTaskId = taskData.SonTaskIdSet.ElementAt(index);
        //    CSendNpcTask npcTask = ConfigReader.GetBornNpcTaskInfo(sendTaskId);
        //    bool send = npcTask.Tag == 1 ? true : false;
        //    if (sendNpcDic.ContainsKey(npcTask.MilitaryId))
        //    {
        //        sendNpcDic[npcTask.MilitaryId] = send;
        //    }
        //    else
        //    {
        //        sendNpcDic.Add(npcTask.MilitaryId, send);
        //    }
        //}

        //private void SendNpc() {
        //    for (int i = 0; i < sendNpcDic.Count; i++) {
        //        if (sendNpcDic.ElementAt(i).Value) {
        //            CGLCtrl_GameLogic.Instance.EmsgToss_GuideAskBornNpc(sendNpcDic.ElementAt(i).Key,1);
        //        }
        //    }
        //}
    }

}
