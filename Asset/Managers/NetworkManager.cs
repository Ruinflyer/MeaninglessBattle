using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;
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
}
