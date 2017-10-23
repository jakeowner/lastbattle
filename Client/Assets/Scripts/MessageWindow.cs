using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameDefine;
using BlGame.Ctrl;

namespace BlGame.View
{
    public enum EMessageType
    {
        EMT_None = -1,
        EMT_Reconnect,      //断网重连提示
        EMT_ReEnter,        //断线重登录提示
        EMT_Disconnect,     //连接异常退出
        EMT_KickOut,        //被踢下线
        EMT_BuyGoodsSuccess,
        EMT_PlaseWaitForGameFuture,
        EMT_SureToWash,
    }

    public class MessageWindow : BaseWindow
    {
        public MessageWindow()
        {
            mScenesType = EScenesType.EST_None;
            mResName = GameConstDefine.GameConnectMsg;
            mResident = false;
        }

        public Callback<bool> m_Callback = null;
        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener<EMessageType>(EGameEvent.eGameEvent_ShowMessage, ShowMessage);
            EventCenter.AddListener<EMessageType, string, Callback<bool>>(EGameEvent.eGameEvent_ShowLogicMessage, ShowMessage);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener<EMessageType>(EGameEvent.eGameEvent_ShowMessage, ShowMessage);
            EventCenter.RemoveListener<EMessageType, string, Callback<bool>>(EGameEvent.eGameEvent_ShowLogicMessage, ShowMessage);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            mCaption = mRoot.Find("Status/Tip1").GetComponent<UILabel>();
            mMessage = mRoot.Find("Status/Tip2").GetComponent<UILabel>();
            mSubmitBt = mRoot.Find("Status/Button").GetComponent<UIButton>();
            mTimeLabel = mRoot.Find("Status/Button/Time").GetComponent<UILabel>();

            m_CloseBtn = mRoot.Find("CloseBtn").GetComponent<UIButton>();
            EventDelegate.Add(mSubmitBt.onClick, OnButtonPressed);
            EventDelegate.Add(m_CloseBtn.onClick, onClickCloseBtn);
        }

        public void onClickCloseBtn()
        {
            if (m_Callback != null)
            {
                m_Callback(false);
            }
            Hide();
        }
        //删除Login外其他控件，例如
        public static void DestroyOtherUI()
        {

        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_ConnectServerSuccess, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_ReConnectSuccess, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_ReConnectFail, Hide);
            EventCenter.AddListener(EGameEvent.eGameEvent_BatttleFinished, BatttleFinished);
            
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ConnectServerSuccess, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ReConnectSuccess, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_ReConnectFail, Hide);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_BatttleFinished, BatttleFinished);
        }

        //显示
        public override void OnEnable()
        {
        }

        //隐藏
        public override void OnDisable()
        {
            mStatus = EMessageType.EMT_None;
        }

        public override void Update(float deltaTime)
        {
            if (mStatus == EMessageType.EMT_Reconnect)
            {
                int timeThrought = (int)(Time.time - mTimeCount);
                if (timeThrought > 0)
                {
                    int limit = mConnectCount - timeThrought;
                    mTimeLabel.text = "00:" + limit.ToString();
                }
                if (timeThrought >= mConnectCount)
                {
                    Hide();
                    ShowMessage(EMessageType.EMT_Disconnect);
                }
            }
        }
        public void ShowMessage(EMessageType st, string str, Callback<bool> callback)
        {
            if (mVisible)
                return;

            m_Callback = callback;
            mStatus = st;

            Show();
            switch (mStatus)
            {
                case EMessageType.EMT_SureToWash:
                    mMessage.text = str;
                    mSubmitBt.isEnabled = true;
                    mTimeLabel.text = "确认";
                    mCaption.gameObject.SetActive(false);
                    m_CloseBtn.gameObject.SetActive(true);
                    break;
            }
        }

        public void ShowMessage(EMessageType st)
        {
            if (mVisible)
                return;

            mStatus = st;
            mBatttleFinished = false;

            Show();
            m_CloseBtn.gameObject.SetActive(false);

            switch (mStatus)
            {
                case EMessageType.EMT_Reconnect:
                    mCaption.text = ConfigReader.GetMsgInfo(40041).content;
                    mMessage.text = ConfigReader.GetMsgInfo(40042).content;

                    mConnectCount = 30;
                    mTimeCount = Time.time;
                    mTimeLabel.text = "00:" + mConnectCount.ToString();
                    mSubmitBt.isEnabled = false;
                    break;
                case EMessageType.EMT_ReEnter:
                    mCaption.text = ConfigReader.GetMsgInfo(40041).content;
                    mMessage.text = ConfigReader.GetMsgInfo(40044).content;
                    mTimeLabel.text = ConfigReader.GetMsgInfo(40045).content;
                    mSubmitBt.isEnabled = true;
                    break;
                case EMessageType.EMT_Disconnect:
                    mCaption.text = ConfigReader.GetMsgInfo(40041).content;
                    mMessage.text = ConfigReader.GetMsgInfo(40043).content;
                    mTimeLabel.text = ConfigReader.GetMsgInfo(40046).content;
                    mSubmitBt.isEnabled = true;
                    break;
                case EMessageType.EMT_KickOut:
                    mCaption.text = ConfigReader.GetMsgInfo(40041).content;
                    mMessage.text = ConfigReader.GetMsgInfo(40047).content;
                    mTimeLabel.text = ConfigReader.GetMsgInfo(40046).content;
                    mSubmitBt.isEnabled = true;
                    break;
                case EMessageType.EMT_BuyGoodsSuccess:
                    mMessage.text = ConfigReader.GetMsgInfo(10031).content;
                    mSubmitBt.isEnabled = true;
                    mTimeLabel.text = "确认";
                    mCaption.gameObject.SetActive(false);
                    break;
                case EMessageType.EMT_PlaseWaitForGameFuture:
                    mMessage.text = "敬请期待";
                    mSubmitBt.isEnabled = true;
                    mTimeLabel.text = "确认";
                    mCaption.gameObject.SetActive(false);
                    break;
            }
        }

        private void OnButtonPressed()
        {
            switch (this.mStatus)
            {
                case EMessageType.EMT_Reconnect:
                    break;
                case EMessageType.EMT_ReEnter:
                    {
                        if (mBatttleFinished)
                        {
                            CGLCtrl_GameLogic.Instance.EmsgToss_AskReEnterRoom();
                        }
                        else
                        {
                            CGLCtrl_GameLogic.Instance.EmsgTocs_AskReConnetToBattle();
                        }
                    }
                    break;
                case EMessageType.EMT_Disconnect:
                case EMessageType.EMT_KickOut:
                    LoginCtrl.Instance.SdkLogOff();
                    Hide();
                    //Application.Quit();
                    break;
                case EMessageType.EMT_BuyGoodsSuccess:
                case EMessageType.EMT_PlaseWaitForGameFuture:
                    Hide();
                    break;
                case EMessageType.EMT_SureToWash:
                    {
                        m_Callback(true);
                        Hide();
                    }
                    break;
            }
        }

        public void BatttleFinished()
        {
            mBatttleFinished = true;
        }

        private EMessageType mStatus = EMessageType.EMT_None;
        private UILabel mCaption;
        private UILabel mMessage;
        private UIButton mSubmitBt;
        private UILabel mTimeLabel;
        private int mConnectCount = 30;
        private float mTimeCount;
        private UIButton m_CloseBtn;
        private bool mBatttleFinished;
    }
}

