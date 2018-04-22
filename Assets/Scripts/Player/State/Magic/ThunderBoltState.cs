using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ThunderBoltState : FSMState
{

    Vector3 hitpoint;

    public ThunderBoltState()
    {
        stateID = FSMStateType.ThunderBolt;
    }

    public override void Act(BaseFSM FSM)
    {
        MessageCenter.AddListener(EMessageType.GetHitPoint, (object obj) => { hitpoint = (Vector3)obj; });

        GameObject go = NetPoolManager.Instantiate("ThunderBolt", hitpoint, FSM.transform.rotation);

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
