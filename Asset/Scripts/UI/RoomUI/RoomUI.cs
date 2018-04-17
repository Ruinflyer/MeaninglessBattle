using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Meaningless;
using MeaninglessNetwork;
using UnityEngine.SceneManagement;

public class RoomUI : BaseUI
{
    //AutoStatement
    private Button Btn_Leave = null;
    private Button Btn_Prepare = null;
    private Button Btn_Start = null;
    private ScrollRect MemberList = null;
    private List<GameObject> MemberListItems = new List<GameObject>();
    protected override void InitUiOnAwake()
    {
        Btn_Leave = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Leave");
        Btn_Leave.onClick.AddListener(LeaveRoom);
        Btn_Prepare = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Prepare");


        Btn_Start = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Start");
        Btn_Start.onClick.AddListener(RequestStartGame);
        MemberList = GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "MemberList");

        NetworkManager.ServerConnection.msgDistribution.AddEventListener("GetRoomInfo", GetRoomInfo);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("StartGame", OnRequestStartGameBack);


    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.RoomUI;
        InitMemberListItem(MemberList.content);
    }

    protected override void OnEnable()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("GetRoomInfo");
        NetworkManager.ServerConnection.Send(protocol, GetRoomInfo);
    }



    private void LeaveRoom()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("LeaveRoom");
        NetworkManager.ServerConnection.Send(protocol, OnLeaveRoomBack);
    }

    private void OnLeaveRoomBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);
        if (returnCode != 0)
        {
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "离开房间失败，服务器无响应";
            param[1] = "知道了";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
        }
        else
        {
            UIManager.Instance.ShowUI(UIid.MainUI);
        }
    }

    private void RequestStartGame()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("RequestStartGame");
        NetworkManager.ServerConnection.Send(protocol, OnRequestStartGameBack);
    }

    private void OnRequestStartGameBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);
        if (returnCode != 0)
        {
            UIManager.Instance.ShowUI(UIid.TipsUI);
            object[] param = new object[2];
            param[0] = "无法开始，人数不够";
            param[1] = "返回";
            MessageCenter.Send_Multparam(EMessageType.TipsUI, param);
        }
        else
        {
            
            
            //游戏开始
            //MessageCenter.Send();
        }
    }

    private void GetRoomInfo(BaseProtocol protocol)
    {
        for (int j = 0; j < 10; j++)
        {
            MemberListItems[j].SetActive(false);
        }
        MemberListItem memberListItem;
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int PlayerNum = p.GetInt(startIndex, ref startIndex);
        for (int i = 0; i < PlayerNum; i++)
        {

            memberListItem = MemberListItems[i].GetComponent<MemberListItem>();
            string playerName = p.GetString(startIndex, ref startIndex);
            memberListItem.Text_PlayerName.text = playerName;

            int isMaster = p.GetInt(startIndex, ref startIndex);
            
            if (playerName == NetworkManager.PlayerName)
            {
                if(isMaster == 1)
                {
                    memberListItem.Text_Rank.text = "房主";
                    Btn_Start.gameObject.SetActive(true);
                }
                else
                {
                    Btn_Start.gameObject.SetActive(false);
                }
            }
            
            MemberListItems[i].SetActive(true);
        }
    }
    /// <summary>
    /// 实例化玩家列表
    /// </summary>
    /// <param name="parent"></param>
    private void InitMemberListItem(Transform parent)
    {
        GameObject Loaded = Resources.Load("UIPrefab/MemberListItem") as GameObject;
        GameObject tmp;
        MemberListItem memberListItem;
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(Loaded, parent);
            memberListItem = tmp.GetComponent<MemberListItem>();
            memberListItem.Text_PlayerName = GameTool.FindTheChild(tmp, "Text_PlayerName").GetComponent<Text>();
            memberListItem.Text_Rank = GameTool.FindTheChild(tmp, "Text_Rank").GetComponent<Text>();
            memberListItem.Text_Ready = GameTool.FindTheChild(tmp, "Text_Ready").GetComponent<Text>();
            MemberListItems.Add(tmp);
            tmp.gameObject.SetActive(false);
        }
    }
}
