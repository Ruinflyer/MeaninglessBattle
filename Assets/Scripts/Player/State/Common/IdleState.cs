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
        FSM.controller.Wings.gameObject.SetActive(false);
        FSM.controller.Gravity = 9.8f;
    }

    public override void Reason(BaseFSM FSM)
    {
        MessageCenter.AddListener(EMessageType.FoundItem, (object obj) => { isFound = (bool)obj; });
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.CanBeMove,
            FSM,
            (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5)||Input.GetButton("Jump")||Input.GetButton("Roll")
            );

        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.CanDefend,
            FSM,
            Input.GetButton("Defend") && FSM.characterStatus.magicType == MagicType.NULL && FSM.characterStatus.weaponType == WeaponType.Shield
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


        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingRipple,
            FSM,
            Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Ripple
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingHeartAttack,
            FSM,
            Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.HeartAttack
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingStygianDesolator,
            FSM,
            Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.StygianDesolator
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingChoshimArrow,
            FSM,
            Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.ChoshimArrow
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingThunderBolt,
            FSM,
            Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Thunderbolt
            );

        CharacterMessageDispatcher.Instance.DispatchMesssage
           (
            FSMTransitionType.Falling,
            FSM,
            FSM.transform.position.y > 20
            );



        if (Input.GetButtonUp("PickUp") && isFound)
        {
            FSM.picked = true;
            FSM.PerformTransition(FSMTransitionType.CanPickUp);
        }
    }

    /*
    public override void Reason(BaseFSM FSM)
    {
       

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
    */
}
