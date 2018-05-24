using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ChoshimArrowState : FSMState
{

    public ChoshimArrowState()
    {
        stateID = FSMStateType.ChoshimArrow;
    }

    public override void Act(BaseFSM FSM)
    {
        if (BagManager.Instance.skillAttributesList[0].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.ChoshimArrow)
            {
                if (BagManager.Instance.skillAttributesList[0].isOn)
                {
                    BagManager.Instance.UseMagic(0);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    GameObject go = NetPoolManager.Instantiate("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
                    AudioManager.PlaySound2D("Arrow").Play();
                    go.GetComponent<MagicBehaviour>().isHit = true;
                    NetworkManager.SendPlayerMagic("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
                }

            }
    
        if (BagManager.Instance.skillAttributesList[1].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.ChoshimArrow)
            {
                if (BagManager.Instance.skillAttributesList[1].isOn)
                {
                    BagManager.Instance.UseMagic(1);
                    FSM.PlayAnimation("Magic Shoot Attack");
                    GameObject go = NetPoolManager.Instantiate("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
                    AudioManager.PlaySound2D("Arrow").Play();
                    go.GetComponent<MagicBehaviour>().isHit = true;
                    NetworkManager.SendPlayerMagic("Choshim Arrow", GameTool.FindTheChild(FSM.gameObject, "RigLArmPalmGizmo").position, FSM.transform.rotation);
                }
            }
    }

    public override void Reason(BaseFSM FSM)
    {
        CharacterMessageDispatcher.Instance.DispatchMesssage
            (FSMTransitionType.IsIdle,
            FSM,
            FSM.animationManager.baseStateInfo.IsName("Idle")
            );
    }
}
