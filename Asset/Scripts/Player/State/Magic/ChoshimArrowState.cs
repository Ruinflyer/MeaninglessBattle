using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ChoshimArrowState : FSMState {

    internal NetPoolManager NetPoolManager = new NetPoolManager();

    public ChoshimArrowState()
    {
        stateID = FSMStateType.ChoshimArrow;
    }

    public override void Act(BaseFSM FSM)
    {
        if (FSM.Attacked)
        {
            FSM.PlayAnimation("Magic Shoot Attack");
            GameObject go = NetPoolManager.Instantiate("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
            Debug.Log(go.name);
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
