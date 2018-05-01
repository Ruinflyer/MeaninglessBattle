using UnityEngine;
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
    private ScrollRect equipScrollRect=null;

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
    private List<GameObject> BagItem=new List<GameObject>();
    [SerializeField]
    private List<GameObject> EquipItem=new List<GameObject>();

    protected override void InitUiOnAwake()
    {
        Btn_Close = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_Close");
        scrollRect = GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "BagList");
        equipScrollRect= GameTool.GetTheChildComponent<ScrollRect>(this.gameObject, "EquipList");
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
        EquipItem = new List<GameObject>();



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
        InitBagItem(scrollRect.content, BagItem);
        InitBagItem(equipScrollRect.content, EquipItem);
        //SetEquippedItems();
        SetBagListItems(BagManager.Instance.List_PickUp, BagItem);
        SetBagListItems(BagManager.Instance.List_Equip, EquipItem);
    }

    protected override void InitInStart()
    {
        
    }

    protected override void OnEnable()
    {
        RefreshBagList(BagManager.Instance.List_PickUp);
        RefreshBagList(BagManager.Instance.List_Equip);
    }

    private void Update()
    {
        //SetEquippedItems(BagManager.Instance.Dict_Equipped);
        
    }


    /// <summary>
    /// 刷新背包列表中 列表项的下标
    /// </summary>
    /// <param name="obj"></param>
    public void RefreshBagList(object obj)
    {
        for (int i = 0; i < BagItem.Count; i++)
        {
            BagItem[i].SetActive(false);
        }
        for (int i = 0; i < EquipItem.Count; i++)
        {
            EquipItem[i].SetActive(false);
        }
        SetBagListItems(BagManager.Instance.List_PickUp,BagItem);
        SetBagListItems(BagManager.Instance.List_Equip, EquipItem);
    }
    /// <summary>
    /// 设置列表物件显示
    /// </summary>
    /// <param name="PickUpList"></param>
    /// <param name="EquippedList"></param>
    private void SetBagListItems(List<SingleItemInfo> PickUpList,List<GameObject> list)
    {
        BagListitem bagListitem = null;

        
            for (int i = 0; i < PickUpList.Count; i++)
            {
                list[i].SetActive(true);
                bagListitem = list[i].GetComponent<BagListitem>();
                bagListitem.Item = PickUpList[i];
                bagListitem.SetInfo(PickUpList[i].ItemName, ResourcesManager.Instance.GetUITexture(PickUpList[i].ResName), gameObject);
                bagListitem.Index = i;
                bagListitem.img.color = Color.white;
            }
        
    }




    private void SetEquippedItems(Dictionary<EquippedItem,SingleItemInfo> EquippedList)
    {

        if (EquippedList.Count > 0)
        {
            if (EquippedList[EquippedItem.Head] != null)
            {
                HeadSlot.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Head].ResName);
            }
            if (EquippedList[EquippedItem.HeadGem1] != null)
            {
                HeadGem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.HeadGem1].ResName);
            }
            if (EquippedList[EquippedItem.HeadGem2] != null)
            {
                HeadGem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.HeadGem2].ResName);
            }
            if (EquippedList[EquippedItem.Body] != null)
            {
                BodySlot.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Body].ResName);
            }
            if (EquippedList[EquippedItem.BodyGem1] != null)
            {
                BodyGem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.BodyGem1].ResName);
            }
            if (EquippedList[EquippedItem.BodyGem2] != null)
            {
                BodyGem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.BodyGem2].ResName);
            }
            if (EquippedList[EquippedItem.Weapon1] != null)
            {
                WeaponSlot1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon1].ResName);
            }
            if (EquippedList[EquippedItem.Weapon1_Gem1] != null)
            {
                Weapon1Gem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon1_Gem1].ResName);
            }
            if (EquippedList[EquippedItem.Weapon1_Gem2] != null)
            {
                Weapon1Gem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon1_Gem2].ResName);
            }
            if (EquippedList[EquippedItem.Weapon2] != null)
            {
                WeaponSlot2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon2].ResName);
            }
            if (EquippedList[EquippedItem.Weapon2_Gem1] != null)
            {
                Weapon2Gem1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon2_Gem1].ResName);
            }
            if (EquippedList[EquippedItem.Weapon2_Gem2] != null)
            {
                Weapon2Gem2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Weapon2_Gem2].ResName);
            }

            if (EquippedList[EquippedItem.Magic1] != null)
            {
                Magic1.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic1].ResName);
            }
            if (EquippedList[EquippedItem.Magic2] != null)
            {
                Magic2.sprite = ResourcesManager.Instance.GetUITexture(EquippedList[EquippedItem.Magic2].ResName);
            }

        }
    }
    /// <summary>
    /// 实例化BagItem，加入列表
    /// </summary>
    /// <param name="parent"></param>
    private void InitBagItem(Transform parent,List<GameObject> list)
    {
        GameObject Loaded = Resources.Load("UIPrefab/BagItem") as GameObject;
        GameObject tmp;
        for (int i = 0; i < 10; i++)
        {
            tmp = Instantiate(Loaded, parent);
            tmp.GetComponent<BagListitem>().cv = gameObject;

            list.Add(tmp);
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
        //UIManager.Instance.HideTheUI(UIid.BagUI,()=> { });
        UIManager.Instance.ReturnUI(UIid.HUDUI);
        CameraBase.Instance.isFollowing = true;
    }
}
