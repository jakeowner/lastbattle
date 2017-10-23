using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using GameDefine;
using System.Linq;
using BlGame.GameData;
using BlGame.Network;
using BlGame.Resource;
using BlGame.Ctrl;

public class UISelelctedServer : MonoBehaviour
{
    private List<ButtonOnPress> toggleList = new List<ButtonOnPress>();
    private List<UISprite> toggleSprite = new List<UISprite>();
    private List<UILabel>  nameList = new List<UILabel>();
    private List<UILabel> stateList = new List<UILabel>();
    private UILabel nameDefault;
    private UILabel stateDefault; 
    private UIGrid grid; 

    void Awake() { 
        grid = transform.GetComponentInChildren<UIGrid>();
        nameDefault = transform.Find("Static/Label3").GetComponent<UILabel>();
        stateDefault = transform.Find("Static/Label4").GetComponent<UILabel>(); 
        CreateServerItems();       
    }
     

    void ShowAllServer(int type = 0) { // type = 0  show name,type = 1 show addr
        for (int i = 0; i < nameList.Count; i++)
        {
            SelectServerData.ServerInfo info = SelectServerData.Instance.GetServerDicInfo().ElementAt(i).Value;
            nameList.ElementAt(i).text = (type == 0) ? info.name : info.addr;
            stateList.ElementAt(i).text = (type == 0) ? ("(" + SelectServerData.Instance.StateString[(int)(info.state)] + ")") : "";
            SetLabelColor(stateList.ElementAt(i), info.state);
        }
    }

    void CreateServerItems() {//create items ,and label 
        int count = SelectServerData.Instance.GetServerDicInfo().Count / 6;
        if (SelectServerData.Instance.GetServerDicInfo().Count % 6 != 0)
            count += 1;
        for (int i = 0; i < count * 6; i++) {
            ResourceUnit sceneRootUnit = ResourcesManager.Instance.loadImmediate(GameConstDefine.LoadServerItems, ResourceType.PREFAB);
            GameObject obj = GameObject.Instantiate(sceneRootUnit.Asset) as GameObject;
            obj.transform.parent = grid.transform;
            obj.transform.localScale = Vector3.one;
            obj.name = "Item" + (i+1).ToString();
            ButtonOnPress toggle = obj.GetComponent<ButtonOnPress>();
            UILabel label = obj.transform.Find("Panel/Label1").GetComponent<UILabel>();
            if (i >= SelectServerData.Instance.GetServerDicInfo().Count)
            {
                label.color = new Color(0f, 0f, 0f );
                label.effectStyle = UILabel.Effect.Outline;
                label.effectColor = new Color(82/255f, 82/255f, 82/255f);
                label.effectDistance = new Vector2(0.5f, 0.5f);
                Destroy(toggle);
                continue;
            }
            UILabel label2 = obj.transform.Find("Panel/Label2").GetComponent<UILabel>();

            toggleSprite.Add(obj.transform.Find("Panel/BG2").GetComponent<UISprite>());
            toggleList.Add(toggle);            
            nameList.Add(label);            
            stateList.Add(label2);
        }
    }

    void OnEnable() {
        ShowAllServer();
        ShowDefaultServer();
        //add listerner sdk end
        for (int i = 0; i < toggleList.Count; i++) {
            toggleList.ElementAt(i).AddListener(i, OnEvent);
        }  
    }

    void OnEvent(int ie ,bool isDown) {
        LoginCtrl.Instance.SelectServer(ie);
        ShowSelectSprite(ie);
    }


    void ShowSelectSprite(int ie) {
        bool tag = false;
        if (ie >= toggleSprite.Count)
            return;
        for (int i = 0; i < toggleSprite.Count; i++)
        {
            tag = false;
            if (i == ie)
            {
                tag = true;
            }
            toggleSprite.ElementAt(i).gameObject.SetActive(tag);
        }
    }
    

    void ShowDefaultServer(int type = 0) {// type = 0  show name,type = 1 show addr  
        SelectServerData.ServerInfo info = SelectServerData.Instance.curSelectServer;
        nameDefault.text = (type == 0) ? info.name : info.addr;        
        stateDefault.text = (type == 0) ? ("("+SelectServerData.Instance.StateString[(int)info.state]+")") : "";
        SetLabelColor(stateDefault, info.state);
        ShowSelectSprite(SelectServerData.Instance.curSelectIndex);
    }

    void SetLabelColor(UILabel label, SelectServerData.ServerState state) {
        switch (state)
        {
            case SelectServerData.ServerState.Busy:
                label.color = Color.yellow;
                break;
            case SelectServerData.ServerState.Fluent:
                label.color = Color.green;
                break;
            case SelectServerData.ServerState.HouseFull:
                label.color = Color.red;
                break;
        }
    }

    int GetSelectIndex(UIToggle toggle) {
        for (int i = 0; i < toggleList.Count; i++) {
            if (toggleList.ElementAt(i) == toggle)
                return i;
        }
        return -1;
    }

    void OnDisable() {
        //remove listerner sdk end
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList.ElementAt(i).RemoveListener(OnEvent);
        }  
    }

/*
    void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 20f, 100f, 100f), "显示名字"))
        {
            ShowAllServer(0);
            ShowDefaultServer(0);
        }

        if (GUI.Button(new Rect(0f, 200f, 100f, 100f), "显示IP"))
        {
            ShowAllServer(1);
            ShowDefaultServer(1);
        }
    }
*/
}
