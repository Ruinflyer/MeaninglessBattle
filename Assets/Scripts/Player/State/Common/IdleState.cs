using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using UnityEngine.EventSystems;

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
        FSM.controller.Wings.gameObject.SetActive(false);
        FSM.controller.Gravity = 9.8f;
    }
    /*
    public override void Reason(BaseFSM FSM)
    {
        
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.CanBeMove,
            FSM,
            (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5)
            );

        
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.AttackWithSingleWield,
            FSM,
            !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && (FSM.characterStatus.weaponType == WeaponType.Sword || FSM.characterStatus.weaponType == WeaponType.Club)
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.AttackWithDoubleHands,
            FSM,
             !EventSystem.current.IsPointerOverGameObject()&&Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.AttackWithSpear,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Spear
            );


        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingRipple,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Ripple
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingHeartAttack,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.HeartAttack
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingStygianDesolator,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.StygianDesolator
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingChoshimArrow,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.ChoshimArrow
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.UsingThunderBolt,
            FSM,
             !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Thunderbolt
            );

        CharacterMessageDispatcher.Instance.DispatchMesssage
          (FSMTransitionType.UsingIceArrow,
          FSM,
           !EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.IceArrow
          );

        CharacterMessageDispatcher.Instance.DispatchMesssage
           (
            FSMTransitionType.Falling,
            FSM,
            FSM.transform.position.y > 20
            );



        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonUp("PickUp") && FSM.isFound)
        {
            FSM.picked = true;
            FSM.PerformTransition(FSMTransitionType.CanPickUp);
        }
    }
    */
    
    public override void Reason(BaseFSM FSM)
    {
       

        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5))
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
      

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonUp("PickUp") && FSM.isFound)
        {
            FSM.picked = true;
            FSM.PerformTransition(FSMTransitionType.CanPickUp);
        }


        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && (FSM.characterStatus.weaponType == WeaponType.Sword || FSM.characterStatus.weaponType == WeaponType.Club))
        {
            FSM.Attacked = true;
            
            FSM.PerformTransition(FSMTransitionType.AttackWithSingleWield);

        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithDoubleHands);
        }


        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Ripple)
        {
            FSM.PerformTransition(FSMTransitionType.UsingRipple);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.HeartAttack)
        {
            FSM.PerformTransition(FSMTransitionType.UsingHeartAttack);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.StygianDesolator)
        {
            FSM.PerformTransition(FSMTransitionType.UsingStygianDesolator);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.IceArrow)
        {

            FSM.PerformTransition(FSMTransitionType.UsingIceArrow);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.ChoshimArrow)
        {
            FSM.PerformTransition(FSMTransitionType.UsingChoshimArrow);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.NULL && FSM.characterStatus.magicType == MagicType.Thunderbolt)
        {
            FSM.PerformTransition(FSMTransitionType.UsingThunderBolt);
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.Spear)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithSpear);
        }
        if (FSM.transform.position.y > 10)
        {
            FSM.PerformTransition(FSMTransitionType.Falling);
        }
    }
    
}
