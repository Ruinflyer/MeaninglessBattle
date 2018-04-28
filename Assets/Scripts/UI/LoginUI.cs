using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
using MeaninglessNetwork;
public class LoginUI : BaseUI
{
    //AutoStatement
    private Button Btn_RandomName = null;
    private Button Btn_Login = null;
    private InputField inputField = null;
    private RandomName randomName;
    private ClientConf clientConf;
    protected override void InitUiOnAwake()
    {

        randomName = new RandomName();
        randomName.LoadRandomNameData();

        Btn_RandomName = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_RandomName");
        Btn_RandomName.onClick.AddListener(SetPlayerName);

        Btn_Login = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Login");
        Btn_Login.onClick.AddListener(StartGame);


        inputField = GameTool.GetTheChildComponent<InputField>(this.gameObject, "InputField");
        inputField.text = randomName.GetRandomName();

        clientConf = MeaninglessJson.LoadJsonFromFile<ClientConf>(MeaninglessJson.Path_StreamingAssets + "ClientConf.json");
        
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.LoginUI;
        
    }

    
    private void SetPlayerName()
    {
        inputField.text = randomName.GetRandomName();
    }

    private void StartGame()
    {
        
        if (inputField.text == "")
        {
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "昵称不可以为空";
            param[1] = "知道了";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
            return;
        }

        //未连接则连接
        if (NetworkManager.ServerConnection.connectionStatus != Connect.ConnectionStatus.Connected)
        {
            NetworkManager.ServerConnection.protocol = new BytesProtocol();
            NetworkManager.ServerConnection.ConnectToServer(clientConf.Host, clientConf.Port);
        }

        
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("Connect");
        protocol.SpliceString(inputField.text);
        NetworkManager.PlayerName = inputField.text;
        NetworkManager.ServerConnection.Send(protocol, OnConnectCallback);


        //UIManager.Instance.ShowUI(UIid.LoadingUI);
        //MessageCenter.Send(EMessageType.Loading, 1);
    }

    private void OnConnectCallback(BaseProtocol protocol)
    {
        
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        string MethodName = p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);
        if (returnCode == 0)
        {
            
            Debug.Log("连接成功");
            
            UIManager.Instance.ShowUI(UIid.MainUI);
            MessageCenter.Send(EMessageType.PlayerName, inputField.text);
            UIManager.Instance.HideTheUI(this.uiId,()=> { });

        }
        else
        {
            Debug.Log("连接失败");
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "连接失败\n可能原因：昵称已被使用、服务器问题 或 网络故障";
            param[1] = "知道了";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
        }
    }
}
