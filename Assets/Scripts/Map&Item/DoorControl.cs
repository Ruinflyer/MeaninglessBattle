using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public string DoorOpenAnimationName = null;
    public string DoorCloseAnimationName = null;
    private bool EnterDoorAera = false;
    private bool isDoorOpen = false;
    public int DoorID;
    private Animation anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            if (EnterDoorAera)
            {
                NetworkManager.SendDoorOpen(DoorID);
            }
        }
       
    }

    /// <summary>
    /// 操控门
    /// </summary>
    public void ControlDoor()
    {
        if (isDoorOpen == false)
        {
            OpenDoor();

        }
        else
        {
            CloseDoor();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        EnterDoorAera = true;
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        EnterDoorAera = false;
    }

    private void OpenDoor()
    {
        anim.CrossFade(DoorOpenAnimationName);
        isDoorOpen = true;
    }

    private void CloseDoor()
    {
        anim.CrossFade(DoorCloseAnimationName);
        isDoorOpen = false;
    }


}
