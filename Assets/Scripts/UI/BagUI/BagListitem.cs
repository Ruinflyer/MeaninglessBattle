using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 背包物件，可被拖动
/// </summary>
public class BagListitem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Image img;
    public Text txt;
    public GameObject cv;
    public SingleItemInfo Item;
    public int Index;

    public Slider PressedSlider;
    private GameObject dragObj;
    private RectTransform rectTransform;

    //开始按下的时间
    private float timeBeginpressed;
    //指针按下flag
    private bool isPointerDown = false;
    //是否长时间按下flag
    private bool isLongPressed;

    // Use this for initialization
    void Start()
    {
        img = GameTool.FindTheChild(gameObject, "ItemImage").GetComponent<Image>();
        txt = GameTool.FindTheChild(gameObject, "ItemName").GetComponent<Text>();
        PressedSlider = GameTool.FindTheChild(gameObject, "PressedSlider").GetComponent<Slider>();

    }

    void Update()
    {
        if (isPointerDown == true && isLongPressed == false)
        {
            PressedSlider.value = Time.time - timeBeginpressed;
            if (Time.time - timeBeginpressed > 1.0f)
            {
                isLongPressed = true;
                if (Item.itemType == ItemType.Expendable)
                {
                    UseItem();

                }

            }
        }
    }
    public void SetInfo(string ItemName, Sprite ItemSprite, GameObject canvas)
    {
        txt.text = ItemName;
        img.sprite = ItemSprite;
        cv = canvas;
        rectTransform = cv.transform as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item.itemType == ItemType.Gem || Item.itemType == ItemType.Armor || Item.itemType == ItemType.Magic || Item.itemType == ItemType.Weapon)
        {
            dragObj = new GameObject("Dragitem");
            dragObj.transform.SetParent(cv.transform);
            dragObj.transform.SetAsLastSibling();

            Image tmp_Img = dragObj.AddComponent<Image>();
            tmp_Img.raycastTarget = false;
            tmp_Img.sprite = img.sprite;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item.itemType == ItemType.Expendable)
        {
            timeBeginpressed = Time.time;
            isPointerDown = true;
            isLongPressed = false;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressed = false;
        isPointerDown = false;
        PressedSlider.value = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void Equip()
    {
        PressedSlider.value = 0;
        txt.text = "";
        img.sprite = null;
        BagManager.Instance.List_Equip.RemoveAt(Index);
        gameObject.SetActive(false);
        Destroy(dragObj);
    }

    public void UseItem()
    {
        MessageCenter.Send(Meaningless.EMessageType.UseItem, Index);
        PressedSlider.value = 0;
        txt.text = "";
        img.sprite = null;
        gameObject.SetActive(false);
        Destroy(dragObj);
    }

    public void ThrowAway()
    {
        if (Item.itemType == ItemType.Gem || Item.itemType == ItemType.Expendable)
            BagManager.Instance.List_PickUp.RemoveAt(Index);
        else
            BagManager.Instance.List_Equip.RemoveAt(Index);
        CameraBase.Instance.player.GetComponent<PlayerController>().DiscardItem(Item.ItemID);
        PressedSlider.value = 0;
        txt.text = "";
        img.sprite = null;
        Item = null;
        gameObject.SetActive(false);
        Destroy(dragObj);
    }

}
