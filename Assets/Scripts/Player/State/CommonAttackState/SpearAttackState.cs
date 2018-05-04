using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;

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
        if (FSM.Attacked)
        {
            foreach (KeyValuePair<string, NetworkPlayer> enemy in FSM.controller.ScenePlayers)
            {
                if (FSM.controller.CheckCanAttack(FSM.gameObject, enemy.Value.gameObject, attackDistance, 45))
                {
                    NetworkManager.SendPlayerHitSomeone(enemy.Value.name, FSM.characterStatus.Attack_Physics * (1 - enemy.Value.status.Defend_Physics / 100));

                    AudioManager.PlaySound2D("Spear").Play();
                }
            }
            FSM.animationManager.PlayAnimation("Spear Melee Attack 02");
            FSM.Attacked = false;
        }




    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM,
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
        CharacterMessageDispatcher.Instance.DispatchMesssage
    (FSMTransitionType.CanBeMove,
    FSM,
    (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5 || Mathf.Abs(Input.GetAxis("Vertical")) > 0.5) && FSM.controller.CC.isGrounded
    );
    }

}
