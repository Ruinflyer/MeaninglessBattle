using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Meaningless
{
    //窗体的ID
    public enum UIid
    {
        NullUI = 0
		,MainUI
		,LoadingUI
		,HUDUI
		,BagUI
		,LoginUI
		,RoomUI
		,TipsUI
		,Canvas
		,FinishUI
        ,EscapeUI
		/*UIIDHere*/

    }
    public enum EMessageType
    {
        NullType,
        //加入的玩家数量
        JoinedPlayers,
        //拾起的物品ID
        PickedupItem,
        //当前选择的武器
        CurrentselectedWeapon,
        //发送当前生命值
        CurrentHP,
        //发送一个int型血量补充值补血
        Heal,
        //发送一个int型充能值
        Recharge,
        //发现物品
        FoundItem,
        //使用物品
        UseItem,
        //获取背包列表并刷新显示
        GetAndSetBagList,
        //获取装备列表并刷新显示
        GetAndSetEquippedList,
        //获取背包列表是否已满
        GetBagListFull,
        //刷新UI中的背包列表
        RefreshBagList,
        //获取摄像机射线终点
        GetHitPoint,
        //装备物品
        EquipItem,
        //脱下物品
        UnEquipItem,
        /***************/
        TipsUI,
        PlayerName,
        LoadingUI,
        LoadingScene,
        Remain,
        FinishUI,//结算界面
        CountdownTime,//倒计时
        //
    }

    public enum BuffType
    {
        Freeze,
        SlowDown,
        Blind,
    }

    public enum FSMStateType
    {
        Idle = 0,
        Move,
        Jump,
        UseItem,
        PickUp,
        Fall,
        SingleWieldAttack,
        DoubleHandsAttack,
        SpearAttack,
        RippleAttack,
        HeartAttack,
        StygianDesolator,
        IceArrow,
        ChoshimArrow,
        ThunderBolt,
        Defend,
        TakeDamage,
    }

    public enum FSMTransitionType
    {
        IsIdle = 0,
        CanBeMove,
        CanBeJump,
        CanPickUp,
        CanUseItem,
        Falling,
        AttackWithSingleWield,
        AttackWithDoubleHands,
        AttackWithSpear,
        UsingRipple,
        UsingHeartAttack,
        UsingStygianDesolator,
        UsingIceArrow,
        UsingChoshimArrow,
        UsingThunderBolt,
        CanDefend,
        Hurt,
    }
    public class GameDefine : MonoBehaviour
    {
        public static Dictionary<UIid, string> dicPath = new Dictionary<UIid, string>()
        {
			{UIid.MainUI,"UIPrefab/MainUI"},
			{UIid.LoadingUI,"UIPrefab/LoadingUI"},
			{UIid.HUDUI,"UIPrefab/HUDUI"},
			{UIid.BagUI,"UIPrefab/BagUI"},
			{UIid.LoginUI,"UIPrefab/LoginUI"},
			{UIid.RoomUI,"UIPrefab/RoomUI"},
			{UIid.TipsUI,"UIPrefab/TipsUI"},
			{UIid.Canvas,"UIPrefab/Canvas"},
			{UIid.FinishUI,"UIPrefab/FinishUI"},
            {UIid.EscapeUI,"UIPrefab/EscapeUI" },
			/*UIDictHere*/
                       

        };
        public static Type GetUIScriptType(UIid uiId)
        {
            Type scriptType = null;
            switch (uiId)
            {
				case UIid.MainUI:
					 scriptType=typeof(MainUI);
					break;
				case UIid.LoadingUI:
					 scriptType=typeof(LoadingUI);
					break;
				case UIid.HUDUI:
					 scriptType=typeof(HUDUI);
					break;
				case UIid.BagUI:
					 scriptType=typeof(BagUI);
					break;
				case UIid.LoginUI:
					 scriptType=typeof(LoginUI);
					break;
				case UIid.RoomUI:
					 scriptType=typeof(RoomUI);
					break;
				case UIid.TipsUI:
					 scriptType=typeof(TipsUI);
					break;
				case UIid.Canvas:
					 scriptType=typeof(Canvas);
					break;
				case UIid.FinishUI:
					 scriptType=typeof(FinishUI);
					break;
                case UIid.EscapeUI:
                    scriptType = typeof(EscapeUI);
                    break;
                /*SwitchUIScriptType*/
                case UIid.NullUI:
                    Debug.LogError("传入的窗体ID为NullUI");
                    break;
                default:
                    Debug.LogError("没有添加对应的case条件，找不到对应的UI脚本");
                    break;
            }
            return scriptType;
        }
    }


}
