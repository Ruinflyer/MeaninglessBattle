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
    public string currentAction;
    public int curActionLayer;
    public CharacterStatus status;
    public float hp=100f;

    #region 记录上一次刷新的变量
    public float LastUpdateTime=0;
    private Vector3 lastPos=Vector3.zero;
    private Vector3 lastRot = Vector3.zero;
    //预测坐标
    private Vector3 forseePos= Vector3.zero;
    //预测旋转度
    private Vector3 forseeRot = Vector3.zero;
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
    public void SetPlayerTransform(float posX,float posY,float posZ,float rotX,float rotY,float rotZ)
    {
        float DeltaTime = Time.time - LastUpdateTime;

        Vector3 recvPos = new Vector3(posX,posY,posZ);
        Vector3 recvRot = new Vector3(rotX, rotY, rotZ);
        
        forseePos = lastPos + (recvPos - lastPos) * 2;
        forseeRot = lastRot + (recvRot - lastRot) * 2;
        if (Time.time -LastUpdateTime>0.3f)
        {
            forseePos = recvPos;
            forseeRot = recvRot;
        }

        if(DeltaTime>0)
        {
            //位置插值
            Vector3 curPos = transform.position;
            transform.position = Vector3.Lerp(curPos, recvPos, DeltaTime);

            //角度插值
            Vector3 curRot = transform.eulerAngles;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(curRot), Quaternion.Euler(recvRot), DeltaTime);
        }
       

        //刷新更新时间
        LastUpdateTime = Time.time;
    }

    /// <summary>
    /// 设置玩家状态信息,生命值,头盔物品ID,当前动画名称等
    /// </summary>
    public void SetPlayerInfo(float HP,int HeadItemID,int BodyItemID,int WeaponID,int Layer,string CurrentAction)
    {
        hp = HP;
        headItemID = HeadItemID;
        bodyItemID = BodyItemID;
        weaponID = WeaponID;
        currentAction = CurrentAction;
        curActionLayer = Layer;
        animationManager.NetPlayClip(Layer, CurrentAction);
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

        if(BagManager.Instance.Head==null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Head.ItemID);
        }

        if (BagManager.Instance.Body == null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Body.ItemID);
        }
        if (BagManager.Instance.Weapon1 == null && BagManager.Instance.Weapon2 == null)
        {
            protocol.SpliceInt(0);
        }
        else
        {
            protocol.SpliceInt(BagManager.Instance.Body .ItemID);
        }

        // protocol.SpliceInt(WeaponID);
        // protocol.SpliceString(CurrentAction);

        //playerFSM.characterStatus.HP;
        status = BagManager.Instance.characterStatus;

        return protocol;
    }

}
