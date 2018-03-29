﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;

public class HUDUI : BaseUI
{
    //AutoStatement
    private Image Img_Weapon1 = null;
	private Image Img_Weapon2 = null;
	private Image Img_Skill1 = null;
	private Image Img_Skill2 = null;
	private Image Img_Shield = null;
    private Image Img_FrontSight;

    protected override void InitUiOnAwake()
    {
        Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon1");
		Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon2");
		Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill1");
		Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill2");
		Img_Shield = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Shield");
        Img_FrontSight = GameTool.GetTheChildComponent<Image>(gameObject, "Img_FrontSight");
        MessageCenter.AddListener(EMessageType.FoundItem, AwakeFrontSight);
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.HUDUI;
    }

    private void AwakeFrontSight(object Active)
    {
        Img_FrontSight.gameObject.SetActive((bool)Active);
    }
}