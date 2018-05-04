using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class DoubleHandsAttackState : FSMState
{

    private float attackDistance = 0;

    public DoubleHandsAttackState()
    {
        stateID = FSMStateType.DoubleHandsAttack;
    }

    public override void Act(BaseFSM FSM)
    {


        if (FSM.controller.GetCurSelectedWeaponInfo() != null)
        {
            attackDistance = FSM.controller.GetCurSelectedWeaponInfo().weaponProperties.weaponLength;
        }
        if (FSM.Attacked)
        {
            foreach (KeyValuePair<string, NetworkPlayer> enemy in FSM.controller.ScenePlayers)
            {
                if (FSM.controller.CheckCanAttack(FSM.gameObject, enemy.Value.gameObject, attackDistance, 45))
                {
                    NetworkManager.SendPlayerHitSomeone(enemy.Value.name, FSM.characterStatus.Attack_Physics * (1 - enemy.Value.status.Defend_Physics / 100));
                    //单机测试
                    //enemy.playerFSM.characterStatus.HP -= FSM.characterStatus.Attack_Physics * (1 - enemy.playerFSM.characterStatus.Defend_Physics / 100);
                }
            }



            if (!FSM.animationManager.attackStateInfo.IsName("Right Punch Attack") && FSM.comboCount == 0 && FSM.animationManager.baseStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 4);
                FSM.comboCount = 1;
                if (FSM.characterStatus.weaponType == WeaponType.DoubleHands)
                {
                    AudioManager.PlaySound2D("Axe").Play();
                }
    
            }
            else if (FSM.animationManager.attackStateInfo.IsName("Right Punch Attack") && FSM.comboCount == 1 && FSM.animationManager.attackStateInfo.normalizedTime > 0.5F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 5);
                FSM.comboCount = 2;
                if (FSM.characterStatus.weaponType == WeaponType.DoubleHands)
                {
                    AudioManager.PlaySound2D("Axe").Play();
                }

            }
            else if (FSM.animationManager.attackStateInfo.IsName("Left Punch Attack") && FSM.comboCount == 2 && FSM.animationManager.attackStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 6);
                FSM.comboCount = 3;
                if (FSM.characterStatus.weaponType == WeaponType.DoubleHands)
                {
                    AudioManager.PlaySound2D("Axe").Play();
                }
            }
            else if (FSM.animationManager.attackStateInfo.IsName("Left Punch Attack") && FSM.comboCount == 3 && FSM.animationManager.attackStateInfo.normalizedTime > 0.4F)
            {
                FSM.animationManager.PlayAnimation("AttackID", 7);
                FSM.comboCount = 4;
                if (FSM.characterStatus.weaponType == WeaponType.DoubleHands)
                {
                    AudioManager.PlaySound2D("Axe").Play();
                }
            }
            FSM.Attacked = false;
        }
    }

    public override void Reason(BaseFSM FSM)
    {

        CharacterMessageDispatcher.Instance.DispatchMesssage
       (FSMTransitionType.IsIdle,
       FSM,
       FSM.animationManager.baseStateInfo.IsName("Idle") && FSM.animationManager.attackStateInfo.normalizedTime > 1.15f
       );

        if ( Input.GetButtonDown("Fire1") && FSM.characterStatus.weaponType == WeaponType.DoubleHands)
        {
            FSM.Attacked = true;
            FSM.PerformTransition(FSMTransitionType.AttackWithDoubleHands);
        }
        CharacterMessageDispatcher.Instance.DispatchMesssage
     (FSMTransitionType.CanBeMove,
     FSM,
     (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded
     );

    }
}
