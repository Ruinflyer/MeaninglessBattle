using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SpearMoveState : FSMState
{
    public SpearMoveState()
    {
        stateID = FSMStateType.SpearMove;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Spear Run");
        FSM.controller.Move(FSM.characterStatus.moveSpeed);
    }

    public override void Reason(BaseFSM FSM)
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5 && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.5 && FSM.controller.CC.isGrounded && FSM.characterStatus.weaponType == WeaponType.Spear)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdleWithSpear);
        }
    }
}
