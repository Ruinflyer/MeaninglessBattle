﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Meaningless;

public class NetworkPlayer : MonoBehaviour {


    public PlayerFSM playerFSM;
    public PlayerController playerController;
    public AnimationManager animationManager;

    public string PlayerName="";
    private int headItemID;
    private int bodyItemID;
    private int weaponID;
    private string currentAction;

    #region 纪录上一次刷新的变量
    public long LastUpdateTime;
    private Vector3 lastPos=Vector3.zero;
    private Quaternion lastRot = Quaternion.identity;
    #endregion

    private AnimationState animState;

    private void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
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
    public void SetPlayerInfo(float HP,int HeadItemID,int BodyItemID,int WeaponID,string CurrentAction)
    {
        playerFSM.characterStatus.HP = HP;
        headItemID = HeadItemID;
        bodyItemID = BodyItemID;
        weaponID = WeaponID;
        currentAction = CurrentAction;
        SetPlayerAnimation(currentAction);
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    public void SetPlayerName(string playerName)
    {
        PlayerName = playerName;
        playerFSM.characterStatus.characterName = playerName;
    }

    /// <summary>
    /// 设置玩家播放指定动画
    /// </summary>
    /// <param name="animationName"></param>
    public void SetPlayerAnimation(string animationName)
    {
        MethodInfo methodInfo= animationManager.GetType().GetMethod("Play"+animationName);
        if(methodInfo==null)
        {
            Debug.LogError("动画: "+animationName+" 不存在! 请去AnimationManager类里添加 Play"+animationName+"() 方法");
            return;
        }
        object[] param = new object[] {animationName };
        methodInfo.Invoke(animationManager,param);
    }


}