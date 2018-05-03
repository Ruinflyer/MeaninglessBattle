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
        FSM.controller.Move(FSM.characterStatus.moveSpeed, FSM.characterStatus.jumpSpeed);

    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
        (FSMTransitionType.IsIdle,
        FSM,
        Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5 && Mathf.Abs(Input.GetAxis("Vertical")) <= 0.5
        );


        CharacterMessageDispatcher.Instance.DispatchMesssage
        (FSMTransitionType.AttackWithSingleWield,
        FSM,
        Input.GetButtonDown("Fire1") && (FSM.characterStatus.weaponType == WeaponType.Sword || FSM.characterStatus.weaponType == WeaponType.Club)
        );

        CharacterMessageDispatcher.Instance.DispatchMesssage
        (FSMTransitionType.AttackWithDoubleHands,
        FSM,
        Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands
        );

        CharacterMessageDispatcher.Instance.DispatchMesssage
        (FSMTransitionType.AttackWithSpear,
        FSM,
        Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Spear
        );


    }
}
