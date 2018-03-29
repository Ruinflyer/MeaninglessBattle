using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class JumpState : FSMState
{
    public JumpState()
    {
        stateID = FSMStateType.Jump;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Jump");
        FSM.controller.Jump(FSM.characterStatus.jumpSpeed);
    }

    public override void Reason(BaseFSM FSM)
    {
        if((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }

        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5)&& FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
    }

}
