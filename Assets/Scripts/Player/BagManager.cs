using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class BagManager : Mono_DDOLSingleton<BagManager>
{
    public int CurrentSelected = 1;
    public Dictionary<EquippedItem, SingleItemInfo> Dict_Equipped;
    public List<SingleItemInfo> List_PickUp = new List<SingleItemInfo>();
    public List<SingleItemInfo> List_Equip = new List<SingleItemInfo>();
    private int preSelected;
    private GameObject player;

    private struct BasicAttributes
    {
        public float rate_Defend_Physics;
        public float rate_Defend_Magic;
        public float rate_Attack_Physics;
        public float rate_Attack_Magic;
        public float rate_Recovery;
        public float rate_MoveSpeed;
        public float rate_DurationTime_Magic;
    }

    public struct SkillAttributes
    {
        public SingleItemInfo skillInfo;
        //CD计时
        public float Timer;
        //剩余次数
        public int remainCount;
        //CD是否到
        public bool isOn;
        //是否使用了技能
        public bool isUse;
    }



    private BasicAttributes armorAttributes;
    private BasicAttributes[] List_WeaponAttributes = new BasicAttributes[2];
    public SkillAttributes[] skillAttributesList = new SkillAttributes[2];

    //默认角色属性值
    private CharacterStatus defaultCharacterStatus;
    //角色属性值
    public CharacterStatus characterStatus;

    //开始补血的时间
    private float timeBeginheal;
    //补血flag
    private bool healFlag = false;
    //补血值
    private float RecoveryValue;
    //补血需要时间
    private float RecoveryTime;
    //每秒补充
    private float RecoveryPerSec;


    void Start()
    {
        player = CameraBase.Instance.player;

        Dict_Equipped = new Dictionary<EquippedItem, SingleItemInfo>();

        for (EquippedItem i = EquippedItem.Head; i <= EquippedItem.Magic2; i++)
        {
            Dict_Equipped.Add(i, null);
        }
        defaultCharacterStatus = MeaninglessJson.LoadJsonFromFile<CharacterStatus>(MeaninglessJson.Path_StreamingAssets + "CharacterStatus.json");
        characterStatus = defaultCharacterStatus;
        //MessageCenter.AddListener_Multparam(EMessageType.EquipItem, (object[] obj) => { EquipItem((EquippedItem)obj[0], (SingleItemInfo)obj[1]); });
        MessageCenter.AddListener(EMessageType.UseItem, (object obj) => { UseItem((int)obj); });
    }


    void Update()
    {
        if (Input.GetButtonDown("Bar1"))
        {
            CurrentSelected = 1;
            player.GetComponent<PlayerController>().ChangeWeapon(1);
        }
        if (Input.GetButtonDown("Bar2"))
        {
            CurrentSelected = 2;
            player.GetComponent<PlayerController>().ChangeWeapon(2);
        }
        if (Input.GetButtonDown("Bar3"))
        {
            CurrentSelected = 3;

        }
        if (Input.GetButtonDown("Bar4"))
        {
            CurrentSelected = 4;
        }

        if (Input.GetButtonDown("Defend"))
        {
            preSelected = CurrentSelected;
            CurrentSelected = 5;
        }

        if (Input.GetButtonUp("Defend"))
        {
            CurrentSelected = preSelected;
        }

        if (healFlag)
        {
            if (RecoveryValue > 0)
            {
                if (Time.time - timeBeginheal > 1.0f)
                {
                    MessageCenter.Send(EMessageType.Heal, RecoveryPerSec);
                    RecoveryTime -= 1.0f;
                    RecoveryValue -= RecoveryPerSec;
                    RecoveryPerSec = RecoveryValue / RecoveryTime;
                    timeBeginheal = Time.time;
                }
            }
            else
            {
                healFlag = false;
            }
        }

        if (Dict_Equipped[EquippedItem.Magic1] != null)
        {
            if (skillAttributesList[0].isOn && skillAttributesList[0].isUse && skillAttributesList[0].remainCount > 0)
            {
                skillAttributesList[0].isOn = false;
                skillAttributesList[0].remainCount -= 1;

            }

            if (skillAttributesList[0].Timer <= 0)
            {
                skillAttributesList[0].Timer = skillAttributesList[0].skillInfo.magicProperties.CDTime;
                skillAttributesList[0].isUse = false;
                skillAttributesList[0].isOn = true;
            }
            if (skillAttributesList[0].isUse && skillAttributesList[0].remainCount > 0)
                skillAttributesList[0].Timer -= Time.deltaTime;
        }
        if (Dict_Equipped[EquippedItem.Magic2] != null)
        {
            if (skillAttributesList[1].isOn && skillAttributesList[1].isUse && skillAttributesList[0].remainCount > 0)
            {
                skillAttributesList[1].isOn = false;
                skillAttributesList[1].remainCount -= 1;
            }

            if (skillAttributesList[1].Timer <= 0)
            {
                skillAttributesList[1].Timer = skillAttributesList[1].skillInfo.magicProperties.CDTime;
                skillAttributesList[1].isUse = false;
                skillAttributesList[1].isOn = true;
            }
            if (skillAttributesList[1].isUse && skillAttributesList[1].remainCount > 0)
                skillAttributesList[1].Timer -= Time.deltaTime;
        }

        MessageCenter.Send(EMessageType.CurrentHP, characterStatus.HP);
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="ItemID"></param>
    public void UseItem(int index)
    {
        if (List_PickUp[index] != null)
        {
            if (List_PickUp[index].itemType == ItemType.Expendable)
            {
                if (List_PickUp[index].expendableProperties.RechargeValue != 0)
                {
                    //MessageCenter.Send(EMessageType.Recharge, List_PickUp[index].expendableProperties.RechargeValue);
                    if(CurrentSelected==4)
                    {
                        skillAttributesList[1].remainCount += (int)List_PickUp[index].expendableProperties.RechargeValue;
                        if (skillAttributesList[1].remainCount > skillAttributesList[1].skillInfo.magicProperties.UsableCount)
                            skillAttributesList[1].remainCount = (int)skillAttributesList[1].skillInfo.magicProperties.UsableCount;
                    }
                    else
                    {
                        skillAttributesList[0].remainCount += (int)List_PickUp[index].expendableProperties.RechargeValue;
                        if (skillAttributesList[0].remainCount > skillAttributesList[0].skillInfo.magicProperties.UsableCount)
                            skillAttributesList[0].remainCount = (int)skillAttributesList[0].skillInfo.magicProperties.UsableCount;
                    }
                }

                if (List_PickUp[index].expendableProperties.Recovery_HP != 0)
                {
                    RecoveryValue = List_PickUp[index].expendableProperties.Recovery_HP;
                    RecoveryTime = List_PickUp[index].expendableProperties.Recovery_Time;
                    RecoveryPerSec = RecoveryValue / RecoveryTime;
                    timeBeginheal = Time.time;
                    healFlag = true;

                }

                List_PickUp.RemoveAt(index);
            }

            if(List_PickUp[index].itemType == ItemType.Gem)
            {
                List_PickUp.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 捡起物品
    /// </summary>
    /// <param name="ItemID">物品ID</param>
    public void PickItem(int ItemID)
    {
        if (ItemID != 0)
        {
            SingleItemInfo ItemInfo;
            ItemInfo = ItemInfoManager.Instance.GetItemInfo(ItemID);
            switch (ItemInfo.itemType)
            {
                case ItemType.Armor:
                case ItemType.Weapon:
                case ItemType.Magic:
                    if(List_Equip.Count<=10)
                    {
                        List_Equip.Add(ItemInfo);
                    }
                    break;
                case ItemType.Expendable:
                case ItemType.Gem:
                    if (List_PickUp.Count <= 10)
                    {
                        List_PickUp.Add(ItemInfo);
                    }
                    break;
            }
        }
    }

    public void UseMagic(int index)
    {
        skillAttributesList[index].isUse = true;
    }

    /// <summary>
    /// 装备物品
    /// </summary>
    /// <param name="equippedItem"></param>
    /// <param name="itemInfo"></param>
    public void EquipItem(EquippedItem equippedItem, SingleItemInfo itemInfo)
    {
        switch (equippedItem)
        {
            case EquippedItem.Head:
                //1.使用UnequipItem(EquippedItem.Head)后，直接脱下头盔将会减去头盔上宝石的属性，所以装备头盔时，当宝石存在，即再次加上宝石属性.
                //2.先装备了宝石再装备头盔，宝石属性仍未添加，所以装备头盔时，当宝石存在，即再次加上宝石属性.
                if (Dict_Equipped[EquippedItem.HeadGem1] != null)
                {
                    armorAttributes.rate_Attack_Physics += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_Recovery;
                }
                if (Dict_Equipped[EquippedItem.HeadGem2] != null)
                {
                    armorAttributes.rate_Attack_Physics += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_Recovery;
                }
                Dict_Equipped[(int)EquippedItem.Head] = itemInfo;
                break;
            case EquippedItem.Body:
                //1.使用UnequipItem(EquippedItem.Body)后，直接脱下身体防具将会减去身体防具上宝石的属性，所以装备身体防具时，当宝石存在，即再次加上宝石属性.
                //2.先装备了宝石再装备身体防具，宝石属性仍未添加，所以装备身体防具时，当宝石存在，即再次加上宝石属性.
                if (Dict_Equipped[EquippedItem.BodyGem1] != null)
                {
                    armorAttributes.rate_Attack_Physics += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_Recovery;
                }
                if (Dict_Equipped[EquippedItem.BodyGem2] != null)
                {
                    armorAttributes.rate_Attack_Physics += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_Recovery;
                }

                //添加身体防具属性
                armorAttributes.rate_Defend_Magic += itemInfo.armorProperties.Rate_MagicalDefend;
                armorAttributes.rate_Defend_Physics += itemInfo.armorProperties.Rate_PhysicalDefend;
                armorAttributes.rate_DurationTime_Magic += itemInfo.armorProperties.Rate_DecreasedDurationTime;
                armorAttributes.rate_MoveSpeed += itemInfo.armorProperties.Rate_MoveSpeed;
                armorAttributes.rate_Recovery += itemInfo.armorProperties.Rate_Recovery;

                Dict_Equipped[EquippedItem.Body] = itemInfo;
                break;

            case EquippedItem.HeadGem1:
                Dict_Equipped[EquippedItem.HeadGem1] = itemInfo;
                if (Dict_Equipped[EquippedItem.Head] != null)
                {
                    armorAttributes.rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.HeadGem2:
                Dict_Equipped[EquippedItem.HeadGem2] = itemInfo;
                if (Dict_Equipped[EquippedItem.Head] != null)
                {
                    armorAttributes.rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.BodyGem1:
                Dict_Equipped[EquippedItem.BodyGem1] = itemInfo;
                if (Dict_Equipped[EquippedItem.Body] != null)
                {
                    armorAttributes.rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.BodyGem2:
                Dict_Equipped[EquippedItem.BodyGem2] = itemInfo;
                if (Dict_Equipped[EquippedItem.Body] != null)
                {
                    armorAttributes.rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    armorAttributes.rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    armorAttributes.rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    armorAttributes.rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;


            case EquippedItem.Weapon1:
                //1.使用UnequipItem(EquippedItem.Weapon1)后，直接脱下武器将会减去武器上宝石的属性，所以装备武器时，当宝石存在，即再次加上宝石属性.
                //2.先装备了宝石再装备Weapon1，宝石属性仍未添加，所以装备武器时，当宝石存在，即再次加上宝石属性.
                if (Dict_Equipped[EquippedItem.Weapon1_Gem1] != null)
                {
                    List_WeaponAttributes[0].rate_Attack_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery += Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_Recovery;
                }
                if (Dict_Equipped[EquippedItem.Weapon1_Gem2] != null)
                {
                    List_WeaponAttributes[0].rate_Attack_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery += Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_Recovery;
                }


                Dict_Equipped[EquippedItem.Weapon1] = itemInfo;

                break;
            case EquippedItem.Weapon2:
                //1.使用UnequipItem(EquippedItem.Weapon2)后，直接脱下武器将会减去武器上宝石的属性，所以装备武器时，当宝石存在，即再次加上宝石属性.
                //2.先装备了宝石再装备Weapon2，宝石属性仍未添加，所以装备武器时，当宝石存在，即再次加上宝石属性.
                if (Dict_Equipped[EquippedItem.Weapon2_Gem1] != null)
                {
                    List_WeaponAttributes[1].rate_Attack_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery += Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_Recovery;
                }
                if (Dict_Equipped[EquippedItem.Weapon2_Gem2] != null)
                {
                    List_WeaponAttributes[1].rate_Attack_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery += Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_Recovery;
                }

                Dict_Equipped[EquippedItem.Weapon2] = itemInfo;
                break;

            case EquippedItem.Weapon1_Gem1:
                Dict_Equipped[EquippedItem.Weapon1_Gem1] = itemInfo;
                if (Dict_Equipped[EquippedItem.Weapon1] != null)
                {
                    List_WeaponAttributes[0].rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.Weapon1_Gem2:
                Dict_Equipped[EquippedItem.Weapon1_Gem2] = itemInfo;
                if (Dict_Equipped[EquippedItem.Weapon1] != null)
                {
                    List_WeaponAttributes[0].rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.Weapon2_Gem1:
                Dict_Equipped[EquippedItem.Weapon2_Gem1] = itemInfo;
                if (Dict_Equipped[EquippedItem.Weapon2] != null)
                {
                    List_WeaponAttributes[1].rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;
            case EquippedItem.Weapon2_Gem2:
                Dict_Equipped[EquippedItem.Weapon2_Gem2] = itemInfo;
                if (Dict_Equipped[EquippedItem.Weapon2] != null)
                {
                    List_WeaponAttributes[1].rate_Attack_Magic += itemInfo.gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics += itemInfo.gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic += itemInfo.gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics += itemInfo.gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic += itemInfo.gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed += itemInfo.gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery += itemInfo.gemProperties.Rate_Recovery;
                }
                break;

            case EquippedItem.Shield:

                Dict_Equipped[EquippedItem.Shield] = itemInfo;
                break;

            case EquippedItem.Magic1:
                Dict_Equipped[EquippedItem.Magic1] = itemInfo;
                skillAttributesList[0].skillInfo = itemInfo;
                skillAttributesList[0].isOn = true;
                skillAttributesList[0].isUse = false;
                skillAttributesList[0].Timer = itemInfo.magicProperties.CDTime;
                skillAttributesList[0].remainCount = (int)itemInfo.magicProperties.UsableCount;
                break;
            case EquippedItem.Magic2:
                Dict_Equipped[EquippedItem.Magic2] = itemInfo;
                skillAttributesList[1].skillInfo = itemInfo;
                skillAttributesList[1].isOn = true;
                skillAttributesList[1].isUse = false;
                skillAttributesList[1].Timer = itemInfo.magicProperties.CDTime;
                skillAttributesList[1].remainCount = (int)itemInfo.magicProperties.UsableCount;
                break;

            default:
                break;
        }

        Dict_Equipped[equippedItem] = itemInfo;


    }

    /// <summary>
    /// 脱下物品
    /// </summary>
    /// <param name="equippedItem"></param>
    public void UnequipItem(EquippedItem equippedItem)
    {
        if (Dict_Equipped[equippedItem] != null)
        {
            switch (equippedItem)
            {
                case EquippedItem.Head:
                    //当头盔脱下，头盔上的宝石将不再生效
                    if (Dict_Equipped[EquippedItem.HeadGem1] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_Recovery;
                    }
                    if (Dict_Equipped[EquippedItem.HeadGem2] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_Recovery;
                    }

                    Dict_Equipped[(int)EquippedItem.Head] = null;
                    break;
                case EquippedItem.HeadGem1:
                    if (Dict_Equipped[EquippedItem.Head] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.HeadGem1].gemProperties.Rate_Recovery;
                    }
                    Dict_Equipped[EquippedItem.HeadGem1] = null;
                    break;
                case EquippedItem.HeadGem2:
                    if (Dict_Equipped[EquippedItem.Head] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.HeadGem2].gemProperties.Rate_Recovery;
                    }
                    Dict_Equipped[EquippedItem.HeadGem2] = null;
                    break;
                case EquippedItem.Body:
                    //当身体防具脱下，身体防具上的宝石将不再生效
                    if (Dict_Equipped[EquippedItem.BodyGem1] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_Recovery;
                    }
                    if (Dict_Equipped[EquippedItem.BodyGem2] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_Recovery;
                    }

                    armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.Body].armorProperties.Rate_MagicalDefend;
                    armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.Body].armorProperties.Rate_PhysicalDefend;
                    armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.Body].armorProperties.Rate_DecreasedDurationTime;
                    armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.Body].armorProperties.Rate_MoveSpeed;
                    armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.Body].armorProperties.Rate_Recovery;

                    Dict_Equipped[EquippedItem.Body] = null;
                    break;
                case EquippedItem.BodyGem1:
                    if (Dict_Equipped[EquippedItem.Body] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.BodyGem1].gemProperties.Rate_Recovery;
                    }
                    Dict_Equipped[EquippedItem.BodyGem1] = null;
                    break;
                case EquippedItem.BodyGem2:
                    if (Dict_Equipped[EquippedItem.Body] != null)
                    {
                        armorAttributes.rate_Attack_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalAttack;
                        armorAttributes.rate_Attack_Physics -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalAttack;
                        armorAttributes.rate_Defend_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MagicalDefend;
                        armorAttributes.rate_Defend_Physics -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_PhysicalDefend;
                        armorAttributes.rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_DecreasedDurationTime;
                        armorAttributes.rate_MoveSpeed -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_MoveSpeed;
                        armorAttributes.rate_Recovery -= Dict_Equipped[EquippedItem.BodyGem2].gemProperties.Rate_Recovery;
                    }
                    Dict_Equipped[EquippedItem.BodyGem2] = null;
                    break;

                case EquippedItem.Shield:
                    Dict_Equipped[EquippedItem.Shield] = null;
                    break;
                case EquippedItem.Weapon1_Gem1:
                    List_WeaponAttributes[0].rate_Attack_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery -= Dict_Equipped[EquippedItem.Weapon1_Gem1].gemProperties.Rate_Recovery;
                    Dict_Equipped[EquippedItem.Weapon1_Gem1] = null;
                    break;
                case EquippedItem.Weapon1_Gem2:
                    List_WeaponAttributes[0].rate_Attack_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[0].rate_Attack_Physics -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[0].rate_Defend_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[0].rate_Defend_Physics -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[0].rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[0].rate_MoveSpeed -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[0].rate_Recovery -= Dict_Equipped[EquippedItem.Weapon1_Gem2].gemProperties.Rate_Recovery;
                    Dict_Equipped[EquippedItem.Weapon1_Gem2] = null;
                    break;
                case EquippedItem.Weapon2_Gem1:
                    List_WeaponAttributes[1].rate_Attack_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery -= Dict_Equipped[EquippedItem.Weapon2_Gem1].gemProperties.Rate_Recovery;
                    Dict_Equipped[EquippedItem.Weapon2_Gem1] = null;
                    break;

                case EquippedItem.Weapon2_Gem2:
                    List_WeaponAttributes[1].rate_Attack_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MagicalAttack;
                    List_WeaponAttributes[1].rate_Attack_Physics -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_PhysicalAttack;
                    List_WeaponAttributes[1].rate_Defend_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MagicalDefend;
                    List_WeaponAttributes[1].rate_Defend_Physics -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_PhysicalDefend;
                    List_WeaponAttributes[1].rate_DurationTime_Magic -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_DecreasedDurationTime;
                    List_WeaponAttributes[1].rate_MoveSpeed -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_MoveSpeed;
                    List_WeaponAttributes[1].rate_Recovery -= Dict_Equipped[EquippedItem.Weapon2_Gem2].gemProperties.Rate_Recovery;
                    Dict_Equipped[EquippedItem.Weapon2_Gem2] = null;
                    break;

                case EquippedItem.Weapon1:
                    Dict_Equipped[EquippedItem.Weapon1] = null;
                    break;
                case EquippedItem.Weapon2:
                    Dict_Equipped[EquippedItem.Weapon2] = null;
                    break;
                case EquippedItem.Magic1:
                    Dict_Equipped[EquippedItem.Magic1] = null;
                    break;
                case EquippedItem.Magic2:
                    Dict_Equipped[EquippedItem.Magic2] = null;
                    break;
            }

        }
    }

    /// <summary>
    /// 根据当前选择的武器返回角色数据，0无武器 1为武器1 2为武器2  3为魔法1 4为魔法2 5为盾牌
    /// </summary>
    /// <returns></returns>
    public CharacterStatus GetCharacterStatus()
    {
        switch (CurrentSelected)
        {
            case 0:
                GetNonWeaponCharacterStatus();
                break;
            case 1:

                //未装备Weapon1而使用Weapon1槽位攻击时，返回无武器属性
                if (Dict_Equipped[EquippedItem.Weapon1] == null)
                {
                    GetNonWeaponCharacterStatus();
                }
                else
                {
                    characterStatus.weaponType = Dict_Equipped[EquippedItem.Weapon1].weaponProperties.weaponType;
                    characterStatus.magicType = MagicType.NULL;

                    characterStatus.Attack_Physics = Dict_Equipped[EquippedItem.Weapon1].weaponProperties.Damage +
                        (Dict_Equipped[EquippedItem.Weapon1].weaponProperties.Damage * armorAttributes.rate_Attack_Physics) +
                        (Dict_Equipped[EquippedItem.Weapon1].weaponProperties.Damage * List_WeaponAttributes[0].rate_Attack_Physics);

                    characterStatus.Attack_Magic = defaultCharacterStatus.Attack_Magic + (defaultCharacterStatus.Attack_Magic * armorAttributes.rate_Attack_Magic) +
                        (List_WeaponAttributes[0].rate_Attack_Magic * defaultCharacterStatus.Attack_Magic);

                    characterStatus.DecreaseDurationTime_Magic = 1 - armorAttributes.rate_DurationTime_Magic - List_WeaponAttributes[0].rate_DurationTime_Magic;

                    characterStatus.Defend_Magic = armorAttributes.rate_Defend_Magic + List_WeaponAttributes[0].rate_Defend_Magic;
                    characterStatus.Defend_Physics = armorAttributes.rate_Defend_Physics + List_WeaponAttributes[0].rate_Defend_Physics;

                    characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed) +
                        (defaultCharacterStatus.moveSpeed * List_WeaponAttributes[0].rate_MoveSpeed);

                    characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + (defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery) +
                        (defaultCharacterStatus.RecoveryValue * List_WeaponAttributes[0].rate_Recovery);


                    if (Dict_Equipped[(int)EquippedItem.Head] != null)
                    {
                        //头盔指定的武器和现在选择的武器一样，攻击+x%
                        if (Dict_Equipped[EquippedItem.Weapon1].weaponProperties.weaponType == Dict_Equipped[(int)EquippedItem.Head].armorProperties.ForWeaponType)
                        {
                            characterStatus.Attack_Physics += (Dict_Equipped[EquippedItem.Weapon1].weaponProperties.Damage * Dict_Equipped[(int)EquippedItem.Head].armorProperties.Rate_Attack);
                        }
                    }

                }
                break;
            case 2:
                //未装备Weapon2而使用Weapon2槽位攻击时，返回无武器属性
                if (Dict_Equipped[EquippedItem.Weapon2] == null)
                {
                    GetNonWeaponCharacterStatus();
                }
                else
                {
                    characterStatus.weaponType = Dict_Equipped[EquippedItem.Weapon2].weaponProperties.weaponType;
                    characterStatus.magicType = MagicType.NULL;

                    characterStatus.Attack_Physics = Dict_Equipped[EquippedItem.Weapon2].weaponProperties.Damage +
                        (Dict_Equipped[EquippedItem.Weapon2].weaponProperties.Damage * armorAttributes.rate_Attack_Physics) +
                        (Dict_Equipped[EquippedItem.Weapon2].weaponProperties.Damage * List_WeaponAttributes[1].rate_Attack_Physics);

                    characterStatus.Attack_Magic = defaultCharacterStatus.Attack_Magic + (defaultCharacterStatus.Attack_Magic * armorAttributes.rate_Attack_Magic) +
                        (List_WeaponAttributes[1].rate_Attack_Magic * defaultCharacterStatus.Attack_Magic);

                    characterStatus.DecreaseDurationTime_Magic = 1 - armorAttributes.rate_DurationTime_Magic - List_WeaponAttributes[1].rate_DurationTime_Magic;

                    characterStatus.Defend_Magic = armorAttributes.rate_Defend_Magic + List_WeaponAttributes[1].rate_Defend_Magic;
                    characterStatus.Defend_Physics = armorAttributes.rate_Defend_Physics + List_WeaponAttributes[1].rate_Defend_Physics;

                    characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed) +
                        (defaultCharacterStatus.moveSpeed * List_WeaponAttributes[1].rate_MoveSpeed);

                    characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + (defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery) +
                        (defaultCharacterStatus.RecoveryValue * List_WeaponAttributes[1].rate_Recovery);

                    if (Dict_Equipped[(int)EquippedItem.Head] != null)
                    {
                        //头盔指定的武器和现在选择的武器一样，攻击+x%
                        if (Dict_Equipped[EquippedItem.Weapon2].weaponProperties.weaponType == Dict_Equipped[(int)EquippedItem.Head].armorProperties.ForWeaponType)
                        {
                            characterStatus.Attack_Physics += (Dict_Equipped[EquippedItem.Weapon2].weaponProperties.Damage * Dict_Equipped[(int)EquippedItem.Head].armorProperties.Rate_Attack);
                        }
                    }

                }

                break;

            case 3:
                characterStatus.weaponType = Dict_Equipped[EquippedItem.Magic1].weaponProperties.weaponType;
                characterStatus.magicType = Dict_Equipped[EquippedItem.Magic1].magicProperties.magicType;

                characterStatus.Attack_Physics = 0;

                characterStatus.Attack_Magic = Dict_Equipped[EquippedItem.Magic1].magicProperties.Damage +
                    (Dict_Equipped[EquippedItem.Magic1].magicProperties.Damage * armorAttributes.rate_Attack_Magic);


                characterStatus.DecreaseDurationTime_Magic = 1 - armorAttributes.rate_DurationTime_Magic;

                characterStatus.Defend_Magic = armorAttributes.rate_Defend_Magic;

                characterStatus.Defend_Physics = armorAttributes.rate_Defend_Physics;

                characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed);

                characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + (defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery);
                break;
            case 4:
                characterStatus.weaponType = Dict_Equipped[EquippedItem.Magic2].weaponProperties.weaponType;
                characterStatus.magicType = Dict_Equipped[EquippedItem.Magic2].magicProperties.magicType;

                characterStatus.Attack_Physics = 0;

                characterStatus.Attack_Magic = Dict_Equipped[EquippedItem.Magic2].magicProperties.Damage +
                    (Dict_Equipped[EquippedItem.Magic2].magicProperties.Damage * armorAttributes.rate_Attack_Magic);


                characterStatus.DecreaseDurationTime_Magic = 1 - armorAttributes.rate_DurationTime_Magic;

                characterStatus.Defend_Magic = armorAttributes.rate_Defend_Magic;

                characterStatus.Defend_Physics = armorAttributes.rate_Defend_Physics;

                characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed);

                characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + (defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery);
                break;
            case 5:
                //一次防御盾前抵挡所有控制一次，一次后破防，一次防御盾前抵挡200伤害，溢出则破防
                characterStatus.weaponType = WeaponType.Shield;
                characterStatus.Attack_Physics = 0;
                characterStatus.Attack_Magic = 0;
                characterStatus.DecreaseDurationTime_Magic = 0;
                characterStatus.Defend_Magic = 200;
                characterStatus.Defend_Physics = 200;
                characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed);
                characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery;

                break;
        }
        return characterStatus;
    }

    /// <summary>
    /// 返回一个无武器状态的角色数据
    /// </summary>
    /// <returns>无武器状态的角色数据</returns>
    public CharacterStatus GetNonWeaponCharacterStatus()
    {
        characterStatus.weaponType = WeaponType.NULL;

        characterStatus.Attack_Physics = defaultCharacterStatus.Attack_Physics + (defaultCharacterStatus.Attack_Physics * armorAttributes.rate_Attack_Physics);

        characterStatus.Attack_Magic = 0;

        characterStatus.DecreaseDurationTime_Magic = 1 - armorAttributes.rate_DurationTime_Magic;

        characterStatus.Defend_Magic = defaultCharacterStatus.Defend_Magic + armorAttributes.rate_Defend_Magic;

        characterStatus.Defend_Physics = defaultCharacterStatus.Defend_Physics + armorAttributes.rate_Defend_Physics;

        characterStatus.moveSpeed = defaultCharacterStatus.moveSpeed + (defaultCharacterStatus.moveSpeed * armorAttributes.rate_MoveSpeed);

        characterStatus.RecoveryValue = defaultCharacterStatus.RecoveryValue + (defaultCharacterStatus.RecoveryValue * armorAttributes.rate_Recovery);
        return characterStatus;
    }
}
