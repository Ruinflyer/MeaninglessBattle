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
       
        if (BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.Thunderbolt)
        {
            if (BagManager.Instance.skillAttributesList[0].isOn)
            {
                BagManager.Instance.UseMagic(0);
                GameObject go = NetPoolManager.Instantiate("ThunderBolt", hitpoint, FSM.transform.rotation);
            }
        }
        else if (BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.Thunderbolt)
        {
            if (BagManager.Instance.skillAttributesList[1].isOn)
            {
                BagManager.Instance.UseMagic(1);
                GameObject go = NetPoolManager.Instantiate("ThunderBolt", hitpoint, FSM.transform.rotation);
            }
        }
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
