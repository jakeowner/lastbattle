using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JT.FWW.GameData;
using GameDefine;
using UICommon;
using BlGame;
using BlGame.GameData;
using BlGame.Network;
using System.Linq;
using BlGame.Ctrl;

namespace BlGame.View
{
    public class LoginWindow : BaseWindow
    {
        public LoginWindow() 
        {
            mScenesType = EScenesType.EST_Login;
            mResName = GameConstDefine.LoadGameLoginUI;
            mResident = false;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener(EGameEvent.eGameEvent_LoginEnter, Show);
            EventCenter.AddListener(EGameEvent.eGameEvent_LoginExit, Hide);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LoginEnter, Show);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LoginExit, Hide);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            mLoginParent = mRoot.Find("Server_Choose");
            mLoginInput = mRoot.Find("Server_Choose/Loginer");
            mLoginSubmit = mRoot.Find("Server_Choose/Button");
            mLoginAccountInput = mRoot.Find("Server_Choose/Loginer/AcountInput").GetComponent<UIInput>();
            mLoginPassInput = mRoot.Find("Server_Choose/Loginer/PassInput").GetComponent<UIInput>();

            mPlayParent = mRoot.Find("LoginBG");
            mPlaySubmitBtn = mRoot.Find("LoginBG/LoginBtn");
            mPlayServerBtn = mRoot.Find("LoginBG/CurrentSelection");
            mPlayNameLabel = mRoot.Find("LoginBG/CurrentSelection/Label3").GetComponent<UILabel>();
            mPlayStateLabel = mRoot.Find("LoginBG/CurrentSelection/Label4").GetComponent<UILabel>();
            mPlayAnimate = mPlaySubmitBtn.GetComponent<Animator>();

            mChangeAccountBtn = mRoot.Find("ChangeAccount");
            mChangeAccountName = mRoot.Find("ChangeAccount/Position/Label1").GetComponent<UILabel>();

            mServerParent = mRoot.Find("UIGameServer");

            mReLoginParent = mRoot.Find("LogInAgain");
            mReLoginSubmit = mRoot.Find("LogInAgain/Status1/Button");

            mVersionLable = mRoot.Find("Label").GetComponent<UILabel>();
            mWaitingParent = mRoot.Find("Connecting");


            UIEventListener.Get(mPlaySubmitBtn.gameObject).onClick += OnPlaySubmit;
            UIEventListener.Get(mPlayServerBtn.gameObject).onClick += OnPlayServer;

            UIEventListener.Get(mChangeAccountBtn.gameObject).onClick += OnChangeAccount;

            UIEventListener.Get(mReLoginSubmit.gameObject).onClick += OnReLoginSubmit;

            UIEventListener.Get(mLoginSubmit.gameObject).onClick += OnLoginSubmit;

            mServerList.Clear();
            for (int i = 0; i < 4; i++)
            {
                UIToggle toggle = mLoginParent.Find("Server" + (i + 1).ToString()).GetComponent<UIToggle>();
                mServerList.Add(toggle);
            }

            for (int i = 0; i < mServerList.Count; i++)
            {
                EventDelegate.Add(mServerList.ElementAt(i).onChange, OnSelectIp);
            }


