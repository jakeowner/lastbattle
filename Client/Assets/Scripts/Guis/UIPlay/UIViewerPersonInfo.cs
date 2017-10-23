using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JT.FWW.Tools;
using BlGame.GameEntity;
using System;
using BlGame.GameData;

public class UIViewerPersonInfo : MonoBehaviour {

    public static UIViewerPersonInfo Instance
    {
        private set;
        get;
    }

    Dictionary<Iplayer, PersonHead> PlayerInfoDict = new Dictionary<Iplayer, PersonHead>();
    List<PersonHead> PersonList = new List<PersonHead>();
    public Iplayer SetCurrClickPlayer
    {
        private set;
        get;
    }
    
    float StartTime = 0f;

    void Awake()
    {

    }
    void Update() {
        foreach (var item in PlayerInfoDict.Values)
        {
            item.OnUpdate();
        }
    }
    void OnEnable()
    {
        Instance = this;
        int index = this.transform.childCount;
        for (int i = 0; i < index; i++)
        {
            Transform tran = this.transform.Find("Player" + (i + 1));
            PersonList.Add(new PersonHead(tran));
            ButtonOnPress button = tran.GetComponent<ButtonOnPress>();
            button.AddListener(i, OnPress);
        }
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_HeroInfoChange, OnUpdateHeroInfo);
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_HeroHpChange, OnUpdateHeroHp);
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_HeroLvChange, OnUpdateHeroLevel);
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_HeroMpChange, OnUpdateHeroMp);
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_HeroDeathTime, OnUpdateHeroDeathTime);
        EventCenter.AddListener<Iplayer>(EGameEvent.eGameEvent_PersonInitInfo, InitViewPersonInfo);
    }

    void OnDisable()
    {
        Instance = null;
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_HeroInfoChange, OnUpdateHeroInfo);
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_HeroHpChange, OnUpdateHeroHp);
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_HeroLvChange, OnUpdateHeroLevel);
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_HeroMpChange, OnUpdateHeroMp);
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_HeroDeathTime, OnUpdateHeroDeathTime);
        EventCenter.RemoveListener<Iplayer>(EGameEvent.eGameEvent_PersonInitInfo, InitViewPersonInfo);
    }
    void OnUpdateHeroDeathTime(Iplayer player)
    {
        StartTime = PlayerPersonData.Instance.DeathTime;
        
        PersonHead head;
        if (!PlayerInfoDict.TryGetValue(player, out head))
        {
            return;
        }
        if (StartTime <= 0)
        {
            head.OnUpdateDeathTime((int)--StartTime);
        }
        head.isDownTime = true;
        head.time = (int)StartTime;
        head.OnUpdateDeathTime((int)StartTime);
    }
    /// <summary>
    /// 更新信息
    /// </summary>
    /// <param name="sGUID"></param>
    void OnUpdateHeroInfo(Iplayer player)
    {
        PersonHead head;
        if (!PlayerInfoDict.TryGetValue(player, out head))
        {
            return;
        }
        head.SetIsVib(true);
        head.OnUpdateHp();
        head.OnUpdateMp();
        head.OnUpdateLevel();
        head.OnUpdateHeadPt();
    }

    public void OnUpdateHeroLevel(Iplayer player)
    {
        PersonHead hd;
        if (!PlayerInfoDict.TryGetValue(player, out hd))
        {
            return;
        }
        hd.OnUpdateLevel();
    }
    
    public void OnUpdateHeroHp(Iplayer player)
    {
        PersonHead hd;
        if (!PlayerInfoDict.TryGetValue(player, out hd))
        {
            return;
        }
        hd.OnUpdateHp();
    }

    public void OnUpdateHeroMp(Iplayer player)
    {
        PersonHead hd;
        if (!PlayerInfoDict.TryGetValue(player, out hd))
        {
            return;
        }
        hd.OnUpdateMp();
    }
    public void SetVib()
    {
        foreach (var item in PersonList)
        {
            item.SetFrame(false);
        }
    }
    void SetHandOnPress(bool ispress)
    {
        if (UIDragObCamera.Instance != null)
        {
            UIDragObCamera.Instance.SetUsable(ispress);
        }
    }
    void OnPress(int ie , bool isPress)
    {
        if (isPress)
        {
            SetHandOnPress(false);
            return;
        }
        PersonHead hd = PersonList[ie];
        if (hd.SGUID == null || hd.Player == null)
        {
            return;
        }
        SetHandOnPress(true);
        SetVib();
        if(hd.Player != null)
        {
            SetCurrClickPlayer = hd.Player;
            PersonList[ie].SetFrame(true);
            SetPlayerInfo(ie, SetCurrClickPlayer);
        }
    }
    public void SetCurrCamearLockHead()
    {
        int index = 6;
        foreach (var item in PersonList)
        {
            if (item == null)
                continue;
            if (item.Player == SetCurrClickPlayer)
            {
                index = item.CampID;
            }
        }
        SetPlayerInfo(index - 1, SetCurrClickPlayer);
    }
    public void SetInitCamearHead()
    {
        int index = 6;
        foreach (var item in PersonList)
        {
            if (item.Player == null)
                continue;
            if(item.CampID <= index)
            {
                index = item.CampID;
                SetCurrClickPlayer = item.Player;
            }
        }
        if (SetCurrClickPlayer != null)
        {
            SetPlayerInfo(index - 1, SetCurrClickPlayer);
        }
    }
    void SetPlayerInfo(int index,Iplayer player) {
        SetCurrClickPlayer = PersonList[index].Player;
        if (UIViewerPlayerPackage.Instance != null)
        {
            UIViewerPlayerPackage.Instance.InfoPlayerPackage(SetCurrClickPlayer, index);
        }
        if (UIViewerSkillInfo.Instance != null)
        {
            UIViewerSkillInfo.Instance.InfoPlayerSkill(SetCurrClickPlayer);
        }
        GameDefine.GameMethod.GetMainCamera.transform.GetComponent<SmoothFollow>().enabled = true;
        GameDefine.GameMethod.GetMainCamera.target = player.realObject.transform;
    }
    /// <summary>
    /// 初始化角色信息
    /// </summary>
    /// <param name="sGUID"></param>
    public void InitViewPersonInfo(Iplayer player)
    {
        int campID = PlayerPersonData.Instance.CampID;
        PersonHead head = PersonList[campID -1];
        if (head.Player != null)
        {
            return;
        }
        head.Player = player;
        //head.SGUID = sGUID;
        head.CampID = campID;
        PlayerInfoDict.Add(player, head);
        
    }

    void PlayerHpChange(UInt64 sGUID)
    { 

    }

    class PersonHead
    {
        public UISprite PlayerIcon;
        public UISprite PlayerHp;
        public UISprite PlayerMp;
        public UILabel PlayerLevel;
        public UInt64 SGUID;
        public int CampID;
        public int time;
        public Iplayer Player;
        public UISprite Frame;
        public UISprite GreyPut;
        public UILabel DeathTime;
        public bool isDownTime = false; 
        float CurrTime = 0;
        public PersonHead(Transform tran)
        {
            PlayerIcon = tran.Find("Position/Portrait").GetComponent<UISprite>();
            PlayerHp = tran.Find("Position/HP").GetComponent<UISprite>();
            PlayerMp = tran.Find("Position/MP").GetComponent<UISprite>();
            PlayerLevel = tran.Find("Position/Level").GetComponent<UILabel>();
            Frame = tran.Find("Position/Frame").GetComponent<UISprite>();
            GreyPut = tran.Find("Position/Death").GetComponent<UISprite>();
            DeathTime = tran.Find("Position/LabelCount").GetComponent<UILabel>();
            GreyPut.gameObject.SetActive(false);
            PlayerIcon.spriteName = "";
            PlayerHp.fillAmount = 1;
            PlayerMp.fillAmount = 1;
            PlayerLevel.text = "1";
            SetFrame(false);
            SetIsVib(false);
        }
        public void SetFrame(bool isVib)
        {
            Frame.gameObject.SetActive(isVib);
        }
        public void SetIsVib(bool isVib)
        {
            PlayerIcon.gameObject.SetActive(isVib);
            PlayerHp.gameObject.SetActive(isVib);
            PlayerMp.gameObject.SetActive(isVib);
            PlayerLevel.gameObject.SetActive(isVib);
        }
        public void OnUpdateHp()
        {
            PlayerHp.fillAmount = Player.Hp / Player.HpMax;
            GreyPut.gameObject.SetActive(PlayerHp.fillAmount <= 0);
            DeathTime.gameObject.SetActive(PlayerHp.fillAmount <= 0);
        }
        
        public void OnUpdateDeathTime(int Time)
        {
            DeathTime.text = Time.ToString();
        }

        public void OnUpdate() {
            if (isDownTime)
            {
                if (Time.time - CurrTime >= 1f)
                {
                    time--;
                    CurrTime = Time.time;
                    OnUpdateDeathTime(time);
                    if (time <= 0f)
                    {
                        isDownTime = false;
                    }
                }
            }
        }

        public void OnUpdateMp()
        {
            PlayerMp.fillAmount = Player.Mp / Player.MpMax;
        }

        public void OnUpdateLevel()
        {
            PlayerLevel.text = Player.Level.ToString();
        }

        public void OnUpdateHeadPt()
        {
            HeroSelectConfigInfo info = ConfigReader.GetHeroSelectInfo(Player.NpcGUIDType);
            if (info != null)
            {
                PlayerIcon.spriteName = info.HeroSelectHead.ToString();
            }
        }

        public void SetCurrInfo(UInt64 sGUID, int camp, UInt32 headID, UInt32 level, UInt32 Hp, UInt32 Mp)
        {
            //SGUID = sGUID;
            //int id = (int)CTools.GetGUIDType(sGUID);
            //HeroSelectConfigInfo info = ConfigReader.GetHeroSelectInfo(id);
            //if (info != null)
            //PlayerIcon.spriteName = info.HeroSelectHead.ToString();
            //PlayerLevel.text = level.ToString();
            //hp = (float)Hp;
            //mp = (float)Mp;
            //MpMax = ConfigReader.GetHeroInfo(id).HeroMaxMp;
            //HpMax = ConfigReader.GetHeroInfo(id).HeroMaxHp;
        }
    }
}

