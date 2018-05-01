using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PickUpState : FSMState
{
    //private int pickedupItemID = -1;
    Transform pickUpItemTran = null;
    int pickedupItemID = -1;

    public PickUpState()
    {
        stateID = FSMStateType.PickUp;

    }

    public override void Act(BaseFSM FSM)
    {     
        if(FSM.picked)
        {
            pickUpItemTran = Camera.main.GetComponent<CameraCollision>().itemTran;
            if (pickUpItemTran != null)
                if (pickUpItemTran.GetComponent<GroundItem>() != null)
                {
                    pickedupItemID = pickUpItemTran.GetComponent<GroundItem>().ItemID;
                    SingleItemInfo ItemInfo;
                    ItemInfo = ItemInfoManager.Instance.GetItemInfo(pickedupItemID);
                    BagManager.Instance.PickItem(pickedupItemID);
                    FSM.GetComponent<PlayerController>().PickItem(pickUpItemTran);
                    FSM.animationManager.PlayAnimation("Pick Up");

                }
                else
                    Debug.LogError("拾起失败");
            else
                Debug.LogError("拾起失败");
        }
        FSM.picked = false;
    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM,
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
    }

}
