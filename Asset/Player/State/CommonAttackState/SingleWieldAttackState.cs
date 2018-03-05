using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SingleWieldAttackState : FSMState
{
    

    public SingleWieldAttackState()
    {
        stateID = FSMStateType.SingleWieldAttack;
    }

    public override void Act(BaseFSM FSM)
    {
       
        if (FSM.Attacked)
        {
            if (!FSM.animationManager.attackStateInfo.IsName("Melee Right Attack 01") && FSM.comboCount == 0 && FSM.animationManager.baseStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 1);
                FSM.comboCount = 1;
            }
            else if (FSM.animationManager.attackStateInfo.IsName("Melee Right Attack 01") && FSM.comboCount == 1 && FSM.animationManager.attackStateInfo.normalizedTime > 0.5F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 2);
                FSM.comboCount = 2;

            }
            else if (FSM.animationManager.attackStateInfo.IsName("Melee Right Attack 02") && FSM.comboCount == 2 && FSM.animationManager.attackStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 3);
                FSM.comboCount = 3;
            }
        }
        FSM.Attacked = false;
    }

    public override void Reason(BaseFSM FSM)
    {
        if (!FSM.animationManager.attackStateInfo.IsName("Idle") && FSM.animationManager.attackStateInfo.normalizedTime >1.15f)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }
        if (Input.GetButtonDown("Fire1")&&(FSM.characterStatus.weaponType==WeaponType.Sword|| FSM.characterStatus.weaponType == WeaponType.Club))
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithSingleWield);
        }
        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
    }
}
