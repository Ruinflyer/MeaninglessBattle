using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    Weapon = 0,
    Armor,
    Expendable,
    Magic,
    Gem,
}
/// <summary>
/// 武器类型
/// </summary>
public enum WeaponType
{
    NULL = 0,
    Club,
    Sword,
    DoubleHands,
    Spear,
    Shield,
}

/// <summary>
/// 防具类型
/// </summary>
public enum ArmorType
{
    NULL=0,
    Head,
    Body
}


/// <summary>
/// 魔法类型
/// </summary>
public enum MagicType
{
    NULL = 0,
    Ripple,
    HeartAttack,
    StygianDesolator,
    IceArrow,
    ChoshimArrow,
    KillerQueen,
    Thunderbolt,
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
    //物品ID
    public int ItemID;
    //物品名称
    public string ItemName;
    //物品ID
    public string ResName;
    //出现概率
    public float OccurrenceProbability;
    public ItemType itemType;
    //防具属性
    public ArmorProperties armorProperties;
    //武器属性
    public WeaponProperties weaponProperties;
    //魔法属性
    public MagicProperties magicProperties;
    //宝石属性
    public GemProperties gemProperties;
    //消耗品属性
    public ExpendableProperties expendableProperties;
}

/// <summary>
/// 防具属性可序列化类
/// </summary>
[System.Serializable]
public class ArmorProperties
{
    //防具类型
    public ArmorType armorType;
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
    public float weaponLength;
    //武器冷却时间(盾牌)
    public float CDTime;
}

/// <summary>
/// 魔法属性可序列化类
/// </summary>
[System.Serializable]
public class MagicProperties
{
    //技能类别
    public MagicType magicType;
    //伤害值
    public float Damage;
    //控制概率
    public float Probability;
    //使用上限
    public float UsableCount;
    //冷却时间
    public float CDTime;
}

/// <summary>
/// 宝石属性可序列化类
/// </summary>
[System.Serializable]
public class GemProperties
{
  
    //物理攻击提高率
    public float Rate_PhysicalAttack;
    //魔法攻击提高率
    public float Rate_MagicalAttack;
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
}

/// <summary>
/// 消耗品属性可序列化类
/// </summary>
[System.Serializable]
public class ExpendableProperties
{
    //生命恢复值
    public float Recovery_HP;
    //恢复时间
    public float Recovery_Time;
    //能量补充值
    public float RechargeValue;

}
public class ItemInfoManager : Singleton<ItemInfoManager>
{

    private ItemsInfo _itemsInfo = null;

