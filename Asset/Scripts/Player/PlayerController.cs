﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerController : MeaninglessCharacterController
{

    private Dictionary<EquippedItem, SingleItemInfo> EquippedDict = new Dictionary<EquippedItem, SingleItemInfo>();

    private Dictionary<Transform, int> Dict_PickUp_Tran = new Dictionary<Transform, int>();

    /*
    public void Equip(int itemID)
    {
        if (itemID != 0)
        {
            SingleItemInfo ItemInfo;
            ItemInfo = ItemInfoManager.Instance.GetItemInfo(itemID);
            switch (ItemInfo.itemType)
            {
                case ItemType.Armor:
                    switch (ItemInfo.armorProperties.armorType)
                    {
                        case ArmorType.NULL:
                            break;
                        case ArmorType.Head:
                            GameObject headRes = ResourcesManager.Instance.GetItem(ItemInfo.ItemName);
                            GameObject headObj = Instantiate(headRes, Head);
                            break;
                        case ArmorType.Body:
                            GameObject bodyObj = ResourcesManager.Instance.GetItem(ItemInfo.ItemName);
                            Material clothesMat = bodyObj.GetComponent<MeshRenderer>().sharedMaterial;
                            GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
                            break;
                    }
                    break;
                case ItemType.Weapon:
                    if (ItemInfo.weaponProperties.weaponType != WeaponType.Shield)
                    {
                        GameObject weaponRes = ResourcesManager.Instance.GetItem(ItemInfo.ItemName);
                        GameObject RWeapon = Instantiate(weaponRes, RHand);
                        RWeapon.transform.parent = RHand;

                        if (ItemInfoManager.Instance.GetWeaponWeaponType(itemID) == WeaponType.DoubleHands)
                        {
                            GameObject LWeapon = Instantiate(weaponRes, LHand);
                            LWeapon.transform.parent = LHand;
                        }

                    }
                    else
                    {
                        GameObject itemRes = ResourcesManager.Instance.GetItem(ItemInfo.ItemName);
                        GameObject Shield = Instantiate(itemRes, LHand);
                        Shield.transform.parent = LHand;
                    }
                    break;
            }
        }
    }
    */

    
    public void EquipClothes(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        Material clothesMat = itemObj.GetComponent<MeshRenderer>().sharedMaterial;
        GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
    }

    public void EquipHelmet(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject RWeapon = Instantiate(itemObj, Head);
    }


    public void EquipWeapon(int itemID)
    {

        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject RWeapon = Instantiate(itemObj, RHand);
        RWeapon.transform.parent = RHand;

        if (ItemInfoManager.Instance.GetWeaponWeaponType(itemID) == WeaponType.DoubleHands)
        {
            GameObject LWeapon = Instantiate(itemObj, LHand);
            LWeapon.transform.parent = LHand;
        }

    }

    public void EquipShield(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetResname(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject Shield = Instantiate(itemObj, LHand);
        Shield.transform.parent = LHand;
    }
    


    public void UnEquip(EquippedItem equippedItem)
    {
        switch (equippedItem)
        {
            case EquippedItem.Head:
                if (Head.childCount != 0)
                {
                    GameObject preHead = Head.GetChild(0).gameObject;
                    int preHeadID = preHead.GetComponent<GroundItem>().ItemID;
                    Destroy(preHead);
                }
                break;
            case EquippedItem.Body:
                GameObject itemObj = ResourcesManager.Instance.GetItem("Ground_Armor_Casual");
                Material clothesMat = itemObj.GetComponent<MeshRenderer>().sharedMaterial;
                GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
                break;
            case EquippedItem.Weapon1:
                if ((RHand.childCount != 0))
                {
                    GameObject preWeapon1 = RHand.GetChild(0).gameObject;
                    int preWeaponID1 = preWeapon1.GetComponent<GroundItem>().ItemID;
                    //销毁
                    Destroy(preWeapon1);
                    if (ItemInfoManager.Instance.GetWeaponWeaponType(preWeaponID1) == WeaponType.DoubleHands)
                    {
                        //销毁
                        Destroy(LHand.GetChild(0).gameObject);
                    }
                }
                break;
            case EquippedItem.Weapon2:
                if ((RHand.childCount != 0))
                {
                    GameObject preWeapon2 = RHand.GetChild(0).gameObject;
                    int preWeaponID2 = preWeapon2.GetComponent<GroundItem>().ItemID;
                    //销毁
                    Destroy(preWeapon2);
                    if (ItemInfoManager.Instance.GetWeaponWeaponType(preWeaponID2) == WeaponType.DoubleHands)
                    {
                        //销毁
                        Destroy(LHand.GetChild(0).gameObject);
                    }
                }
                break;
            case EquippedItem.Shield:
                if (LHand.childCount != 0)
                {
                    GameObject preShield = LHand.GetChild(0).gameObject;
                    int preWeaponID2 = preShield.GetComponent<GroundItem>().ItemID;
                    //销毁
                    Destroy(preShield);
                }
                break;
        }
    }

    public override void Jump(float jumpSpeed)
    {
        Vector3 moveDirection = Vector3.zero;
        if (CC.isGrounded)
        {
            moveDirection.y += jumpSpeed * Time.fixedDeltaTime;
        }
        CC.Move(moveDirection * Time.fixedDeltaTime);
    }

    public override void Move(float walkSpeed)
    {
        Vector3 moveDirection = Vector3.zero;
        if (CC.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed;
        }
        CC.Move(moveDirection * Time.fixedDeltaTime);
    }



    public override void FindTranform(Body type)
    {
        Transform RigPelvis = GameTool.FindTheChild(gameObject, "RigPelvis");
        if (type == Body.LHand)
        {
            LHand = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Left");
        }
        if (type == Body.RHand)
        {
            RHand = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Right");
        }
        if (type == Body.Head)
        {
            Head = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Head");
        }
    }

    public override void ChangeWeapon()
    {
        switch (CurrentSelected)
        {
            case 1:
                if (EquippedDict[EquippedItem.Weapon1] != null)
                {
                    if (EquippedDict[EquippedItem.Shield] != null && EquippedDict[EquippedItem.Weapon1].weaponProperties.weaponType != WeaponType.DoubleHands)
                    {
                        BagManager.Instance.EquipItem(EquippedItem.Shield, ItemInfoManager.Instance.GetItemInfo(EquippedDict[EquippedItem.Shield].ItemID));
                        EquipShield(EquippedDict[EquippedItem.Shield].ItemID);
                    }
                    else if (EquippedDict[EquippedItem.Weapon1].weaponProperties.weaponType == WeaponType.DoubleHands)
                    {
                        BagManager.Instance.UnequipItem(EquippedItem.Shield);
                        UnEquip(EquippedItem.Shield);
                    }

                    if (EquippedDict[EquippedItem.Weapon2] != null)
                    {
                        BagManager.Instance.UnequipItem(EquippedItem.Weapon2);
                        UnEquip(EquippedItem.Weapon2);
                    }

                    //BagManager.Instance.EquipItem(EquippedItem.Weapon1, ItemInfoManager.Instance.GetItemInfo(EquippedDict[EquippedItem.Weapon1].ItemID));
                    EquipWeapon(EquippedDict[EquippedItem.Weapon1].ItemID);
                }
                break;
            case 2:
                if (EquippedDict[EquippedItem.Weapon2] != null)
                {
                    if (EquippedDict[EquippedItem.Shield] != null && EquippedDict[EquippedItem.Weapon2].weaponProperties.weaponType != WeaponType.DoubleHands)
                    {
                        BagManager.Instance.EquipItem(EquippedItem.Shield, ItemInfoManager.Instance.GetItemInfo(EquippedDict[EquippedItem.Shield].ItemID));
                        EquipShield(EquippedDict[EquippedItem.Shield].ItemID);
                    }
                    else if (EquippedDict[EquippedItem.Weapon2].weaponProperties.weaponType == WeaponType.DoubleHands)
                    {
                        BagManager.Instance.UnequipItem(EquippedItem.Shield);
                        UnEquip(EquippedItem.Shield);
                    }
                    if (EquippedDict[EquippedItem.Weapon1] != null)
                    {
                        BagManager.Instance.UnequipItem(EquippedItem.Weapon1);
                        UnEquip(EquippedItem.Weapon1);
                    }
                    //BagManager.Instance.EquipItem(EquippedItem.Weapon2, ItemInfoManager.Instance.GetItemInfo(EquippedDict[EquippedItem.Weapon2].ItemID));
                    EquipWeapon(EquippedDict[EquippedItem.Weapon2].ItemID);
                }
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

    }

    public void PickItem(Transform Item)
    {
        Dict_PickUp_Tran.Add(Item, Item.GetComponent<GroundItem>().ItemID);
        Item.SetParent(transform);
        Item.gameObject.SetActive(false);
    }

    public void DiscardItem(int itemID)
    {
        foreach (Transform key in Dict_PickUp_Tran.Keys)
        {
            if (Dict_PickUp_Tran[key].Equals(itemID))
            {
                key.SetParent(null);
                key.gameObject.SetActive(true);
                Dict_PickUp_Tran.Remove(key);
            }
        }
    }

    public override SingleItemInfo GetCurSelectedWeaponInfo()
    {
        SingleItemInfo itemInfo = null;
        switch (CurrentSelected)
        {
            case 0:
                itemInfo = null;
                break;
            case 1:
                itemInfo = EquippedDict[EquippedItem.Weapon1];
                break;
            case 2:
                itemInfo = EquippedDict[EquippedItem.Weapon2];
                break;
            case 3:
                itemInfo = EquippedDict[EquippedItem.Magic1];
                break;
            case 4:
                itemInfo = EquippedDict[EquippedItem.Magic2];
                break;
            case 5:
                itemInfo = EquippedDict[EquippedItem.Shield];
                break;
        }
        return itemInfo;
    }



    //单人测试用搜索敌人
    public override void SearchEnemy(float Range)
    {
        Collider[] colliderArr = Physics.OverlapSphere(transform.position, Range, LayerMask.GetMask("Player"));
        for (int i = 0; i < colliderArr.Length; i++)
        {
            if (colliderArr[i].GetComponent<BaseFSM>() != null && colliderArr[i] != gameObject)
            {
                if (!List_Enemy.Contains(colliderArr[i].GetComponent<NetworkPlayer>()))
                    List_Enemy.Add(colliderArr[i].GetComponent<NetworkPlayer>());
            }
        }
    }



    /// <summary>
    /// 检测敌人是否处于攻击范围
    /// </summary>
    /// <param name="center"></param>
    /// <param name="enemy"></param>
    /// <param name="distance"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public override bool CheckCanAttack(GameObject center, GameObject enemy, float distance, float angle)
    {
        Vector3 relativeVector = enemy.transform.position - center.transform.position;
        float dot = Vector3.Dot(center.transform.forward, relativeVector);

        if (dot > Mathf.Cos(angle) * distance)
        {
            if (!List_CanAttack.Contains(enemy.GetComponent<NetworkPlayer>()))
                List_CanAttack.Add(enemy.GetComponent<NetworkPlayer>());
            return true;
        }
        else
        {
            if (List_CanAttack.Contains(enemy.GetComponent<NetworkPlayer>()))
                List_CanAttack.Remove(enemy.GetComponent<NetworkPlayer>());
            return false;
        }
    }


    public override void UseGravity(float Gravity)
    {
        Vector3 moveDirection = Vector3.zero;
        if (!CC.isGrounded)
        {
            moveDirection.y -= Gravity * Time.fixedDeltaTime;
        }
        else
            moveDirection = Vector3.zero;
        CC.Move(moveDirection);
    }

    protected override void Initialize()
    {
        FindTranform(Body.RHand);
        FindTranform(Body.LHand);
        FindTranform(Body.Head);
    }

    protected override void CCUpdate()
    {
        CurrentSelected = BagManager.Instance.CurrentSelected;
        EquippedDict = BagManager.Instance.Dict_Equipped;
        OpenBag();
        ChangeWeapon();
        MessageCenter.Send(EMessageType.CurrentselectedWeapon, CurrentSelected);
        SearchEnemy(10);
    }

    protected override void CCFixedUpdate()
    {
        UseGravity(Gravity);
    }
}
