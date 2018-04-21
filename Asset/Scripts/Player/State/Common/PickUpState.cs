using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PickUpState : FSMState
{
    //private int pickedupItemID = -1;
    Transform pickUpItemTran = null;
    int pickedupItemID = -1;

    public PickUpState()
    {
        stateID = FSMStateType.PickUp;

    }

    public override void Act(BaseFSM FSM)
    {
        pickUpItemTran = Camera.main.GetComponent<CameraCollision>().itemTran;

        if (pickUpItemTran != null)
            if (pickUpItemTran.GetComponent<GroundItem>() != null)
            {
                pickedupItemID = pickUpItemTran.GetComponent<GroundItem>().ItemID;
                SingleItemInfo ItemInfo;
                ItemInfo = ItemInfoManager.Instance.GetItemInfo(pickedupItemID);

                switch (ItemInfo.itemType)
                {
                    case ItemType.Armor:
                        switch (ItemInfo.armorProperties.armorType)
                        {
                            case ArmorType.NULL:
                                break;
                            case ArmorType.Head:
                                
                                if (BagManager.Instance.Dict_Equipped[EquippedItem.Head] != null)
                                {
                                    FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Head].ItemID);
                                    BagManager.Instance.UnequipItem(EquippedItem.Head);
                                    FSM.GetComponent<PlayerController>().UnEquip(EquippedItem.Head);
                                }
                                BagManager.Instance.PickItem(pickedupItemID);
                                FSM.GetComponent<PlayerController>().EquipHelmet(pickedupItemID);
                                FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                                break;
                            case ArmorType.Body:
                                if (BagManager.Instance.Dict_Equipped[EquippedItem.Body] != null)
                                {
                                    FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Body].ItemID);
                                    BagManager.Instance.UnequipItem(EquippedItem.Body);
                                    FSM.GetComponent<PlayerController>().UnEquip(EquippedItem.Body);
                                }
                                BagManager.Instance.PickItem(pickedupItemID);
                                FSM.GetComponent<PlayerController>().EquipClothes(pickedupItemID);
                                FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                                break;
                        }
                        break;
                    case ItemType.Weapon:
                        if (ItemInfo.weaponProperties.weaponType != WeaponType.Shield)
                        {
                            if (BagManager.Instance.Dict_Equipped[EquippedItem.Weapon1] != null)
                            {
                                if (BagManager.Instance.Dict_Equipped[EquippedItem.Weapon2] != null)
                                {
                                    if (BagManager.Instance.CurrentSelected == 2)
                                    {
                                        FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Weapon2].ItemID);
                                        BagManager.Instance.UnequipItem(EquippedItem.Weapon2);
                                        FSM.GetComponent<PlayerController>().UnEquip(EquippedItem.Weapon2);

                                    }
                                    else
                                    {
                                        FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Weapon1].ItemID);
                                        BagManager.Instance.UnequipItem(EquippedItem.Weapon1);
                                        FSM.GetComponent<PlayerController>().UnEquip(EquippedItem.Weapon1);
                                    }
                                    BagManager.Instance.PickItem(pickedupItemID);
                                    FSM.GetComponent<PlayerController>().EquipWeapon(pickedupItemID);
                                    FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                                }
                                else
                                {
                                    BagManager.Instance.PickItem(pickedupItemID);
                                    FSM.GetComponent<PlayerController>().EquipWeapon(pickedupItemID);
                                    FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                                }
                            }
                            else
                            {
                                BagManager.Instance.PickItem(pickedupItemID);
                                FSM.GetComponent<PlayerController>().EquipWeapon(pickedupItemID);
                                FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                            }
                        }
                        else
                        {
                            if (BagManager.Instance.Dict_Equipped[EquippedItem.Shield] != null)
                            {
                                FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Shield].ItemID);
                                BagManager.Instance.UnequipItem(EquippedItem.Shield);
                                FSM.GetComponent<PlayerController>().UnEquip(EquippedItem.Shield);
                            }
                            BagManager.Instance.PickItem(pickedupItemID);
                            FSM.GetComponent<PlayerController>().EquipShield(pickedupItemID);
                            FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                        }
                        break;
                    case ItemType.Magic:
                        if (BagManager.Instance.Dict_Equipped[EquippedItem.Magic1] != null)
                        {
                            if (BagManager.Instance.Dict_Equipped[EquippedItem.Magic2] != null)
                            {
                                if (BagManager.Instance.CurrentSelected == 2)
                                {
                                    FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Magic2].ItemID);
                                    BagManager.Instance.UnequipItem(EquippedItem.Magic2);
                                }
                                else
                                {
                                    FSM.GetComponent<PlayerController>().DiscardItem(BagManager.Instance.Dict_Equipped[EquippedItem.Magic1].ItemID);
                                    BagManager.Instance.UnequipItem(EquippedItem.Magic1);
                                }
                                BagManager.Instance.PickItem(pickedupItemID);
                                FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                            }
                            else
                            {
                                BagManager.Instance.PickItem(pickedupItemID);
                                FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                            }
                        }
                        else
                        {
                            BagManager.Instance.PickItem(pickedupItemID);
                            FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                        }
                        break;
                    case ItemType.Expendable:
                    case ItemType.Gem:
                        BagManager.Instance.PickItem(pickedupItemID);
                        FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                        break;
                }

                FSM.animationManager.PlayAnimation("Pick Up");

            }
            else
                Debug.LogError("拾起失败");
        else
            Debug.LogError("拾起失败");
    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM.GetComponent<NetworkPlayer>(),
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
    }

}
