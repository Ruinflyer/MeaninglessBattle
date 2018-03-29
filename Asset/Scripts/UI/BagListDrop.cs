using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BagListDrop : MonoBehaviour, IDropHandler
{


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<DropItem>() != null)
            {
                eventData.pointerDrag.GetComponent<DropItem>().DropedToList();
            }

        }
    }
}
