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
    public class TimeDownWindow : BaseWindow
    {
        public TimeDownWindow()
        {
            mScenesType = EScenesType.EST_Play;
            mResName = GameConstDefine.LoadTimeDownUI;
            mResident = false;

            mStart = false;
        }

        ////////////////////////////继承接口/////////////////////////
        //类对象初始化
        public override void Init()
        {
            EventCenter.AddListener<long>(EGameEvent.eGameEvent_BattleTimeDownEnter, ShowTime);
            EventCenter.AddListener<long>(EGameEvent.eGameEvent_GameStartTime,Stop);
        }

        //类对象释放
        public override void Realse()
        {
            EventCenter.RemoveListener<long>(EGameEvent.eGameEvent_BattleTimeDownEnter, ShowTime);
            EventCenter.RemoveListener<long>(EGameEvent.eGameEvent_GameStartTime, Stop);
        }

        //窗口控件初始化
        protected override void InitWidget()
        {
            mMinute = mRoot.Find("Minute").GetComponent<UISprite>();
            mSecond = mRoot.Find("Second").GetComponent<UISprite>();
            mAnimation = mRoot.GetComponent<Animation>();
        }


        //窗口控件释放
        protected override void RealseWidget()
        {
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
            mStart = false;
        }

        //隐藏
        public override void OnDisable()
        {
        }

        private void ShowTime(long time)
        {
            if (time < 0)
                return;

            Show();

            mTime = time;
            mDeltaTime = Time.time;
            mStart = true;

            SetTime(mTime);
        }

        private void Stop(long time)
        {
            Hide();
        }

        public override void Update(float deltaTime)
        {
            if (!mStart)
                return;

            if (Time.time - mDeltaTime > 1f)
            {
                mTime -= 1;
                mDeltaTime = Time.time;

                if (mTime <= 0)
                {
                    Hide();
                    return;
                }

                SetTime(mTime);
            }
        }

        private void SetTime(long time)
        {
            long ten = time / 10;
            long unit = time % 10;

            mMinute.spriteName = ten.ToString();
            mSecond.spriteName = unit.ToString();
            mAnimation.Play(PlayMode.StopAll);
        }

        UISprite mMinute;
        UISprite mSecond;
        Animation mAnimation;
        long mTime;
        float mDeltaTime;
        bool mStart;
    }

}
