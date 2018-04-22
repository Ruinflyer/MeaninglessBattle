using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class IceArrowState : FSMState {


    public IceArrowState()
    {
        stateID = FSMStateType.IceArrow;
    }

    public override void Act(BaseFSM FSM)
    {

            FSM.PlayAnimation("Magic Shoot Attack");
            GameObject go = NetPoolManager.Instantiate("Ice Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
            Debug.Log(go.name);

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
