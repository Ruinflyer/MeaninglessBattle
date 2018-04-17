using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public string DoorOpenAnimationName = null;
    public string DoorCloseAnimationName = null;
    private bool EnterDoorAera = false;
    private bool isDoorOpen = false;
    private Animation anim;
    private PhotonView photonView;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && EnterDoorAera)
        {
            if (photonView == null)
            {
                photonView = GetComponent<PhotonView>();
            }

            if (isDoorOpen == false)
            {
                OpenDoor();

                photonView.RPC("OpenDoor", PhotonTargets.Others);
            }
            else
            {
                CloseDoor();
                photonView.RPC("CloseDoor", PhotonTargets.Others);
            }

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

    [PunRPC]
    public void OpenDoor()
    {
        anim.CrossFade(DoorOpenAnimationName);
        isDoorOpen = true;
    }
    [PunRPC]
    public void CloseDoor()
    {
        anim.CrossFade(DoorCloseAnimationName);
        isDoorOpen = false;
    }

   
}
