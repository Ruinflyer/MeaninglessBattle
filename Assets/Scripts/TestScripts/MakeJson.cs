using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class MakeJson : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        ItemsInfo itemsInfo = new ItemsInfo() { ItemInfoList = new List<SingleItemInfo>() };
        SingleItemInfo singleItemInfo = new SingleItemInfo()
        {
            itemType = ItemType.Weapon,
            ItemName = "棍棒",
            ResName = "Bat",
            OccurrenceProbability = 0.10f,
            weaponProperties = new WeaponProperties() { weaponType = WeaponType.Club, CDTime = 0f, Damage = 40 },
            armorProperties=null
        };
        itemsInfo.ItemInfoList.Add(singleItemInfo);
        singleItemInfo = new SingleItemInfo()
        {
            itemType = ItemType.Armor,
            ItemName = "角斗士头盔",
            ResName = "Helmet_Gladiator",
            OccurrenceProbability = 0.20f,
            weaponProperties = null,
            armorProperties = new ArmorProperties()
            {
                ForWeaponType = WeaponType.Club,
                Rate_Attack = 1.10f,
                Rate_DecreasedDurationTime = 0f,
                Rate_MagicalDefend = 0f,
                Rate_MoveSpeed=0f,
                Rate_PhysicalDefend=0f,
                Rate_Recovery=0f,
            }
        };
        itemsInfo.ItemInfoList.Add(singleItemInfo);
        MeaninglessJson.SavaJsonAsFile(MeaninglessJson.Path_StreamingAssets + "ItemsInfo.json", MeaninglessJson.ToJson(itemsInfo));

    }

   
}
