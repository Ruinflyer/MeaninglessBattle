using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class IdleState : FSMState
{
    public bool isFound;

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
        MessageCenter.AddListener(EMessageType.FoundItem, (object obj) => { isFound = (bool)obj; });

        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
        if (Input.GetButton("Jump"))
        {
            FSM.PerformTransition(FSMTransitionType.CanBeJump);
        }

        if(Input.GetButton("Defend")&&FSM.characterStatus.magicType==MagicType.NULL&&FSM.characterStatus.weaponType==WeaponType.Shield)
        {
            FSM.PerformTransition(FSMTransitionType.CanDefend);
        }

        if (Input.GetButtonDown("PickUp") && isFound)
        {
            FSM.PerformTransition(FSMTransitionType.CanPickUp);
        }


        if (Input.GetButton("Fire2"))
        {
            FSM.controller.CurrentSelected = 0;
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


        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Ripple)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingRipple);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.HeartAttack)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingHeartAttack);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.StygianDesolator)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingStygianDesolator);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.IceArrow)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingIceArrow);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.ChoshimArrow)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingChoshimArrow);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Thunderbolt)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.UsingThunderBolt);
        }

        if (Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Spear)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithSpear);
        }
    }
}
