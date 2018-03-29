using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerController : MeaninglessCharacterController
{

    public override void EquipClothes(int itemID)
    {
        string itemName = ItemInfoManager.Instance.GetItemName(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        Material clothesMat = itemObj.GetComponent<MeshRenderer>().material;
        GameTool.FindTheChild(gameObject, "Base").GetComponent<SkinnedMeshRenderer>().material = clothesMat;
    }

    public override void EquipHelmet(int itemID,Transform Head)
    {
        if (Head.childCount != 0)
        {
            GameObject preWeapon = Head.GetChild(0).gameObject;
            int preWeaponID = preWeapon.GetComponent<GroundItem>().ItemID;
            gameObject.GetComponent<PlayerBag>().PickItem(preWeaponID);
            //销毁
            Destroy(preWeapon);
        }

        string itemName = ItemInfoManager.Instance.GetItemName(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject RWeapon = Instantiate(itemObj, Head);
    }

    public override void EquipWeapon(int itemID,Transform LHand,Transform RHand)
    {
        if(RHand.childCount!=0)
        {
            GameObject preWeapon = RHand.GetChild(0).gameObject;
            int preWeaponID = preWeapon.GetComponent<GroundItem>().ItemID;
            gameObject.GetComponent<PlayerBag>().PickItem(preWeaponID);
            //销毁
            Destroy(preWeapon);
            if(ItemInfoManager.Instance.GetWeaponWeaponType(preWeaponID)==WeaponType.DoubleHands)
            {
                //销毁
                Destroy(LHand.GetChild(0).gameObject);
            }
        }

        string itemName = ItemInfoManager.Instance.GetItemName(itemID);
        GameObject itemObj = ResourcesManager.Instance.GetItem(itemName);
        GameObject RWeapon=Instantiate(itemObj, RHand);
        RWeapon.transform.parent = RHand;

        if (ItemInfoManager.Instance.GetWeaponWeaponType(itemID)==WeaponType.DoubleHands)
        {
            GameObject LWeapon=Instantiate(itemObj, LHand);
            LWeapon.transform.parent = LHand;
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

    protected override void CCFixedUpdate()
    {
        UseGravity(Gravity);
    }
}
