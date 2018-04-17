using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessServer;
public class NetworkManager : MonoBehaviour {

    public static Connect ServerConnection = new Connect();
	// Use this for initialization
	void Start () {
		
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
