using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessNetwork;

public class DropPoint : MonoBehaviour {
    //是否为调试用
    public bool DebugMode = false;
    public Transform[] Droppoint;

    private void Start()
    {
        NetworkManager.AddOnceEventListener("Droppoint", OnDroppointBack);

    }

    /// <summary>
    /// 下降点回调
    /// </summary>
    public void OnDroppointBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int index = p.GetInt(startIndex, ref startIndex);
        if(DebugMode==false)
        {
            CameraBase.Instance.gameObject.transform.position = Droppoint[index].position;
            CameraBase.Instance.player.transform.position = Droppoint[index].position;
        }
        
    }
}
