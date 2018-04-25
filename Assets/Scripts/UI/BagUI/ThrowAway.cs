using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowAway : MonoBehaviour, IDropHandler
{


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<DropItem>() != null)
            {
                eventData.pointerDrag.GetComponent<DropItem>().ThrowAway();
                MessageCenter.Send(Meaningless.EMessageType.RefreshBagList, null);
            }

            if(eventData.pointerDrag.GetComponent<BagListitem>() != null)
            {
                eventData.pointerDrag.GetComponent<BagListitem>().ThrowAway();
                MessageCenter.Send(Meaningless.EMessageType.RefreshBagList, null);
            }
        }
    }
}