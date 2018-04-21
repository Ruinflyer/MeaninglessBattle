using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class StygianDesolatorState : FSMState
{
    //internal NetPoolManager NetPoolManager = new NetPoolManager();

    public StygianDesolatorState()
    {
        stateID = FSMStateType.StygianDesolator;
    }

    public override void Act(BaseFSM FSM)
    {

            FSM.PlayAnimation("Spin Attack");
            GameObject go = NetPoolManager.Instantiate("Stygian Desolator", GameTool.FindTheChild(FSM.gameObject, "RigPelvisGizmo").position, FSM.transform.rotation);

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
