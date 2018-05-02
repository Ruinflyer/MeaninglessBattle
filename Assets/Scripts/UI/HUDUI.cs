﻿using UnityEngine;
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
    private Text Text_Remain = null;
    private Text Text_Time = null;

    private bool RefreshTime = false;
    private int countdownTime = 0;
    private bool isMoving = false;
    private float lastTime = 0;
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
        Text_Remain = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Remain");
        Text_Time = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Time");
        MessageCenter.AddListener(EMessageType.FoundItem, AwakePickUpTip);
        MessageCenter.AddListener(EMessageType.CurrentHP, UpdateHP);
        MessageCenter.AddListener(EMessageType.Remain, (object obj) => { Text_Remain.text = "" + (int)obj; });
        MessageCenter.AddListener_Multparam(EMessageType.CountdownTime,(object[] objs)=> 
        {
            countdownTime = (int)objs[0];//倒计时秒数
            isMoving = (bool)objs[1];//false为保持时间 true为移动时间
            RefreshTime = true;//开始计时
        });
    }

    private void Update()
    {
        SetBarIcon();
        UpdateSkillCount();
        UpdateTime();
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

    private void SetBarIcon()
    {
        if (BagManager.Instance.Weapon1.weaponProperties!=null)
        {
            if (BagManager.Instance.Weapon1.ResName!="")
                Img_Weapon1.sprite = ResourcesManager.Instance.GetUITexture(BagManager.Instance.Weapon1.ResName);
        }
        if (BagManager.Instance.Weapon2.weaponProperties != null)
        {
            if (BagManager.Instance.Weapon2.ResName != "")
                Img_Weapon2.sprite = ResourcesManager.Instance.GetUITexture(BagManager.Instance.Weapon2.ResName);
        }
        if (BagManager.Instance.Magic1.magicProperties != null)
        {
            if (BagManager.Instance.Magic1.ResName != "")
                Img_Skill1.sprite = ResourcesManager.Instance.GetUITexture(BagManager.Instance.Magic1.ResName);
        }
        if(BagManager.Instance.Magic2.magicProperties != null)
        {
            if (BagManager.Instance.Magic2.ResName != "")
                Img_Skill2.sprite = ResourcesManager.Instance.GetUITexture(BagManager.Instance.Magic2.ResName);
        }

    }
    
    private void UpdateTime()
    {
        if(Time.time- lastTime>1f)
        {
            countdownTime -= 1;
            if (RefreshTime)
            {
                if (isMoving)
                {
                    Text_Time.text = "暗影移动时间: " + countdownTime.ToString() + "秒";
                }
                else
                {
                    Text_Time.text = "暗影保持时间: " + countdownTime.ToString() + "秒";
                }
            }
            lastTime = Time.time;
        }
       
    }

}
