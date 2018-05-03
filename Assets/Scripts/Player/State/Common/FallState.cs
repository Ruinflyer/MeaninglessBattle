using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class FallState :  FSMState
{

    public FallState()
    {
        stateID = FSMStateType.Fall;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Falling");
        FSM.controller.Gravity = 20;
        FSM.controller.Wings.gameObject.SetActive(true);
        FSM.controller.FallingCtrl(5f);
       
    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage(
            FSMTransitionType.IsIdle,
            FSM,
            FSM.transform.position.y <= 10
            );
          
    }

}
