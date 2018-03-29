using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class HeartAttackState : FSMState
{
    internal NetPoolManager NetPoolManager = new NetPoolManager();

    public HeartAttackState()
    {
        stateID = FSMStateType.HeartAttack;
    }

    public override void Act(BaseFSM FSM)
    {
        if (FSM.Attacked)
        {
            FSM.PlayAnimation("Magic Shoot Attack");
            GameObject go = NetPoolManager.Instantiate("Heart Attack", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
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
