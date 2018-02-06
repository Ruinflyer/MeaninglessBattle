using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    Weapon=0,
    Armor,
    Expendable
}
/// <summary>
/// 武器类型
/// </summary>
public enum WeaponType
{
   NULL=0,
   Club,
   Sword,
   DoubleHands,
   Spear,
   Shield
}

/// <summary>
/// 物品信息集合可序列化类
/// </summary>
public class ItemsInfo
{
    public List<SingleItemInfo> ItemInfoList;
}

/// <summary>
/// 单个物品信息可序列化类
/// </summary>
[System.Serializable]
public class SingleItemInfo
{
    //物品名称
    public string ItemName;
    //资源名
    public string ResName;
    //出现概率
    public float OccurrenceProbability;
    public ItemType itemType;
    //防具属性
    public ArmorProperties armorProperties;
    //武器属性
    public WeaponProperties weaponProperties;
}

/// <summary>
/// 防具属性可序列化类
/// </summary>
[System.Serializable]
public class ArmorProperties
{
    //对哪种武器类别作用
    public WeaponType ForWeaponType;
    //攻击提高率
    public float Rate_Attack;
    //物理防御提高率
    public float Rate_PhysicalDefend;
    //魔法防御提高率
    public float Rate_MagicalDefend;
    //移动速度提高率
    public float Rate_MoveSpeed;
    //生命恢复速率
    public float Rate_Recovery;
    //控制类技能持续时间降低率
    public float Rate_DecreasedDurationTime;
    //防具等级
    public int Level;
}

/// <summary>
/// 武器属性可序列化类
/// </summary>
[System.Serializable]
public class WeaponProperties
{
    //武器类别
    public WeaponType weaponType;
    //武器伤害值
    public float Damage;
    //攻击速率(动画时间加速比)
    public float Rate_AtkSpeed;
    //武器冷却时间(盾牌)
    public float CDTime;
}


public class ItemInfoManager : Singleton<ItemInfoManager> {

    private ItemsInfo _itemsInfo=null;

    /// <summary>
    /// TKey:   资源名称
    /// TValue：SingleItemInfo 单个物品信息可序列化类
    /// </summary>
    private Dictionary<string, SingleItemInfo> Dict_ItemInfo;
	public void LoadInfo()
    {
        _itemsInfo=MeaninglessJson.LoadJsonFromFile<ItemsInfo>(MeaninglessJson.Path_StreamingAssets+"ItemsInfo.json");
        foreach(SingleItemInfo s_iteminfo in _itemsInfo.ItemInfoList)
        {
            Dict_ItemInfo.Add(s_iteminfo.ResName,s_iteminfo);
        }
    }

    /// <summary>
    /// 通过资源名获得物品名称
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回物品名</returns>
    public string GetItemName(string ResName)
    {
        if(Dict_ItemInfo!=null)
        {
            return Dict_ItemInfo[ResName].ItemName;
        }
        return null;
    }
    /// <summary>
    /// 通过资源名获得出现概率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回出现概率</returns>
    public float GetOccurrenceProbability(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].OccurrenceProbability;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得物品类型
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回物品类型，默认返回消耗品类型</returns>
    public ItemType GetItemType(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].itemType;
        }
        return ItemType.Expendable;
    }
    /// <summary>
    /// 通过资源名获得武器属性
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回武器属性</returns>
    public WeaponProperties GetWeaponProperties(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].weaponProperties;
        }
        return null;
    }
    /// <summary>
    /// 通过资源名获得防具属性
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具属性，默认返回空</returns>
    public ArmorProperties GetArmorProperties(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties;
        }
        return null;
    }

    /// <summary>
    /// 通过资源名获得武器攻击力
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回武器攻击力</returns>
    public float GetWeaponDamage(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].weaponProperties.Damage;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得武器攻击速度
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回武器攻击速度</returns>
    public float GetWeaponAtkSpeed(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].weaponProperties.Rate_AtkSpeed;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得武器冷却时间(通常为盾牌所使用)
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回武器冷却时间</returns>
    public float GetWeaponCDTime(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].weaponProperties.CDTime;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得武器类型
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回武器类型</returns>
    public WeaponType GetWeaponWeaponType(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].weaponProperties.weaponType;
        }
        return WeaponType.NULL;
    }

    /// <summary>
    /// 通过资源名获得防具对哪种武器类别作用
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具对哪种武器类别作用</returns>
    public WeaponType GetArmor_ForWeaponType(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.ForWeaponType;
        }
        return WeaponType.NULL;
    }
    /// <summary>
    /// 通过资源名获得防具攻击提高率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具攻击提高率</returns>
    public float GetArmor_RateAttack(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_Attack;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得防具物理防御提高率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具物理防御提高率</returns>
    public float GetArmor_RatePhysicalDefend(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_PhysicalDefend;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得防具魔法防御提高率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具魔法防御提高率</returns>
    public float GetArmor_RateMagicalDefend(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_MagicalDefend;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得防具移动速度提高率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具移动速度提高率</returns>
    public float GetArmor_RateMoveSpeed(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_MoveSpeed;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得防具生命恢复速率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具生命恢复速率</returns>
    public float GetArmor_RateRecovery(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_Recovery;
        }
        return 0f;
    }
    /// <summary>
    /// 通过资源名获得防具控制类技能持续时间降低率
    /// </summary>
    /// <param name="ResName">资源名称</param>
    /// <returns>返回防具控制类技能持续时间降低率</returns>
    public float GetArmor_RateDecreasedDurationTime(string ResName)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ResName].armorProperties.Rate_DecreasedDurationTime;
        }
        return 0f;
    }
}