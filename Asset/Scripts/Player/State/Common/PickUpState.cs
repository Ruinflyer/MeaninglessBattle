using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PickUpState : FSMState
{
    private int pickedupItemID = -1;

    public PickUpState()
    {
        stateID = FSMStateType.PickUp;
    }

    public override void Act(BaseFSM FSM)
    {
        MessageCenter.AddListener(EMessageType.PickedupItem, (object obj) => { pickedupItemID = (int)obj; });
        if (pickedupItemID != -1)
        {
            FSM.GetComponent<PlayerBag>().PickItem(pickedupItemID);
            FSM.PlayAnimation("Pick Up");
        }
        else
            Debug.LogError("拾起失败");
    }

    public override void Reason(BaseFSM FSM)
    {
        if (FSM.animationManager.baseStateInfo.IsName("Idle"))
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }
    }
}
