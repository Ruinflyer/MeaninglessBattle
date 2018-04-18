using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerController : MeaninglessCharacterController
{

    private List<SingleItemInfo> EquippedList;
    private Dictionary<MagicType, Timer> magicCDTimer;
    private int preSelected;
    private Dictionary<Transform, int> Dict_PickUp_Tran = new Dictionary<Transform, int>();
    public List<BaseFSM> List_CanAttack = new List<BaseFSM>();

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

        if (Input.GetButtonDown("Bar1"))
        {
            CurrentSelected = 1;

        }
        if (Input.GetButtonDown("Bar2"))
        {
            CurrentSelected = 2;

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
            Debug.Log("C" + CurrentSelected);
            preSelected = CurrentSelected;
            Debug.Log(preSelected);
            CurrentSelected = 5;
        }

        if (Input.GetButtonUp("Defend"))
        {
            Debug.Log(CurrentSelected);
            CurrentSelected = preSelected;
        }

        switch (CurrentSelected)
        {
            case 1:
                if (EquippedList[(int)EquippedItem.Shield].ItemName != null && EquippedList[(int)EquippedItem.Weapon1].weaponProperties.weaponType != WeaponType.DoubleHands)
                {
                    GetComponent<PlayerBag>().EquipItem(EquippedItem.Shield, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Shield].ItemID));
                }
                else if (EquippedList[(int)EquippedItem.Weapon1].weaponProperties.weaponType == WeaponType.DoubleHands)
                {
                    UnEquip(EquippedItem.Shield);
                }
                UnEquip(EquippedItem.Weapon2);
                GetComponent<PlayerBag>().EquipItem(EquippedItem.Weapon1, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Weapon1].ItemID));
                //EquipWeapon(EquippedItem.Weapon1, EquippedList[(int)EquippedItem.Weapon1].ItemID);
                break;
            case 2:
                if (EquippedList[(int)EquippedItem.Shield].ItemName != null && EquippedList[(int)EquippedItem.Weapon2].weaponProperties.weaponType != WeaponType.DoubleHands)
                {
                    GetComponent<PlayerBag>().EquipItem(EquippedItem.Shield, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Shield].ItemID));
                }
                else if (EquippedList[(int)EquippedItem.Weapon2].weaponProperties.weaponType == WeaponType.DoubleHands)
                {
                    UnEquip(EquippedItem.Shield);
                }
                UnEquip(EquippedItem.Weapon1);
                GetComponent<PlayerBag>().EquipItem(EquippedItem.Weapon2, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Weapon2].ItemID));
                // EquipWeapon(EquippedItem.Weapon2, EquippedList[(int)EquippedItem.Weapon2].ItemID);
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

    public bool CheckCanAttack(GameObject center,GameObject enemy, float distance,float angle)
    {
        Vector3 relativeVector = enemy.transform.position - center.transform.position;
        float dot = Vector3.Dot(center.transform.forward, relativeVector);

        if (dot > Mathf.Cos(angle) * distance)
        {
            if (!List_CanAttack.Contains(enemy.GetComponent<BaseFSM>()))
                List_CanAttack.Add(enemy.GetComponent<BaseFSM>());
            return true;
        }
        else
        {
            if (List_CanAttack.Contains(enemy.GetComponent<BaseFSM>()))
                List_CanAttack.Remove(enemy.GetComponent<BaseFSM>());
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
        MessageCenter.AddListener(EMessageType.GetAndSetEquippedList, (object obj) =>
        {
            EquippedList = (List<SingleItemInfo>)obj;
        });
    }

    protected override void CCFixedUpdate()
    {
        UseGravity(Gravity);
        OpenBag();
        ChangeWeapon();
        MessageCenter.Send(EMessageType.CurrentselectedWeapon, CurrentSelected);
    }
}
