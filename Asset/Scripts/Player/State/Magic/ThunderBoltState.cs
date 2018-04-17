using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ThunderBoltState : FSMState {

    Vector3 hitpoint;

    public ThunderBoltState()
    {
        stateID = FSMStateType.ThunderBolt;
    }

    public override void Act(BaseFSM FSM)
    {
        MessageCenter.AddListener(EMessageType.GetHitPoint, (object obj) => { hitpoint = (Vector3)obj; });
        if (FSM.Attacked)
        {
            GameObject go = NetPoolManager.Instantiate("ThunderBolt", hitpoint, FSM.transform.rotation);
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
