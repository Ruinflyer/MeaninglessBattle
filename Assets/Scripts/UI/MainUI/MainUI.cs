using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
using MeaninglessNetwork;
public class MainUI : BaseUI
{
    //AutoStatement
    private Button Btn_Create = null;
    private Button Btn_Refresh = null;
    private ScrollRect RoomList = null;
    private ScrollRect MemberList = null;
    private Text Text_PlayerName = null;

    private UnityEngine.Object RoomListItem = null;
    private UnityEngine.Object MemberListItem = null;
    protected override void InitUiOnAwake()
    {

        RoomListItem = Resources.Load("UIPrefab/RoomListItem");
        MemberListItem = Resources.Load("UIPrefab/MemberListItem");


        Btn_Create = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Create");
        Btn_Create.onClick.AddListener(CreateRoom);

        Btn_Refresh = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Refresh");
        Btn_Refresh.onClick.AddListener(RefreshRoom);

        RoomList = GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "RoomList");
        MemberList = GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "MemberList");

        Text_PlayerName = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_PlayerName");

        MessageCenter.AddListener(EMessageType.PlayerName, (object obj) =>
        {
            Text_PlayerName.text = (string)obj;
        });


       
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.MainUI;
        NetworkManager.AddEventListener("GetRoomList", GetRoomList);
    }

    protected override void OnEnable()
    {
        RefreshRoom();
    }

    public void JoinRoom(int RoomIndex)
    {

        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("JoinRoom");
        protocol.SpliceInt(RoomIndex);
        NetworkManager.ServerConnection.Send(protocol, OnJoinRoomBack);
    }

    private void OnJoinRoomBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);

        if (returnCode == 0)
        {
            UIManager.Instance.ShowUI(UIid.RoomUI);
        }
        else
        {
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "加入房间失败";
            param[1] = "返回";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
        }

    }

    private void CreateRoom()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("CreateRoom");
        NetworkManager.ServerConnection.Send(protocol,OnCreateRoomBack);
    }

    private void OnCreateRoomBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);

        if (returnCode == 0)
        {
            
            UIManager.Instance.ShowUI(UIid.RoomUI);
            //UIManager.Instance.HideTheUI(this.uiId,()=>{ });
        }
        else
        {
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "创建房间失败";
            param[1] = "返回";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
        }
    }

    private void RefreshRoom()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("GetRoomList");
        NetworkManager.ServerConnection.Send(protocol);
    }

    private void GetRoomList(BaseProtocol protocol)
    {
        for (int j = 0; j < RoomList.content.childCount; j++)
        {
            Destroy(RoomList.content.GetChild(j).gameObject);
        }

        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        string MethodName = p.GetString(startIndex, ref startIndex);
        int RoomCount = p.GetInt(startIndex, ref startIndex);
        for (int i = 0; i < RoomCount; i++)

        {
            int playerNum = p.GetInt(startIndex, ref startIndex);
            int RoomStatus = p.GetInt(startIndex, ref startIndex);
            AddRoomListItem(i, playerNum, RoomStatus);

        }
    }
    private void AddRoomListItem(int index, int PlayerNum, int RoomStatus)
    {
        GameObject gobj = Instantiate(RoomListItem) as GameObject;
        gobj.transform.SetParent(RoomList.content);
        Text Text_RoomName = GameTool.FindTheChild(gobj, "RoomName").GetComponent<Text>();
        Text Text_MemberCount = GameTool.FindTheChild(gobj, "MemberCount").GetComponent<Text>();
        Text Text_Status = GameTool.FindTheChild(gobj, "Status").GetComponent<Text>();
        Text_RoomName.text = "房间号" + index.ToString();
        Text_MemberCount.text = PlayerNum.ToString() + " / 10";
        if (RoomStatus == 0)
        {
            Text_Status.text = "准备中";
        }
        else
        {
            Text_Status.text = "游玩中";
        }

        gobj.GetComponent<MainUIRoomListItem>().DoubleClickEvent.AddListener(()=> 
        {
            JoinRoom(index);
        });

    }


    protected override void OnDestroy()
    {
        NetworkManager.ServerConnection.msgDistribution.DeleteEventListener("GetRoomList", GetRoomList);
    }
}
