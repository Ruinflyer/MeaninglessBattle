using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

    public float minDistnce = 1f;
    public float maxDistance = 5f;
    public float smooth = 10;
    public float distance;
    private Vector3 dir;
    RaycastHit hit;
    Ray rayToItem;
    RaycastHit hitToItem;

    public Transform itemTran=null;

    private void Awake()
    {
        dir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        if (Physics.Linecast(transform.parent.position, transform.parent.TransformPoint(dir * maxDistance), out hit))
        {
            distance = Mathf.Clamp(hit.distance * 0.9f, minDistnce, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dir * distance, Time.deltaTime * smooth);

        rayToItem = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayToItem, out hitToItem))
        {
            float distance = Vector3.Distance(rayToItem.origin, hitToItem.point);
            if (hitToItem.collider != null)
            {
                if(distance<15)
                {
                    if (distance <= 5)
                    {
                        if (hitToItem.collider.GetComponent<GroundItem>() != null)
                        {
                            MessageCenter.Send(Meaningless.EMessageType.FoundItem, true);
                            itemTran = hitToItem.collider.transform;

                        }
                        else
                        {
                            MessageCenter.Send(Meaningless.EMessageType.FoundItem, false);
                            itemTran = null;
                        }
                    }
                    else
                    {
                        MessageCenter.Send(Meaningless.EMessageType.FoundItem, false);
                        itemTran = null;
                    }
                    
                }
          
            }
            else
            {
                MessageCenter.Send(Meaningless.EMessageType.FoundItem, false);
                itemTran = null;
            }
        }
        else
        {
            MessageCenter.Send(Meaningless.EMessageType.FoundItem, false);
            itemTran = null;
        }

       
    }
}
