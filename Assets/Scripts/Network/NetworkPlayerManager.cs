using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessNetwork;
using Meaningless;

public class NetworkPlayerManager : MonoBehaviour
{

    public Dictionary<string, NetworkPlayer> ScenePlayers = new Dictionary<string, NetworkPlayer>();

    // Use this for initialization
    void Start()
    {
        NetworkManager.AddEventListener("GetPlayersInfo", OnGetPlayersInfo);
        NetworkManager.AddEventListener("UpdatePlayerInfo", UpdatePlayerInfo);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 更新网络玩家数据
    /// </summary>
    /// <param name="protocol"></param>
    public void UpdatePlayerInfo(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string playerName = p.GetString(startIndex, ref startIndex);
        float HP = p.GetFloat(startIndex, ref startIndex);
        float posX = p.GetFloat(startIndex, ref startIndex);
        float posY = p.GetFloat(startIndex, ref startIndex);
        float posZ = p.GetFloat(startIndex, ref startIndex);
        float rotX = p.GetFloat(startIndex, ref startIndex);
        float rotY = p.GetFloat(startIndex, ref startIndex);
        float rotZ = p.GetFloat(startIndex, ref startIndex);
        int HeadItem = p.GetInt(startIndex, ref startIndex);
        int BodyItem = p.GetInt(startIndex, ref startIndex);
        int WeaponID = p.GetInt(startIndex, ref startIndex);
        int Layer= p.GetInt(startIndex, ref startIndex);
        string CurrentAction = p.GetString(startIndex, ref startIndex);

        if (ScenePlayers.ContainsKey(playerName))
        {
            NetworkPlayer nPlayer = ScenePlayers[playerName].GetComponent<NetworkPlayer>();
            nPlayer.SetPlayerInfo(HP, HeadItem, BodyItem, WeaponID, Layer,CurrentAction);
            nPlayer.SetPlayerTransform(posX, posY, posZ, rotX, rotY, rotZ);
        }
    }


    /// <summary>
    /// 获取玩家列表回调
    /// </summary>
    public void OnGetPlayersInfo(BaseProtocol baseProtocol)
    {
        BytesProtocol p = baseProtocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int Playernum = p.GetInt(startIndex, ref startIndex);
        UnityEngine.Object playerprefab = Resources.Load("NetPlayerPrefab");
        GameObject tmp_player = null;
        for (int i = 0; i < Playernum; i++)
        {
            string playerName = p.GetString(startIndex, ref startIndex);
            //自己不需要更新
            if (playerName!=NetworkManager.PlayerName)
            {
                tmp_player = Instantiate(playerprefab) as GameObject;
                tmp_player.name = playerName;
                tmp_player.transform.SetParent(transform);

                NetworkPlayer nPlayer = tmp_player.GetComponent<NetworkPlayer>();
                nPlayer.SetPlayerName(playerName);
                ScenePlayers.Add(playerName, nPlayer);
                nPlayer.LastUpdateTime = NetworkManager.GetTimeStamp();
            }

            
        }

    }
    /// <summary>
    /// 删除玩家
    /// </summary>
    public void DelPlayer(string playerName)
    {
        if (ScenePlayers.ContainsKey(playerName))
        {
            ScenePlayers.Remove(playerName);
        }
    }

    /// <summary>
    /// 玩家离开
    /// </summary>
    public void PlayerLeave(string playerName)
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("LeaveRoom");
        NetworkManager.ServerConnection.Send(p, OnLeaveRoomBack);
        DelPlayer(playerName);

    }

    private void OnLeaveRoomBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int returnCode = p.GetInt(startIndex, ref startIndex);
        if (returnCode == 0)
        {
            UIManager.Instance.ShowUI(UIid.MainUI);
        }

    }


}
