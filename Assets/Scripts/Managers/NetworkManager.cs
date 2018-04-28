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


}
