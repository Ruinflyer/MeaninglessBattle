using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Meaningless;


public class HUDUI : BaseUI
{
    //AutoStatement
    private Image Img_Weapon1 = null;
	private Image Img_Weapon2 = null;
	private Image Img_Skill1 = null;
	private Image Img_Skill2 = null;
	private Image Img_Shield = null;
    private Image Img_PickUpTip=null;
    private Image Img_FrontSight=null;



    protected override void InitUiOnAwake()
    {
        Img_PickUpTip = GameTool.GetTheChildComponent<Image>(this.gameObject, "PickUpTip");
        Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon1");
		Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon2");
		Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill1");
		Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill2");
		Img_Shield = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Shield");
        Img_FrontSight = GameTool.GetTheChildComponent<Image>(gameObject, "Img_FrontSight");

        MessageCenter.AddListener(EMessageType.FoundItem, AwakePickUpTip);
        
    }

    private void Update()
    {
        SetBarIcon(BagManager.Instance.Dict_Equipped);
    }

    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.HUDUI;
    }

    private void AwakePickUpTip(object Active)
    {
        Img_PickUpTip.gameObject.SetActive((bool)Active);
    }

    private void SetBarIcon(Dictionary<EquippedItem,SingleItemInfo> EquippedList)
    {
        if (EquippedList[EquippedItem.Weapon1] != null)
            Img_Weapon1.sprite=ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon1].ResName);
        if (EquippedList[EquippedItem.Weapon2] != null)
            Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon2].ResName);
        if (EquippedList[EquippedItem.Magic1] != null)
            Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic1].ResName);
        if (EquippedList[EquippedItem.Magic2] != null)
            Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic2].ResName);
        if (EquippedList[EquippedItem.Shield] != null)
            Img_Shield.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Shield].ResName);

    }
    

}
