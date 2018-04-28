using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class UseItemState : FSMState
{
    private int itemID;

    private UseItemState()
    {
        stateID = FSMStateType.UseItem;
    }

    public override void Act(BaseFSM FSM)
    {
        MessageCenter.AddListener(EMessageType.UseItem, (object obj) => { itemID = (int)obj; });
        FSM.GetComponent<PlayerBag>().UseItem(itemID);
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
