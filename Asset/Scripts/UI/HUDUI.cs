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
    [SerializeField]
    private List<SingleItemInfo> EquippedList;


    protected override void InitUiOnAwake()
    {
        Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon1");
		Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon2");
		Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill1");
		Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill2");
		Img_Shield = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Shield");
        Img_PickUpTip = GameTool.GetTheChildComponent<Image>(this.gameObject, "PickUpTip");
        Img_FrontSight = GameTool.GetTheChildComponent<Image>(gameObject, "Img_FrontSight");
        EquippedList = new List<SingleItemInfo>();

        MessageCenter.AddListener(EMessageType.FoundItem, AwakePickUpTip);
        
        MessageCenter.AddListener(EMessageType.GetAndSetEquippedList, (object obj) =>
        {
            EquippedList = (List<SingleItemInfo>)obj;
            SetBarIcon(EquippedList);
        });
        
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.HUDUI;
    }

    private void SetBarIcon(List<SingleItemInfo> EquippedList)
    {
        if (EquippedList[(int)EquippedItem.Weapon1] != null)
            Img_Weapon1.sprite=ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon1].ResName);
        if (EquippedList[(int)EquippedItem.Weapon2] != null)
            Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon2].ResName);
        if (EquippedList[(int)EquippedItem.Magic1] != null)
            Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Magic1].ResName);
        if (EquippedList[(int)EquippedItem.Magic2] != null)
            Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Magic2].ResName);
        if (EquippedList[(int)EquippedItem.Shield] != null)
            Img_Shield.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Shield].ResName);

    }
    
    private void AwakePickUpTip(object Active)
    {
        Img_PickUpTip.gameObject.SetActive((bool)Active);
    }
}
