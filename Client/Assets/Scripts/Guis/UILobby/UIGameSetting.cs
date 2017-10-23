using UnityEngine;
using System.Collections;
using JT.FWW.Tools;
using BlGame.GameEntity;
using GameDefine;
using JT.FWW.GameData;
using BlGame;
using BlGame.Network;
using BlGame.GameState;
public class UIGameSetting : MonoBehaviour
{
    private ButtonOnPress btnSoundEffect;
    private ButtonOnPress btnVoice;
    private ButtonOnPress btnChange;
    private GameObject[] objVoice = new GameObject[2];
    private GameObject[] ojbSound = new GameObject[2];
    private bool vOpenState = true;
    private bool sOpenState = true;

    public const string soundKey = "SoundKey";
    public const string voiceKey = "VoiceKey";

    void Awake()
    {
        btnChange = transform.Find("ChangeAccount").GetComponent<ButtonOnPress>();
        btnVoice = transform.Find("MusicSwitch").GetComponent<ButtonOnPress>();
        btnSoundEffect = transform.Find("SoundSwitch").GetComponent<ButtonOnPress>();
        objVoice[0] = btnVoice.transform.Find("On").gameObject;
        objVoice[1] = btnVoice.transform.Find("Off").gameObject;
        ojbSound[0] = btnSoundEffect.transform.Find("On").gameObject;
        ojbSound[1] = btnSoundEffect.transform.Find("Off").gameObject;
    }

    void OnEnable()
    {
        SetSave(ref sOpenState, soundKey);
        SetSave(ref vOpenState, voiceKey);
        SetVoiceEnable(vOpenState);
        SetSoundEnable(sOpenState);
        btnVoice.AddListener(OnVoiceChange);
        btnSoundEffect.AddListener(OnSoundChange);
        btnChange.AddListener(OnChangeAccount);
    }

    void OnDisable()
    {
        btnVoice.RemoveListener(OnVoiceChange);
        btnSoundEffect.RemoveListener(OnSoundChange);

    }

    void OnChangeAccount(int ie, bool isPress) {
        if (isPress)
            return;
        GameMethod.LogOutToLogin();
    }

    void OnVoiceChange(int ie, bool isPress) {
        if (isPress) {
            return;
        }
        vOpenState = !vOpenState;
        int state = vOpenState ? 1 : 0;
        PlayerPrefs.SetInt(voiceKey, state);
        SetVoiceEnable(vOpenState);
    }

    void OnSoundChange(int ie, bool isPress) {
        if (isPress)
        {
            return;
        }
        sOpenState = !sOpenState;
        int state = sOpenState ? 1 : 0;
        PlayerPrefs.SetInt(soundKey, state);
        SetSoundEnable(sOpenState);
    }

    void SetSave(ref bool saveState,string key) {
        saveState = true;
        if (PlayerPrefs.HasKey(key))
        {
            int state = PlayerPrefs.GetInt(key);
            saveState = (state == 1) ? true : false;
        }
        else
        {
            int state = saveState ? 1 : 0;
            PlayerPrefs.SetInt(key, state);
        }
    }

    void SetVoiceEnable(bool show)
    {
        int index = show ? 0 : 1;
        objVoice[index].SetActive(true);
        objVoice[1 - index].SetActive(false);
        AudioManager.Instance.EnableVoice(show);
    }

    void SetSoundEnable(bool show)
    {
        int index = show ? 0 : 1;
        ojbSound[index].SetActive(true);
        ojbSound[1 - index].SetActive(false);
        AudioManager.Instance.EnableSound(show);
    }
}
