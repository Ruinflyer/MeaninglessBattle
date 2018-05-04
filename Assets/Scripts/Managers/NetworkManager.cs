using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;
using System;

public class NetworkManager : MonoSingleton<NetworkManager>
{

    public static Connect ServerConnection = new Connect();
    public static string PlayerName="";
	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
        ServerConnection.Update();

    }

    public static BaseProtocol GetHeartBeat()
    {
        BytesProtocol bytesProtocol = new BytesProtocol();
        bytesProtocol.SpliceString("HeartBeat");
        return bytesProtocol;
    }

    /// <summary>
    /// 获取1970年1月1日零点到现在的时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

        return Convert.ToInt64(timeSpan.TotalSeconds);

    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="protocol"></param>
    public static void Send(BaseProtocol protocol)
    {
        ServerConnection.Send(protocol);
    }
    public static void Send(BaseProtocol protocol, DelegateEvent delegateEvent)
    {
        ServerConnection.Send(protocol,delegateEvent);
    }
    /// <summary>
    /// 添加网络事件监听
    /// </summary>
    /// <param name="MethodName">消息名</param>
    /// <param name="Callback">回调</param>
    public static void AddEventListener(string MethodName, DelegateEvent Callback)
    {
        ServerConnection.msgDistribution.AddEventListener(MethodName, Callback);
    }
    /// <summary>
    /// 添加单次网络事件监听，接收后回调一次事件监听将回收
    /// </summary>
    /// <param name="MethodName">消息名</param>
    /// <param name="Callback">回调</param>
    public static void AddOnceEventListener(string MethodName, DelegateEvent Callback)
    {
        ServerConnection.msgDistribution.AddOnceEventListener(MethodName, Callback);
    }


    /// <summary>
    /// 发送击中玩家消息
    /// </summary>
    /// <param name="Name">玩家名</param>
    /// <param name="Damage">伤害值</param>
    public static void SendPlayerHitSomeone(string Name,float Damage)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerHitSomeone");
        protocol.SpliceString(Name);
        protocol.SpliceFloat(Damage);
        ServerConnection.Send(protocol);
    }



    /// <summary>
    /// 发送获取玩家列表消息
    /// </summary>
    public static void SendGetPlayersInfo()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("GetPlayersInfo");
        ServerConnection.Send(protocol);
    }

    /// <summary>
    /// 发送玩家可以开玩
    /// </summary>
    public static void SendPlayerReady()
    {
        BytesProtocol ready = new BytesProtocol();
        ready.SpliceString("PlayerReady");
        ready.SpliceString(NetworkManager.PlayerName);
        Send(ready);
    }

    /// <summary>
    /// 地图加载完毕时上报服务端统计
    /// </summary>
    public static void SendMapLoaded()
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("MapLoaded");
        ServerConnection.Send(p);
    }

    /// <summary>
    /// 发送 玩家有害状态
    /// </summary>
    public static void SendPlayerGetBuff(string playerName,BuffType buffType,float buffTime)
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("PlayerGetBuff");
        p.SpliceString(playerName);
        p.SpliceInt((int)buffType);
        p.SpliceFloat(buffTime);
        Send(p);
    }


    /// <summary>
    /// 发送角色同步信息
    /// </summary>
    public static void SendUpdatePlayerInfo(Vector3 pos , Vector3 rot, string CurrentAction)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("UpdatePlayerInfo");
        protocol.SpliceFloat(pos.x);
        protocol.SpliceFloat(pos.y);
        protocol.SpliceFloat(pos.z);
        protocol.SpliceFloat(rot.x);
        protocol.SpliceFloat(rot.y);
        protocol.SpliceFloat(rot.z);
       
        protocol.SpliceString(CurrentAction);
        Send(protocol);
    }                      
    
    /// <summary>
    /// 发送拾取物品消息
    /// </summary>
    public static void SendPickItem(int GroundItemID)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PickItem");
        protocol.SpliceInt(GroundItemID);
        Send(protocol);
    }

    /// <summary>
    /// 发送门打开消息
    /// </summary>
    /// <param name="DoorID">门ID</param>
    public static void SendDoorOpen(int DoorID)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("DoorOpen");
        protocol.SpliceInt(DoorID);
        Send(protocol);
    }


    /// <summary>
    /// 发送戴头盔的ItemID
    /// </summary>
    public static void SendPlayerEquipHelmet(int ItemID)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerEquipHelmet");
        protocol.SpliceInt(ItemID);
        Send(protocol);
    }
    /// <summary>
    /// 发送拿衣服 衣服ID
    /// </summary>
    public static void SendPlayerEquipClothe(int ItemID)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerEquipClothe");
        protocol.SpliceInt(ItemID);
        Send(protocol);
    }
    /// <summary>
    /// 发送装备武器ID
    /// </summary>
    public static void SendPlayerEquipWeapon(int ItemID)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerEquipWeapon");
        protocol.SpliceInt(ItemID);
        Send(protocol);
    }
    /// <summary>
    /// 发送社保魔法
    /// </summary>
    public static void SendPlayerMagic(string MagicName,Vector3 pos,Quaternion rot )
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerMagic");
        protocol.SpliceString(MagicName);
        protocol.SpliceFloat(pos.x);
        protocol.SpliceFloat(pos.y);
        protocol.SpliceFloat(pos.z);
        protocol.SpliceFloat(rot.eulerAngles.x);
        protocol.SpliceFloat(rot.eulerAngles.y);
        protocol.SpliceFloat(rot.eulerAngles.z);
        Send(protocol);
    }

    /// <summary>
    /// 发送请求下落点
    /// </summary>
    public static void SendReqDroppoint()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("Droppoint");
        Send(protocol);
    }

    /// <summary>
    /// 发送扔物品消息
    /// </summary>
    public static void SendDropItem(int GroundItemID,Transform trans)
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("DropItem");
        protocol.SpliceInt(GroundItemID);
        protocol.SpliceFloat(trans.position.x);
        protocol.SpliceFloat(trans.position.y);
        protocol.SpliceFloat(trans.position.z);
        Send(protocol);
    }

    /// <summary>
    /// 发送玩家死亡消息，用于自杀
    /// </summary>
    public static void SendPlayerDead()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerDead");
        Send(protocol);
    }

    /// <summary>
    /// 发送玩家被毒圈伤害消息
    /// </summary>
    public static void SendPlayerPoison()
    {
        BytesProtocol protocol = new BytesProtocol();
        protocol.SpliceString("PlayerPoison");
        Send(protocol);
    }
}                                                                      
                                                                       
                                            