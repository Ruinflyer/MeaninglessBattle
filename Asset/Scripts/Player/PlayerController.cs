using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerController : MeaninglessCharacterController
{

    private List<SingleItemInfo> EquippedList;
    private Dictionary<MagicType, Timer> magicCDTimer;
    private int preSelected;

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
            preSelected = CurrentSelected;
            CurrentSelected = 5;
        }

        if (Input.GetButtonUp("Defend"))
        {
            CurrentSelected = preSelected;
        }

        switch (CurrentSelected)
        {
            case 1:
                GetComponent<PlayerBag>().EquipItem(EquippedItem.Weapon1, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Weapon1].ItemID));
                //EquipWeapon(EquippedItem.Weapon1, EquippedList[(int)EquippedItem.Weapon1].ItemID);
                break;
            case 2:
                GetComponent<PlayerBag>().EquipItem(EquippedItem.Weapon2, ItemInfoManager.Instance.GetItemInfo(EquippedList[(int)EquippedItem.Weapon2].ItemID));
                // EquipWeapon(EquippedItem.Weapon2, EquippedList[(int)EquippedItem.Weapon2].ItemID);
                break;
            case 3:
                break;
            case 4:
                break;
        }

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
