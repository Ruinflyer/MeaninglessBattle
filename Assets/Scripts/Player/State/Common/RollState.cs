using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class RollState : FSMState
{
    public RollState()
    {
        stateID = FSMStateType.Roll;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Roll");
    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (
            FSMTransitionType.IsIdle,
            FSM,
            FSM.animationManager.baseStateInfo.IsName("Idle")
        );
    }

}
