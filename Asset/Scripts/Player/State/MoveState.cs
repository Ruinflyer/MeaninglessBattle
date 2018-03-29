using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class MoveState : FSMState
{
    public MoveState()
    {
        stateID = FSMStateType.Move;
    }


    public override void Act(BaseFSM FSM)
    {
        FSM.PlayAnimation("Run");
        FSM.controller.Move(FSM.characterStatus.moveSpeed);
    }

    public override void Reason(BaseFSM FSM)
    {

        if (Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5 && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.5&& FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }
        if(Input.GetButton("Jump"))
        {
            FSM.PerformTransition(FSMTransitionType.CanBeJump);
        }

        if (Input.GetButtonDown("Fire1") && (FSM.characterStatus.weaponType == WeaponType.Sword || FSM.characterStatus.weaponType == WeaponType.Club))
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithSingleWield);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithDoubleHands);
        }


    }
}
