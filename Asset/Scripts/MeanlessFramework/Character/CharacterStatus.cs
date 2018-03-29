using System.Collections;
using System.Collections.Generic;
using Meaningless;
using System;

[Serializable]
public class CharacterStatus
{
    public string characterName;
    public WeaponType weaponType;
    public MagicType magicType;
    

    public float moveSpeed;
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
    //针对武器的属性值
    public Dictionary<WeaponType, float> AppointedWeaponData;
}

