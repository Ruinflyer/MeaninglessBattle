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
    /// 地图加载完毕时上报服务端统计
    /// </summary>
    public static void SendMapLoaded()
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("MapLoaded");
        ServerConnection.Send(p);
    }

}
