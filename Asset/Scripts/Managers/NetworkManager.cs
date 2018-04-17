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
}
