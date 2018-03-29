﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DropItem : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public EquippedItem equippedItemType;
    private Image image;
    private GameObject dragObj;
    private RectTransform rectTransform;
    private GameObject cv;
    [SerializeField]
    public SingleItemInfo ItemInfo;
    private bool canDrag = false;
    private bool bagListFull = false;


    // Use this for initialization
    void Start()
    {
        image = GameTool.FindTheChild(gameObject, "Image").GetComponent<Image>();
        MessageCenter.AddListener(Meaningless.EMessageType.GetBagListFull, (object obj) => { bagListFull = (bool)obj; });
        cv = gameObject.transform.parent.gameObject;
        rectTransform = cv.transform as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnDrop(PointerEventData eventData)
    {
        if (canDrag == false)
        {
            if (eventData.pointerDrag != null)
            {
                BagListitem bagListitem = eventData.pointerDrag.GetComponent<BagListitem>();
                DropItem dropItem = eventData.pointerDrag.GetComponent<DropItem>();

                switch (equippedItemType)
                {
                    case EquippedItem.BodyGem1:
                    case EquippedItem.BodyGem2:
                    case EquippedItem.HeadGem1:
                    case EquippedItem.HeadGem2:
                    case EquippedItem.Weapon1_Gem1:
                    case EquippedItem.Weapon1_Gem2:
                    case EquippedItem.Weapon2_Gem1:
                    case EquippedItem.Weapon2_Gem2:
                        if (bagListitem != null)
                        {
                            if (bagListitem.Item.itemType == ItemType.Gem)
                            {
                                image.sprite = bagListitem.img.sprite;
                                object[] param = new object[2];

                                param[0] = equippedItemType;
                                param[1] = bagListitem.Item;
                                MessageCenter.Send_Multparam(Meaningless.EMessageType.EquipItem, param);

                                ItemInfo = bagListitem.Item;
                                bagListitem.UseItem();

                                canDrag = true;

                            }
                        }
                        if (dropItem != null)
                        {
                            if (dropItem.ItemInfo.itemType == ItemType.Gem)
                            {
                                image.sprite = dropItem.image.sprite;
                                MessageCenter.Send(Meaningless.EMessageType.UnEquipItem, dropItem.equippedItemType);

                                object[] param = new object[2];
                                param[0] = equippedItemType;
                                param[1] = dropItem.ItemInfo;
                                MessageCenter.Send_Multparam(Meaningless.EMessageType.EquipItem, param);

                                ItemInfo = dropItem.ItemInfo;
                                canDrag = true;
                                dropItem.DropedToOtherSlot();
                            }
                        }
                        break;
                }


            }
        }

    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {

            dragObj = new GameObject("Dragitem");
            dragObj.transform.SetParent(cv.transform);

            dragObj.transform.SetAsLastSibling();

            Image tmp_Img = dragObj.AddComponent<Image>();
            tmp_Img.raycastTarget = false;
            tmp_Img.sprite = image.sprite;
            dragObj.GetComponent<RectTransform>().sizeDelta = new Vector2(28f, 28f);
        }

    }
    public void OnDrag(PointerEventData eventData)
    {

        if (dragObj != null)
        {
            RectTransform rt = dragObj.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rt.position = globalMousePos;
                rt.rotation = rectTransform.rotation;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if (dragObj != null)
        {
            //拖拽结束后销毁
            Destroy(dragObj);

        }

    }

    public void DropedToList()
    {
        if (!bagListFull)
        {
            image.sprite = ResourcesManager.Instance.GetUITexture("Null");
            MessageCenter.Send(Meaningless.EMessageType.PickedupItem, ItemInfo.ItemID);
            MessageCenter.Send(Meaningless.EMessageType.UnEquipItem, equippedItemType);

            canDrag = false;
            ItemInfo = null;
        }
    }

    public void DropedToOtherSlot()
    {
        image.sprite = ResourcesManager.Instance.GetUITexture("Null");
        MessageCenter.Send(Meaningless.EMessageType.UnEquipItem, equippedItemType);

        canDrag = false;
        ItemInfo = null;
    }
}