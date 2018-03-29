using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;

public class MainUI : BaseUI
{
    //AutoStatement
    private Button Btn_Start = null;
    private Button Btn_RandomName = null;
    private InputField inputField = null;
    private RandomName randomName;

    protected override void InitUiOnAwake()
    {
        randomName = new RandomName();
        randomName.LoadRandomNameData();

        Btn_Start = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Start");
        Btn_Start.onClick.AddListener(StartGame);

        Btn_RandomName = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_RandomName");
        Btn_RandomName.onClick.AddListener(SetPlayerName);

        inputField = GameTool.GetTheChildComponent<InputField>(this.gameObject, "InputField");
        inputField.text = randomName.GetRandomName();

        

    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.MainUI;
        
    }

    protected override void Start()
    {
        UIManager.Instance.ShowUI(UIid.LoadingUI);
        MessageCenter.Send(EMessageType.Loading, 0);
    }
    private void StartGame()
    {

        
        
        PhotonNetwork.playerName = inputField.text;

        UIManager.Instance.ShowUI(UIid.LoadingUI);
        MessageCenter.Send(EMessageType.Loading,1);
    }

    private void SetPlayerName()
    {
        inputField.text = randomName.GetRandomName();
    }
}
