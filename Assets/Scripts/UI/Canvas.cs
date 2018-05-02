using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
public class Canvas : BaseUI
{
    //AutoStatement
    private Button Btn_RandomName = null;
	private Button Btn_Login = null;
	private Image Img_Weapon1 = null;
	private Image Img_Weapon2 = null;
	private Image Img_Skill1 = null;
	private Image Img_Skill2 = null;
	private Image Img_FrontSight = null;
	private Button Btn_Close = null;
	private Button Btn_Create = null;
	private Button Btn_Refresh = null;
	private Button Btn_Leave = null;
	private Button Btn_Prepare = null;
	private Button Btn_Start = null;
	private Button Btn_OK = null;
	
    protected override void InitUiOnAwake()
    {
        Btn_RandomName = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_RandomName");
		Btn_Login = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Login");
		Img_Weapon1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon1");
		Img_Weapon2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Weapon2");
		Img_Skill1 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill1");
		Img_Skill2 = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_Skill2");
		Img_FrontSight = GameTool.GetTheChildComponent<Image>(this.gameObject,"Img_FrontSight");
		Btn_Close = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Close");
		Btn_Create = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Create");
		Btn_Refresh = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Refresh");
		Btn_Leave = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Leave");
		Btn_Prepare = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Prepare");
		Btn_Start = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_Start");
		Btn_OK = GameTool.GetTheChildComponent<Button>(this.gameObject,"Btn_OK");
		
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.Canvas;
    }
}
