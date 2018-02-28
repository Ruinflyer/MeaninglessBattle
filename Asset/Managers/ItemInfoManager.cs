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
    Expendable
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


public class ItemInfoManager : Singleton<ItemInfoManager>
{

    private ItemsInfo _itemsInfo = null;

    /// <summary>
    /// TKey:   物品ID称
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
    /// 通过物品ID获得武器攻击速度
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    /// <returns>返回武器攻击速度</returns>
    public float GetWeaponAtkSpeed(int ItemID)
    {
        if (Dict_ItemInfo != null)
        {
            return Dict_ItemInfo[ItemID].weaponProperties.Rate_AtkSpeed;
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
}