    /// <summary>
    /// TKey:   物品ID
    /// TValue：SingleItemInfo 单个物品信息可序列化类
    /// </summary>
    private Dictionary<int, SingleItemInfo> Dict_ItemInfo;
    public bool LoadInfo()
    {
        Dict_ItemInfo = new Dictionary<int, SingleItemInfo>();
        _itemsInfo = MeaninglessJson.LoadJsonFromFile<ItemsInfo>(MeaninglessJson.Path_StreamingAssets + "ItemsInfo.json");
        if(_itemsInfo!=null)
        {
            foreach (SingleItemInfo s_iteminfo in _itemsInfo.ItemInfoList)
            {
                Dict_ItemInfo.Add(s_iteminfo.ItemID, s_iteminfo);
            }
        }
        if(Dict_ItemInfo.Count==_itemsInfo.ItemInfoList.Count)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 通过物品ID获得物品资源名
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>物品资源名</returns>
    public string GetResname(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].ResName;
        }
        return null;
    }

    /// <summary>
    /// 通过物品ID获得物品名称
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回物品名</returns>
    public string GetItemName(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].ItemName;
        }
        return null;
    }
    /// <summary>
    /// 通过物品ID获得出现概率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回出现概率</returns>
    public float GetOccurrenceProbability(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].OccurrenceProbability;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得物品类型
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回物品类型，默认返回消耗品类型</returns>
    public ItemType GetItemType(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].itemType;
        }
        return ItemType.Expendable;
    }
    /// <summary>
    /// 通过物品ID获得武器属性
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回武器属性</returns>
    public WeaponProperties GetWeaponProperties(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].weaponProperties;
        }
        return null;
    }
    /// <summary>
    /// 通过物品ID获得防具属性
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具属性，默认返回空</returns>
    public ArmorProperties GetArmorProperties(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties;
        }
        return null;
    }

    /// <summary>
    /// 通过物品ID获得武器攻击力
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回武器攻击力</returns>
    public float GetWeaponDamage(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].weaponProperties.Damage;
        }
        return 0f;
    }
   
    /// <summary>
    /// 通过物品ID获得武器冷却时间(通常为盾牌所使用)
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回武器冷却时间</returns>
    public float GetWeaponCDTime(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].weaponProperties.CDTime;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得武器类型
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回武器类型</returns>
    public WeaponType GetWeaponWeaponType(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].weaponProperties.weaponType;
        }
        return WeaponType.NULL;
    }

    /// <summary>
    /// 通过物品ID获得防具对哪种武器类别作用
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具对哪种武器类别作用</returns>
    public WeaponType GetArmor_ForWeaponType(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.ForWeaponType;
        }
        return WeaponType.NULL;
    }
    /// <summary>
    /// 通过物品ID获得防具攻击提高率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具攻击提高率</returns>
    public float GetArmor_RateAttack(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_Attack;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得防具物理防御提高率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具物理防御提高率</returns>
    public float GetArmor_RatePhysicalDefend(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_PhysicalDefend;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得防具魔法防御提高率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具魔法防御提高率</returns>
    public float GetArmor_RateMagicalDefend(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_MagicalDefend;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得防具移动速度提高率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具移动速度提高率</returns>
    public float GetArmor_RateMoveSpeed(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_MoveSpeed;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得防具生命恢复速率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具生命恢复速率</returns>
    public float GetArmor_RateRecovery(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_Recovery;
        }
        return 0f;
    }
    /// <summary>
    /// 通过物品ID获得防具控制类技能持续时间降低率
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回防具控制类技能持续时间降低率</returns>
    public float GetArmor_RateDecreasedDurationTime(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].armorProperties.Rate_DecreasedDurationTime;
        }
        return 0f;
    }

    /// <summary>
    /// 返回一个获得所有物品的获得概率的数组
    /// </summary>
    /// <returns>所有物品获得概率的数组</returns>
    public float[] GetTotalOccurrenceProbability()
    {
        if (Dict_ItemInfo != null)
        {
            float[] tmp_float=new float[_itemsInfo.ItemInfoList.Count];
            for (int i = 0; i < _itemsInfo.ItemInfoList.Count; i++)
            {
                tmp_float[i] = _itemsInfo.ItemInfoList[i].OccurrenceProbability;
            }
            return tmp_float;
        }
        return null;
    }

    /// <summary>
    /// 返回一个获得所有物品物品ID的数组
    /// </summary>
    /// <returns>所有物品物品ID的数组</returns>
    public int[] GetAllItemsID()
    {
        if (Dict_ItemInfo != null)
        {
            int[] tmp_int = new int[_itemsInfo.ItemInfoList.Count];
            for (int i = 0; i < _itemsInfo.ItemInfoList.Count; i++)
            {
                tmp_int[i] = _itemsInfo.ItemInfoList[i].ItemID;
            }
            return tmp_int;
        }
        return null;
    }

    /// <summary>
    /// 通过物品ID获取单个物品信息
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>单个物品信息</returns>
    public SingleItemInfo GetItemInfo(int ItemID)
    {
        if(Dict_ItemInfo!=null)
        {
            SingleItemInfo itemInfo;
            if(Dict_ItemInfo.TryGetValue(ItemID,out itemInfo))
            {
                return itemInfo;
            }
        }
        else
        {
            LoadInfo();
            SingleItemInfo itemInfo;
            if (Dict_ItemInfo.TryGetValue(ItemID, out itemInfo))
            {
                return itemInfo;
            }
        }
        return null;
    }
}