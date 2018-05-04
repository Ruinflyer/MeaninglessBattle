using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessNetwork;
using Meaningless;
public class DeadArea : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkManager.SendPlayerDead();

        //玩家死亡处理
        UIManager.Instance.ShowUI(UIid.FinishUI);
        CameraBase.Instance.isFollowing = false;
        object[] param = new object[3];
        param[0] = "珍惜生命，远离悬崖";
        param[1] = "你死于跳崖自杀";
        param[2] = " ";
        MessageCenter.Send_Multparam(EMessageType.FinishUI, param);
    }
}
