using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SpearAttackState : FSMState
{
    private float attackDistance = 0;

    public SpearAttackState()
    {
        stateID = FSMStateType.SpearAttack;
    }

    public override void Act(BaseFSM FSM)
    {

        if (FSM.controller.GetCurSelectedWeaponInfo() != null)
        {
            attackDistance = FSM.controller.GetCurSelectedWeaponInfo().weaponProperties.weaponLength;
        }

        foreach (NetworkPlayer enemy in FSM.controller.List_Enemy)
        {
            if (FSM.controller.CheckCanAttack(FSM.gameObject, enemy.gameObject, attackDistance, 45))
            {
                //单机测试
                enemy.playerFSM.characterStatus.HP -= FSM.characterStatus.Attack_Physics * (1 - enemy.playerFSM.characterStatus.Defend_Physics / 100);
            }
        }
        FSM.animationManager.PlayAnimation("Spear Melee Attack 02");


    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM.GetComponent<NetworkPlayer>(),
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
    }

}
