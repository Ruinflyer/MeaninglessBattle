using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MainUIRoomListItem : MonoBehaviour,IPointerClickHandler {

    public UnityEvent DoubleClickEvent = new UnityEvent();
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount==2)
        {
            DoubleClickEvent.Invoke();
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
