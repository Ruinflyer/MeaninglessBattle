using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class IdleState : FSMState
{

    public IdleState()
    {
        stateID = FSMStateType.Idle;
    }

    public override void Act(BaseFSM FSM)
    {
        FSM.animationManager.PlayIdle();
        FSM.comboCount = 0;
    }

    public override void Reason(BaseFSM FSM)
    {


        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5)&&FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
        if (Input.GetButton("Jump"))
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

        if(Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Magic&&FSM.characterStatus.magicType==MagicType.Ripple)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingRipple);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Magic && FSM.characterStatus.magicType == MagicType.HeartAttack)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingHeartAttack);
        }

        if(Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Magic && FSM.characterStatus.magicType == MagicType.StygianDesolator)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingStygianDesolator);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Magic && FSM.characterStatus.magicType == MagicType.IceArrow)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingIceArrow);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Magic && FSM.characterStatus.magicType == MagicType.ChoshimArrow)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingChoshimArrow);
        }

        if (FSM.characterStatus.weaponType == WeaponType.Spear)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdleWithSpear);
        }
    }
}
