using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Meaningless;
using MeaninglessNetwork;

public class NetworkPlayer : MonoBehaviour {

    
    public PlayerController playerController;
    public AnimationManager animationManager;

    public string PlayerName="";
    
    public int headItemID;
    public int bodyItemID;
    public int weaponID;
    public int currentAction;
    public int curActionLayer;
    public CharacterStatus status;
    public float hp=100f;

    #region 记录上一次刷新的变量
    public long LastUpdateTime;
    private Vector3 lastPos=Vector3.zero;
    private Quaternion lastRot = Quaternion.identity;
    #endregion

    private AnimationState animState;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animationManager = GetComponent<AnimationManager>();
    }

    /// <summary>
    /// 设置玩家变换数据与更新时间
    /// </summary>
    public void SetPlayerTransform(float posX,float posY,float posZ,float rotX,float rotY,float rotZ,long CurUpdateTime)
    {
        //位置插值
        lastPos = transform.position;
        Vector3 curPos = new Vector3(posX, posY, posZ);
        transform.position = Vector3.Lerp(lastPos, curPos, (CurUpdateTime -LastUpdateTime));

        //角度插值
        lastRot = transform.rotation;
        Quaternion curRot = Quaternion.Euler(rotX,rotY,rotZ);
        transform.rotation = Quaternion.Lerp(lastRot,curRot, (CurUpdateTime - LastUpdateTime));

        //刷新更新时间
        LastUpdateTime = CurUpdateTime;
    }

    /// <summary>
    /// 设置玩家状态信息,生命值,头盔物品ID,当前动画名称等
    /// </summary>
    public void SetPlayerInfo(float HP,int HeadItemID,int BodyItemID,int WeaponID,int CurrentAction)
    {
        hp = HP;
        headItemID = HeadItemID;
        bodyItemID = BodyItemID;
        weaponID = WeaponID;
        currentAction = CurrentAction;
        animationManager.NetPlayClip();
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    public void SetPlayerName(string playerName)
    {
        PlayerName = playerName;
    }

    /// <summary>
    /// 获取自己的数据,作为协议
    /// </summary>
    public BytesProtocol GetPlayerInfo()
    {
        BytesProtocol protocol = new BytesProtocol();

        protocol.SpliceString("UpdatePlayerInfo");
        protocol.SpliceFloat(hp);
        protocol.SpliceFloat(transform.position.x);
        protocol.SpliceFloat(transform.position.y);
        protocol.SpliceFloat(transform.position.z);
        protocol.SpliceFloat(transform.rotation.x);
        protocol.SpliceFloat(transform.rotation.y);
        protocol.SpliceFloat(transform.rotation.z);

        if(BagManager.Instance.Dict_Equipped[EquippedItem.Head]==null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Dict_Equipped[EquippedItem.Head].ItemID);
        }

        if (BagManager.Instance.Dict_Equipped[EquippedItem.Body] == null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Dict_Equipped[EquippedItem.Body].ItemID);
        }
        if (BagManager.Instance.Dict_Equipped[EquippedItem.Weapon1] == null && BagManager.Instance.Dict_Equipped[EquippedItem.Weapon2] == null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Dict_Equipped[EquippedItem.Body].ItemID);
        }

       // protocol.SpliceInt(WeaponID);
       // protocol.SpliceString(CurrentAction);

        //playerFSM.characterStatus.HP;
        return protocol;
    }

}
