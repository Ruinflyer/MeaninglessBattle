using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class RippleAttackState : FSMState
{
    internal NetPoolManager NetPoolManager=new NetPoolManager();

    public RippleAttackState()
    {
        stateID = FSMStateType.RippleAttack;
    }

    public override void Act(BaseFSM FSM)
    {
        if(FSM.Attacked)
        {
            FSM.PlayAnimation("Magic Shoot Attack");

            NetPoolManager.Instantiate("Ripple", FSM.transform.position+new Vector3(0,1,2f), FSM.transform.rotation);
        }
        FSM.Attacked = false;
    }

    public override void Reason(BaseFSM FSM)
    {
        if (FSM.animationManager.baseStateInfo.IsName("Idle"))
        {
            FSM.PerformTransition(FSMTransitionType.IsIdle);
        }
    }
}
