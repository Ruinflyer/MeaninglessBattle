using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class StygianDesolatorState : FSMState
{
    internal NetPoolManager NetPoolManager = new NetPoolManager();

    public StygianDesolatorState()
    {
        stateID = FSMStateType.StygianDesolator;
    }

    public override void Act(BaseFSM FSM)
    {
        if (FSM.Attacked)
        {
            FSM.PlayAnimation("Spin Attack");
            GameObject go = NetPoolManager.Instantiate("Stygian Desolator", GameTool.FindTheChild(FSM.gameObject, "RigPelvisGizmo").position, FSM.transform.rotation);
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
