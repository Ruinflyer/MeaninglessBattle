using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerController : MeaninglessCharacterController
{

    /// <summary>
    ///  Key:GroundItemID Value:ItemID
    /// </summary>
    private Dictionary<int, int> Dict_PickUp_Tran = new Dictionary<int, int>();

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
        Debug.Log(itemObj);
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
                    Destroy(preHead);
                }
                break;
            case EquippedItem.Body:
                GameObject itemObj = ResourcesManager.Instance.GetItem("Armor_Casual");
                Material clothesMat = itemObj.GetComponent<MeshRenderer>().sharedMaterial;
                GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
                break;
            case EquippedItem.Weapon1:
                if ((RHand.childCount != 0))
                {
                    GameObject preWeapon1 = RHand.GetChild(0).gameObject;
                    string preWeaponName = preWeapon1.name;
                    //销毁
                    Destroy(preWeapon1);
                    if (LHand.childCount != 0)
                        if (LHand.GetChild(0).name == preWeaponName)
                        {
                            //销毁
                            Destroy(LHand.GetChild(0).gameObject);
                        }
                }
                break;
            case EquippedItem.Weapon2:
                if (RHand.childCount != 0)
                {
                    GameObject preWeapon2 = RHand.GetChild(0).gameObject;
                    string preWeaponName = preWeapon2.name;
                    //销毁
                    Destroy(preWeapon2);
                    if (LHand.childCount != 0)
                        if (LHand.GetChild(0).name == preWeaponName)
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

    public override void Move(float walkSpeed, float jumpSpeed)
    {
        Vector3 moveDirection = Vector3.zero;
        if (CC.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= walkSpeed;
            if (Input.GetButtonDown("Jump"))
                moveDirection.y += jumpSpeed;
        }
        CC.Move(moveDirection * Time.fixedDeltaTime);
    }

    public override void FallingCtrl(float Speed)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= Speed;
        CC.Move(moveDirection * Time.fixedDeltaTime);
    }

    public override void ChangeWeapon(int currentSelected)
    {
        switch (currentSelected)
        {
            case 1:
                if (BagManager.Instance.Weapon1 != null)
                {
                    if (BagManager.Instance.Weapon2 != null)
                    {
                        UnEquip(EquippedItem.Weapon2);
                    }
                    EquipWeapon(BagManager.Instance.Weapon1.ItemID);
                }
                break;
            case 2:
                if (BagManager.Instance.Weapon2 != null)
                {
                    if (BagManager.Instance.Weapon1 != null)
                    {
                        UnEquip(EquippedItem.Weapon1);
                    }
                    EquipWeapon(BagManager.Instance.Weapon2.ItemID);
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
        GroundItem GItem = Item.GetComponent<GroundItem>();
        Dict_PickUp_Tran.Add(GItem.GroundItemID, GItem.ItemID);
        Item.SetParent(transform);
        NetworkManager.SendPickItem(GItem.GroundItemID);
        Item.gameObject.SetActive(false);
    }

    public void DiscardItem(int itemID)
    {
        foreach (int GID in Dict_PickUp_Tran.Keys)
        {
            if(Dict_PickUp_Tran[GID]==itemID)
            {
                GroundItem GItem = MapManager.Instance.Items[GID];
                //发送
                NetworkManager.SendDropItem(GID, GItem.gameObject.transform);
                GItem.gameObject.transform.SetParent(null);
                GItem.gameObject.SetActive(true);
                Dict_PickUp_Tran.Remove(GID);
                break;
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
                itemInfo = BagManager.Instance.Weapon1;
                break;
            case 2:
                itemInfo = BagManager.Instance.Weapon2;
                break;
            case 3:
                itemInfo = BagManager.Instance.Magic1;
                break;
            case 4:
                itemInfo = BagManager.Instance.Magic2;
                break;
        }
        return itemInfo;
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
        float dis = (enemy.transform.position - center.transform.position).magnitude;
        Vector3 relativeVector = enemy.transform.position - center.transform.position;
        float ang = Vector3.Angle(relativeVector, center.transform.forward);

        if (dis < distance && ang < angle)
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

    private void GetBuff(BuffType buff, CharacterStatus status)
    {
        ;
        Material buffMat = Resources.Load("Debuff/" + buff.ToString()) as Material;
        switch (buff)
        {
            case BuffType.SlowDown:
                if (buffMat != null)
                    GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = buffMat;
                status.moveSpeed *= 0.7f;
                break;
            case BuffType.Freeze:
                if (buffMat != null)
                    GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = buffMat;
                status.moveSpeed *= 0.001f;
                break;
            case BuffType.Blind:
                Camera.main.GetComponent<BlindEffect>().enabled = true;
                break;
        }

    }

    private void Losebuff(BuffType buff, CharacterStatus status)
    {
        Material noBuffMat = Resources.Load("Debuff/NoBuff") as Material;
        switch (buff)
        {
            case BuffType.SlowDown:
                status.moveSpeed /= 0.7f;
                if (BagManager.Instance.Body == null)
                    UnEquip(EquippedItem.Body);
                else
                    EquipClothes(BagManager.Instance.Body.ItemID);
                break;
            case BuffType.Freeze:
                if (BagManager.Instance.Body == null)
                    UnEquip(EquippedItem.Body);
                else
                    EquipClothes(BagManager.Instance.Body.ItemID);
                status.moveSpeed /= 0.001f;
                break;
            case BuffType.Blind:
                Camera.main.GetComponent<BlindEffect>().enabled = false;
                break;
        }

    }

    public override void GetDeBuffInTime(BuffType debuff, float time, CharacterStatus status)
    {
        Buff buff = new Buff(debuff, time, status, GetBuff, Losebuff);
        buffList.Add(buff);
        foreach (Buff Buff in buffList)
        {
            Buff.OnEnter();
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
        Transform RigPelvis = GameTool.FindTheChild(gameObject, "RigPelvis");
        LHand = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Left");
        RHand = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Right");
        Head = GameTool.FindTheChild(RigPelvis.gameObject, "Dummy Prop Head");
        Wings = GameTool.FindTheChild(gameObject, "Wings");
    }

    protected override void CCUpdate()
    {
        CurrentSelected = BagManager.Instance.CurrentSelected;
        OpenBag();
        Esc();
        //MessageCenter.Send(EMessageType.CurrentselectedWeapon, CurrentSelected);

        if (buffList.Count > 0)
        {
            foreach (Buff buff in buffList)
            {
                if (buff.canUpdate)
                    buff.OnUpdate();
                if (buff.canDestory)
                {
                    buff.OnExit();
                    buffList.Remove(buff);
                }
            }
        }

    }

    protected override void CCFixedUpdate()
    {
        UseGravity(Gravity);
    }
}
