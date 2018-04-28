using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class DefendState : FSMState {

	public DefendState()
    {
        stateID = FSMStateType.Defend;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Defend");

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
