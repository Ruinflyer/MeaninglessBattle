using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class DoubleHandsAttackState : FSMState {


    public DoubleHandsAttackState()
    {
        stateID = FSMStateType.DoubleHandsAttack;
    }

    public override void Act(BaseFSM FSM)
    {

        if (FSM.Attacked)
        {
            if (!FSM.animationManager.attackStateInfo.IsName("Right Punch Attack") && FSM.comboCount == 0 && FSM.animationManager.baseStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 4);
                FSM.comboCount = 1;
            }
            else if (FSM.animationManager.attackStateInfo.IsName("Right Punch Attack") && FSM.comboCount == 1 && FSM.animationManager.attackStateInfo.normalizedTime > 0.5F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 5);
                FSM.comboCount = 2;

            }
            else if (FSM.animationManager.attackStateInfo.IsName("Left Punch Attack") && FSM.comboCount == 2 && FSM.animationManager.attackStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 6);
                FSM.comboCount = 3;
            }
            else if (FSM.animationManager.attackStateInfo.IsName("Left Punch Attack") && FSM.comboCount == 3 && FSM.animationManager.attackStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 7);
                FSM.comboCount = 4;
            }
        }
        FSM.Attacked = false;
    }

    public override void Reason(BaseFSM FSM)
    {
        if (!FSM.animationManager.attackStateInfo.IsName("Idle") && FSM.animationManager.attackStateInfo.normalizedTime > 1.15f)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }
        if (Input.GetButtonDown("Fire1")&&FSM.characterStatus.weaponType==WeaponType.DoubleHands)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithDoubleHands);
        }
        if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded)
        {
            FSM.PerformTransition(FSMTransitionType.CanBeMove);
        }
    }
}
