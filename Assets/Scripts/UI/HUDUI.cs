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
    private Image Img_Skill1_Mask = null;
    private Image Img_Skill2_Mask = null;
    private Image Img_Shield = null;
    private Image Img_PickUpTip=null;
    private Image Img_FrontSight=null;
    private Text Text_Skill1_Count = null;
    private Text Text_Skill2_Count = null;
    private Slider Slider_HP = null;

   

    protected override void InitUiOnAwake()
    {
        Img_PickUpTip = GameTool.GetTheChildComponent<Image>(this.gameObject, "PickUpTip");
        Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon1");
		Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon2");
		Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill1");
		Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill2");
        Img_Skill1_Mask= GameTool.GetTheChildComponent<Image>(Img_Skill1.gameObject, "Ing_Skill1_Mask");
        Img_Skill2_Mask = GameTool.GetTheChildComponent<Image>(Img_Skill2.gameObject, "Ing_Skill2_Mask");
        Img_Shield = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Shield");
        Img_FrontSight = GameTool.GetTheChildComponent<Image>(gameObject, "Img_FrontSight");
        Text_Skill1_Count= GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Count3");
        Text_Skill2_Count = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Count4");
        Slider_HP = GameTool.GetTheChildComponent<Slider>(this.gameObject, "Slider");
        MessageCenter.AddListener(EMessageType.FoundItem, AwakePickUpTip);
        MessageCenter.AddListener(EMessageType.CurrentHP, UpdateHP);
    }

    private void Update()
    {
        SetBarIcon(BagManager.Instance.Dict_Equipped);
        UpdateSkillCount();
    }

    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.HUDUI;
    }

    private void AwakePickUpTip(object Active)
    {
        Img_PickUpTip.gameObject.SetActive((bool)Active);
    }

    private void UpdateSkillCount()
    {
        Text_Skill1_Count.text = BagManager.Instance.skillAttributesList[0].remainCount+ "/" + BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.UsableCount;
        Img_Skill1_Mask.fillAmount = BagManager.Instance.skillAttributesList[0].Timer / BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.CDTime;
        Text_Skill2_Count.text = BagManager.Instance.skillAttributesList[1].remainCount + "/" + BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.UsableCount;
        Img_Skill2_Mask.fillAmount = BagManager.Instance.skillAttributesList[1].Timer / BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.CDTime;
    }

    private void UpdateHP(object HP)
    {
        Slider_HP.value = (float)HP/100;
    }

    private void SetBarIcon(Dictionary<EquippedItem, SingleItemInfo> EquippedList)
    {
        if (EquippedList.ContainsKey(EquippedItem.Weapon1))
        {
            if (EquippedList[EquippedItem.Weapon1] != null && EquippedList[EquippedItem.Weapon1].ResName!=null)
                Img_Weapon1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon1].ResName);
        }
        if (EquippedList.ContainsKey(EquippedItem.Weapon2) )
        {
            if (EquippedList[EquippedItem.Weapon2] != null && EquippedList[EquippedItem.Weapon2].ResName != null)
                Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon2].ResName);
        }
        if (EquippedList.ContainsKey(EquippedItem.Magic1) )
        {
            if (EquippedList[EquippedItem.Magic1] != null && EquippedList[EquippedItem.Magic1].ResName != null)
                Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic1].ResName);
        }
        if (EquippedList.ContainsKey(EquippedItem.Magic2))
        {
            if (EquippedList[EquippedItem.Magic2] != null && EquippedList[EquippedItem.Magic2].ResName != null)
                Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic2].ResName);
        }
        if (EquippedList.ContainsKey(EquippedItem.Shield) )
        {
            if (EquippedList[EquippedItem.Shield] != null && EquippedList[EquippedItem.Shield].ResName != null)
                Img_Shield.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Shield].ResName);
        }

    }
    

}
