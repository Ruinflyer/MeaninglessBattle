using System.Collections;
using System.Collections.Generic;
using Meaningless;
using System;

//角色属性可序列化类
[Serializable]
public class CharacterStatus
{
    //角色名
    public string characterName;
    //当前武器类型
    public WeaponType weaponType;
    //当前魔法类型
    public MagicType magicType;
    //移动速度
    public float moveSpeed;
    //无视
    public float jumpSpeed;
    //生命值
    public float HP;
    //物理攻击值
    public float Attack_Physics;
    //魔法攻击值
    public float Attack_Magic;
    //每秒恢复值
    public float RecoveryValue;
    //物理防御值
    public float Defend_Physics;
    //魔法防御值
    public float Defend_Magic;
    //魔法持续时间减少值
    public float DecreaseDurationTime_Magic;
    

}

