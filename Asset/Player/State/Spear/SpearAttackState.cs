using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class SpearAttackState :  FSMState
{
    public override void Act(BaseFSM FSM)
    {
        if (FSM.Attacked)
            FSM.animationManager.PlayAnimation("Spear Melee Attack 02");
        FSM.Attacked = false;
    }

    public override void Reason(BaseFSM FSM)
    {
        if(!FSM.animationManager.baseStateInfo.IsName("Spear Idle") &&FSM.animationManager.baseStateInfo.normalizedTime>1f)
        {
            FSM.PerformTransition(FSMTransitionType.IsIdleWithSpear);
        }
    }

}
