using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ChoshimArrowState : FSMState {

    public ChoshimArrowState()
    {
        stateID = FSMStateType.ChoshimArrow;
    }

    public override void Act(BaseFSM FSM)
    {
 
            FSM.PlayAnimation("Magic Shoot Attack");
            GameObject go = NetPoolManager.Instantiate("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);

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
