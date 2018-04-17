using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SpearIdleState : FSMState
{
    public SpearIdleState()
    {
        stateID = FSMStateType.SpearIdle;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.animationManager.PlayAnimation("Spear Idle");
        FSM.comboCount = 0;
    }

    public override void Reason(BaseFSM FSM)
    {
        if (FSM.characterStatus.weaponType != WeaponType.Spear)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }

        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded && FSM.characterStatus.weaponType == WeaponType.Spear)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMoveWithSpear);
        }
        if (Input.GetButtonDown("Fire1") && (FSM.characterStatus.weaponType == WeaponType.Spear))
        {
            FSM.PerformTransition(FSMTransitionType.AttackWithSpear);
        }
    }
}
