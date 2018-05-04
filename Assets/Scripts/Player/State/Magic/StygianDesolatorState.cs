﻿using System.Collections;
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
        if (BagManager.Instance.skillAttributesList[0].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[0].skillInfo.magicProperties.magicType == MagicType.StygianDesolator)
            {
                if (BagManager.Instance.skillAttributesList[0].isOn)
                {
                    BagManager.Instance.UseMagic(0);
                    FSM.PlayAnimation("Spin Attack");
                    GameObject go = NetPoolManager.Instantiate("Stygian Desolator", GameTool.FindTheChild(FSM.gameObject, "RigPelvisGizmo").position, FSM.transform.rotation);
                    AudioManager.PlaySound2D("Stygian Desolator").Play();
                    go.GetComponent<MagicBehaviour>().isHit = true;
                }

            }
        if (BagManager.Instance.skillAttributesList[1].skillInfo != BagManager.Instance.NullInfo)
            if (BagManager.Instance.skillAttributesList[1].skillInfo.magicProperties.magicType == MagicType.StygianDesolator)
            {

                if (BagManager.Instance.skillAttributesList[1].isOn)
                {
                    BagManager.Instance.UseMagic(1);
                    FSM.PlayAnimation("Spin Attack");
                    GameObject go = NetPoolManager.Instantiate("Stygian Desolator", GameTool.FindTheChild(FSM.gameObject, "RigPelvisGizmo").position, FSM.transform.rotation);
                    AudioManager.PlaySound2D("Stygian Desolator").Play();
                    go.GetComponent<MagicBehaviour>().isHit = true;
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
