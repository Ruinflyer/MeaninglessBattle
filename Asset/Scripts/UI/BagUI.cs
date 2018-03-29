﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
using System.Collections.Generic;

public class BagUI : BaseUI
{
    //AutoStatement
    private Button Btn_Close = null;
    private ScrollRect scrollRect = null;

    private Image HeadSlot;
    private Image HeadGem1;
    private Image HeadGem2;
    private Image BodySlot;
    private Image BodyGem1;
    private Image BodyGem2;
    private Image WeaponSlot1;
    private Image Weapon1Gem1;
    private Image Weapon1Gem2;
    private Image WeaponSlot2;
    private Image Weapon2Gem1;
    private Image Weapon2Gem2;
    private Image Magic1;
    private Image Magic2;

    [SerializeField]
    private List<SingleItemInfo> PickUpList;
    [SerializeField]
    private List<SingleItemInfo> EquippedList;
    [SerializeField]
    private List<GameObject> BagItem;
    protected override void InitUiOnAwake()
    {
        Btn_Close = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Close");
        scrollRect = GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "BagList");
        /**/
        HeadSlot = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "HeadSlot").gameObject, "Image");
        HeadGem1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "HeadGem1").gameObject, "Image");
        HeadGem2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "HeadGem2").gameObject, "Image");
        BodySlot = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "BodySlot").gameObject, "Image");
        BodyGem1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "BodyGem1").gameObject, "Image");
        BodyGem2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "BodyGem2").gameObject, "Image");
        WeaponSlot1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "WeaponSlot1").gameObject, "Image");
        Weapon1Gem1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "Weapon1Gem1").gameObject, "Image");
        Weapon1Gem2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "Weapon1Gem2").gameObject, "Image");
        WeaponSlot2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "WeaponSlot2").gameObject, "Image");
        Weapon2Gem1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "Weapon2Gem1").gameObject, "Image");
        Weapon2Gem2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "Weapon2Gem2").gameObject, "Image");
        Magic1 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "MagicSlot1").gameObject, "Image");
        Magic2 = GameTool.GetTheChildComponent<Image>(GameTool.FindTheChild(this.gameObject, "MagicSlot2").gameObject, "Image");
        Btn_Close.onClick.AddListener(CloseBagUI);

        BagItem = new List<GameObject>();
        PickUpList = new List<SingleItemInfo>();
        EquippedList = new List<SingleItemInfo>();

        MessageCenter.AddListener(EMessageType.GetAndSetBagList, (object obj) =>
        {
            PickUpList = (List<SingleItemInfo>)obj;
            SetBagListItems(PickUpList);
        });
        MessageCenter.AddListener(EMessageType.GetAndSetEquippedList, (object obj) =>
        {
            EquippedList = (List<SingleItemInfo>)obj;
            SetEquippedItems(EquippedList);
        });

        MessageCenter.AddListener(EMessageType.RefreshBagList, RefreshBagList);
       
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.BagUI;
        HeadSlot.sprite = ResourcesManager.Instance.GetUITexture("Null");
        HeadGem1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        HeadGem2.sprite = ResourcesManager.Instance.GetUITexture("Null");
        BodySlot.sprite = ResourcesManager.Instance.GetUITexture("Null");
        BodyGem1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        BodyGem2.sprite = ResourcesManager.Instance.GetUITexture("Null");
        WeaponSlot1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Weapon1Gem1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Weapon1Gem2.sprite = ResourcesManager.Instance.GetUITexture("Null");
        WeaponSlot2.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Weapon2Gem1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Weapon2Gem2.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Magic1.sprite = ResourcesManager.Instance.GetUITexture("Null");
        Magic2.sprite = ResourcesManager.Instance.GetUITexture("Null");
    }

    protected override void InitInStart()
    {
        InitBagItem(scrollRect.content);
        SetBagListItems(PickUpList);
        SetEquippedItems(EquippedList);

    }


    /// <summary>
    /// 刷新背包列表中 列表项的下标
    /// </summary>
    /// <param name="obj"></param>
    private void RefreshBagList(object obj)
    {
        for (int i = 0; i < BagItem.Count; i++)
        {
            BagItem[i].SetActive(false);
        }
        SetBagListItems(PickUpList);

    }
    /// <summary>
    /// 设置列表物件显示
    /// </summary>
    /// <param name="PickUpList"></param>
    /// <param name="EquippedList"></param>
    private void SetBagListItems(List<SingleItemInfo> PickUpList)
    {
        BagListitem bagListitem = null;
        if (PickUpList.Count > 0)
        {
            for (int i = 0; i < PickUpList.Count; i++)
            {
                BagItem[i].SetActive(true);
                bagListitem = BagItem[i].GetComponent<BagListitem>();
                bagListitem.Item = PickUpList[i];
                bagListitem.SetInfo(PickUpList[i].ItemName, ResourcesManager.Instance.GetUITexture(PickUpList[i].ResName), gameObject);
                bagListitem.Index = i;
                bagListitem.img.color = Color.white;
            }
        }

    }
    private void SetEquippedItems(List<SingleItemInfo> EquippedList)
    {
        if (EquippedList.Count > 0)
        {
            if (EquippedList[(int)EquippedItem.Head] != null)
            {
                HeadSlot.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Head].ResName);
            }
            if (EquippedList[(int)EquippedItem.HeadGem1] != null)
            {
                HeadGem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.HeadGem1].ResName);
            }
            if (EquippedList[(int)EquippedItem.HeadGem2] != null)
            {
                HeadGem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.HeadGem2].ResName);
            }
            if (EquippedList[(int)EquippedItem.Body] != null)
            {
                BodySlot.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Body].ResName);
            }
            if (EquippedList[(int)EquippedItem.BodyGem1] != null)
            {
                BodyGem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.BodyGem1].ResName);
            }
            if (EquippedList[(int)EquippedItem.BodyGem2] != null)
            {
                BodyGem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.BodyGem2].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon1] != null)
            {
                WeaponSlot1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon1].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon1_Gem1] != null)
            {
                Weapon1Gem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon1_Gem1].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon1_Gem2] != null)
            {
                Weapon1Gem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon1_Gem2].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon2] != null)
            {
                WeaponSlot1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon2].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon2_Gem1] != null)
            {
                Weapon2Gem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon2_Gem1].ResName);
            }
            if (EquippedList[(int)EquippedItem.Weapon2_Gem2] != null)
            {
                Weapon2Gem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[(int)EquippedItem.Weapon2_Gem2].ResName);
            }

        }
    }
    /// <summary>
    /// 实例化BagItem，加入列表
    /// </summary>
    /// <param name="parent"></param>
    private void InitBagItem(Transform parent)
    {
        GameObject Loaded = Resources.Load("UIPrefab/BagItem") as GameObject;
        GameObject tmp;
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(Loaded, parent);
            tmp.GetComponent<BagListitem>().cv = gameObject;

            BagItem.Add(tmp);
            tmp.gameObject.SetActive(false);
        }
    }

    private void HideBagItem(int index)
    {
        if (BagItem[index] != null)
        {
            BagItem[index].SetActive(false);
        }
    }

    private void CloseBagUI()
    {
        UIManager.Instance.HideTheUI(UIid.BagUI,()=> { });
    }
}