            DestroyOtherUI();
        }

        //删除Login外其他控件，例如
        public static void DestroyOtherUI()
        {
            Camera camera = GameMethod.GetUiCamera;
            for (int i = 0; i < camera.transform.childCount; i++)
            {
                if (camera.transform.GetChild(i) != null && camera.transform.GetChild(i).gameObject != null)
                {

                    GameObject obj = camera.transform.GetChild(i).gameObject;
                    if (obj.name != "UIGameLogin(Clone)")
                    {
                        GameObject.DestroyImmediate(obj);
                    }                    
                }
            }
        }

        //窗口控件释放
        protected override void RealseWidget()
        {
        }

        //游戏事件注册
        protected override void OnAddListener()
        {
            EventCenter.AddListener<EErrorCode>(EGameEvent.eGameEvent_LoginError, LoginFail);//登錄反饋
            EventCenter.AddListener(EGameEvent.eGameEvent_LoginSuccess, LoginSuceess);
            EventCenter.AddListener<string,string>(EGameEvent.eGameEvent_SdkRegisterSuccess, SdkRegisterSuccess);//sdk register success
            EventCenter.AddListener(EGameEvent.eGameEvent_SdkServerCheckSuccess, SdkServerCheckSuccess);//sdk register success
            EventCenter.AddListener(EGameEvent.eGameEvent_SelectServer, SelectServer);//选择了服务器
            EventCenter.AddListener(EGameEvent.eGameEvent_LoginFail, ShowLoginFail);
            EventCenter.AddListener(EGameEvent.eGameEvent_SdkLogOff, SdkLogOff);
        }

        //游戏事件注消
        protected override void OnRemoveListener()
        {
            EventCenter.RemoveListener<EErrorCode>(EGameEvent.eGameEvent_LoginError, LoginFail);
            EventCenter.RemoveListener<string,string>(EGameEvent.eGameEvent_SdkRegisterSuccess, SdkRegisterSuccess);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_SdkServerCheckSuccess, SdkServerCheckSuccess);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_SelectServer, SelectServer);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LoginFail, ShowLoginFail);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_LoginSuccess, LoginSuceess);
            EventCenter.RemoveListener(EGameEvent.eGameEvent_SdkLogOff, SdkLogOff);
        }

        //显示
        public override void OnEnable()
        {
            mVersionLable.text = SdkConector.GetBundleVersion();
            mPlayAnimate.enabled = true;
            ShowServer(LOGINUI.None);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR || SKIP_SDK
            mLoginInput.gameObject.SetActive(true);
#endif
        }

        //隐藏
        public override void OnDisable()
        {
        }

        ////////////////////////////////UI事件响应////////////////////////////////////

        void OnPlaySubmit(GameObject go)
        {
            mWaitingParent.gameObject.SetActive(true);
            UIEventListener.Get(mPlaySubmitBtn.gameObject).onClick -= OnPlaySubmit;

            LoginCtrl.Instance.GamePlay();
        }

        void OnPlayServer(GameObject go)
        {
            ShowServer(LOGINUI.SelectServer);
        }

        void OnChangeAccount(GameObject go)
        {
            LoginCtrl.Instance.SdkLogOff();
        }

        void OnReLoginSubmit(GameObject go)
        {
            mReLoginParent.gameObject.SetActive(false);

            LoginCtrl.Instance.SdkLogOff();
        }

        void OnLoginSubmit(GameObject go)
        {
#if UNITY_STANDALONE_WIN
            if (string.IsNullOrEmpty(mLoginAccountInput.value))
                return;
            mLoginPassInput.value = "123";
#else
           if (string.IsNullOrEmpty(mLoginAccountInput.value) || string.IsNullOrEmpty(mLoginPassInput.value))
                return;
#endif


            mWaitingParent.gameObject.SetActive(true);

            LoginCtrl.Instance.Login(mLoginAccountInput.value, mLoginPassInput.value);
        }

        void OnSelectIp()
        {
            if (UIToggle.current == null || !UIToggle.current.value)
                return;
            for (int i = 0; i < mServerList.Count; i++)
            {
                if (mServerList.ElementAt(i) == UIToggle.current)
                {
                    LoginCtrl.Instance.SelectLoginServer(i);
                    break;
                }
            }
        }


        ////////////////////////////////游戏事件响应////////////////////////////////////

        //登录失败
        void LoginFail(EErrorCode errorCode)
        {
            mPlayAnimate.enabled = true;

            mPlaySubmitBtn.gameObject.SetActive(true);
            GameObject.DestroyImmediate(mPlayEffect.gameObject);
        }

        //登陆失败反馈
        void ShowLoginFail()
        {
            mReLoginParent.gameObject.SetActive(true);
            mWaitingParent.gameObject.SetActive(false);
            UIEventListener.Get(mPlaySubmitBtn.gameObject).onClick += OnPlaySubmit;
        }

        //登陆成功
        void LoginSuceess()
        {
            UIEventListener.Get(mPlaySubmitBtn.gameObject).onClick -= OnPlaySubmit;
        }

        //选择了服务器
        void SelectServer()
        {
            ShowSelectServerInfo();
            ShowServer(LOGINUI.Login);
        }

        //显示服务器信息或者显示登录信息
        void ShowServer(LOGINUI state)
        {
            bool showLogin = false;
            bool showServer = false;
            bool showSelectServer = false;
            switch (state)
            {
                case LOGINUI.Login:
                    ShowSelectServerInfo();
                    showLogin = true;
                    showServer = false;
                    showSelectServer = false;
                    break;
                case LOGINUI.SelectServer:
                    showLogin = false;
                    showServer = true;
                    showSelectServer = false;
                    break;
                case LOGINUI.None:
                    showLogin = false;
                    showServer = false;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR || SKIP_SDK
                    showSelectServer = true;
#endif
                    break;
            }
            mPlayParent.gameObject.SetActive(showLogin);
            mServerParent.gameObject.SetActive(showServer);
            mLoginParent.gameObject.SetActive(showSelectServer);
            if (showLogin)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR|| SKIP_SDK
                mChangeAccountName.text = mLoginAccountInput.value;
#else
                mChangeAccountName.text = SdkConector.NickName();
#endif
            }
            mChangeAccountBtn.gameObject.SetActive(showLogin);
        }

        //显示选中的server信息 
        void ShowSelectServerInfo()
        {
            SelectServerData.ServerInfo info = SelectServerData.Instance.curSelectServer;
            mPlayNameLabel.text = info.name;
            mPlayStateLabel.text = "(" + SelectServerData.Instance.StateString[(int)info.state] + ")";
            SelectServerData.Instance.SetLabelColor(mPlayStateLabel, info.state);
        }

        //SDK注册成功
        void SdkRegisterSuccess(string uid, string sessionId)
        {
            LoginCtrl.Instance.SdkRegisterSuccess(uid, sessionId);

            mWaitingParent.gameObject.SetActive(true);
        }

        //SDK检查成功
        void SdkServerCheckSuccess()
        {
            ShowServer(LOGINUI.Login);
            mWaitingParent.gameObject.SetActive(false);

            #if UNITY_STANDALONE_WIN || UNITY_EDITOR|| SKIP_SDK
            #else
                SdkConector.ShowToolBar(0);
            #endif
        }

        //SDK退出
        void SdkLogOff()
        {
            
            ShowServer(LOGINUI.None);

            mLoginPassInput.value = "";
            mLoginAccountInput.value = "";
        }

        IEnumerator ShakeLabel()
        {
            mPlayEffect = GameMethod.CreateWindow(GameConstDefine.LoadGameLoginEffectPath, new Vector3(-5, -270, 0), mRoot.transform);
            mPlaySubmitBtn.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.4f);
        }
        
        enum LOGINUI
        {
            None = -1,
            Login,
            SelectServer,
        }

        //开始
        Transform mPlayParent;
        Transform mPlaySubmitBtn;
        Transform mPlayServerBtn;
        UILabel mPlayNameLabel;
        UILabel mPlayStateLabel;
        Animator mPlayAnimate;
        GameObject mPlayEffect;

        //登录
        Transform mLoginParent;
        Transform mLoginInput;
        Transform mLoginSubmit;
        UIInput mLoginPassInput;
        UIInput mLoginAccountInput;

        //改变帐号
        Transform mChangeAccountBtn;
        UILabel mChangeAccountName;

        //选服
        Transform mServerParent;

        //重新登录选择
        Transform mReLoginParent;
        Transform mReLoginSubmit;

        //等待中
        Transform mWaitingParent;

        //版本号   
        UILabel mVersionLable;

        //服务器列表
        private List<UIToggle> mServerList = new List<UIToggle>();

    }

}
