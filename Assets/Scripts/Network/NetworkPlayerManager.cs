using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessNetwork;
using Meaningless;

/// <summary>
/// 网络玩家管理器，一些战斗事件也在这里
/// </summary>
public class NetworkPlayerManager : MonoBehaviour
{

    public Dictionary<string, NetworkPlayer> ScenePlayers = new Dictionary<string, NetworkPlayer>();
    // Use this for initialization
    void Start()
    {
        NetworkManager.AddEventListener("GetPlayersInfo", OnGetPlayersInfo);
        NetworkManager.AddEventListener("UpdatePlayerInfo", UpdatePlayerInfo);
        NetworkManager.AddEventListener("PlayerKilled",OnPlayerKilled);
        NetworkManager.AddEventListener("PlayerEquipHelmet", OnPlayerEquipHelmet);
        NetworkManager.AddEventListener("PlayerEquipClothe", OnPlayerEquipClothe);
        NetworkManager.AddEventListener("PlayerEquipWeapon", OnPlayerEquipWeapon);
        NetworkManager.AddEventListener("PlayerMagic",OnPlayerMagicBack);
        NetworkManager.AddEventListener("PlayerSuccess", OnPlayerSuccess);
        NetworkManager.AddEventListener("PlayerDead", OnPlayerDead);
        NetworkManager.AddEventListener("PlayerGetBuff", OnPlayerGetBuff);
    }

    // Update is called once per frame
    void Update()
    {
        MessageCenter.Send(EMessageType.Remain, ScenePlayers.Count+1);
        //Debug.Log(ScenePlayers.Count+"Name"+NetworkManager.PlayerName);
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
        string CurrentAction = p.GetString(startIndex, ref startIndex);

        //自己的消息
        if(playerName==NetworkManager.PlayerName)
        {
            BagManager.Instance.characterStatus.HP = HP;
        }

        if (ScenePlayers.ContainsKey(playerName))
        {
            ScenePlayers[playerName].SetPlayerInfo(HP, CurrentAction);
            ScenePlayers[playerName].SetPlayerTransform(posX, posY, posZ, rotX, rotY, rotZ);
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
            ScenePlayers[playerName].gameObject.SetActive(false);
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
        NetworkManager.Send(p, OnLeaveRoomBack);
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

    /// <summary>
    /// 玩家死亡处理
    /// </summary>
    /// <param name="protocol"></param>
    private void OnPlayerKilled(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        //杀手名
        string KillerName= p.GetString(startIndex, ref startIndex);
        //被杀玩家名
        string KilledPlayerName= p.GetString(startIndex, ref startIndex); 

        //玩家死亡处理
        if(KilledPlayerName==NetworkManager.PlayerName)
        {
            UIManager.Instance.ShowUI(UIid.FinishUI);
            object[] param = new object[3];
            param[0] = "无畏之争——无谓之争";
            param[1] = "你被 "+KillerName+" 击杀";
            param[2] = " ";
            MessageCenter.Send_Multparam(EMessageType.FinishUI,param);
            CameraBase.Instance.isFollowing = false;
            return;
        }

        DelPlayer(KilledPlayerName);
        
    }
    private void OnPlayerDead(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string KilledPlayerName = p.GetString(startIndex, ref startIndex);
        if(KilledPlayerName==NetworkManager.PlayerName)
        {
            if(CameraBase.Instance.isEscape==false)
            {
                UIManager.Instance.ShowUI(UIid.FinishUI);
                object[] param = new object[3];
                param[0] = "无畏之争，无谓之争";
                param[1] = "你被暗影毒杀";
                param[2] = " ";
                MessageCenter.Send_Multparam(EMessageType.FinishUI, param);
                CameraBase.Instance.isFollowing = false;
            }
            
        }
        DelPlayer(KilledPlayerName);
    }
    private void OnPlayerSuccess(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        UIManager.Instance.ShowUI(UIid.FinishUI);
        object[] param = new object[3];
        param[0] = "无畏之争，无胃之争";
        param[1] = "今晚不会有人抢你的鸡吃了";
        param[2] = "胜利者 ";
        MessageCenter.Send_Multparam(EMessageType.FinishUI, param);
        CameraBase.Instance.isFollowing = false;
    }
    private void OnPlayerEquipHelmet(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string Playername= p.GetString(startIndex, ref startIndex);
        int ItemID = p.GetInt(startIndex, ref startIndex);
        Debug.Log("网络玩家 "+Playername+"  又要威又要戴头盔");
        if(ScenePlayers.ContainsKey(Playername))
        {
            ScenePlayers[Playername].SetPlayerHelmet(ItemID);
        }
    }
    private void OnPlayerEquipClothe(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string Playername = p.GetString(startIndex, ref startIndex);
        int ItemID = p.GetInt(startIndex, ref startIndex);
        if (ScenePlayers.ContainsKey(Playername))
        {
            ScenePlayers[Playername].SetPlayeClothe(ItemID);
        }
    }
    private void OnPlayerEquipWeapon(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string Playername = p.GetString(startIndex, ref startIndex);
        int ItemID = p.GetInt(startIndex, ref startIndex);
        if (ScenePlayers.ContainsKey(Playername))
        {
            ScenePlayers[Playername].SetPlayerWeapon(ItemID);
        }
    }

    /// <summary>
    /// 魔法回调
    /// </summary>
    /// <param name="protocol"></param>
    private void OnPlayerMagicBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string MagicName = p.GetString(startIndex, ref startIndex);
        float posX = p.GetFloat(startIndex, ref startIndex);
        float posY = p.GetFloat(startIndex, ref startIndex);
        float posZ = p.GetFloat(startIndex, ref startIndex);
        float rotX = p.GetFloat(startIndex, ref startIndex);
        float rotY = p.GetFloat(startIndex, ref startIndex);
        float rotZ = p.GetFloat(startIndex, ref startIndex);
        
        NetPoolManager.Instantiate(MagicName,new Vector3(posX,posY,posZ), Quaternion.Euler(rotX, rotY, rotZ));
    }
    private void OnPlayerGetBuff(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        string playerName= p.GetString(startIndex,ref startIndex);
        int bufftype=p.GetInt(startIndex, ref startIndex);
        float buffTime=p.GetFloat(startIndex,ref startIndex);

        if(playerName==NetworkManager.PlayerName)
        {
            CameraBase.Instance.player.GetComponent<PlayerController>().GetDeBuffInTime((BuffType)bufftype,buffTime, BagManager.Instance.characterStatus);
        }
    }
}
