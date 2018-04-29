﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerFSM :BaseFSM
{

    /*
    public void LoadCharacterStatus()
    {
            string path="Player/"+"player" + ".json";
            characterStatus = MeaninglessJson.LoadJsonFromFile<CharacterStatus>(MeaninglessJson.Path_StreamingAssets+ path);
    }
    */

    protected override void Initialize()
    {
        comboCount = 0;
        animationManager = GetComponent<AnimationManager>();
        controller = GetComponent<MeaninglessCharacterController>();
        ConstructFSM();
        characterStatus = controller.characterStatus;
        //LoadCharacterStatus();

    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(this);
        CurrentState.Act(this);
        
    }

    protected override void FSMUpdate()
    {
        characterStatus = controller.characterStatus;
    }

    private void ConstructFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);
        idle.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        idle.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        idle.AddTransition(FSMTransitionType.UsingRipple, FSMStateType.RippleAttack);
        idle.AddTransition(FSMTransitionType.UsingHeartAttack, FSMStateType.HeartAttack);
        idle.AddTransition(FSMTransitionType.UsingStygianDesolator, FSMStateType.StygianDesolator);
        idle.AddTransition(FSMTransitionType.UsingIceArrow, FSMStateType.IceArrow);
        idle.AddTransition(FSMTransitionType.UsingChoshimArrow, FSMStateType.ChoshimArrow);
        idle.AddTransition(FSMTransitionType.CanPickUp, FSMStateType.PickUp);
        idle.AddTransition(FSMTransitionType.UsingThunderBolt, FSMStateType.ThunderBolt);
        idle.AddTransition(FSMTransitionType.AttackWithSpear, FSMStateType.SpearAttack);
        idle.AddTransition(FSMTransitionType.CanDefend, FSMStateType.Defend);
        idle.AddTransition(FSMTransitionType.Falling, FSMStateType.Fall);
        idle.AddTransition(FSMTransitionType.CanRoll, FSMStateType.Roll);

        MoveState move = new MoveState();
        move.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        move.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        move.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        move.AddTransition(FSMTransitionType.AttackWithSpear, FSMStateType.SpearAttack);
        move.AddTransition(FSMTransitionType.CanRoll, FSMStateType.Roll);

       

        DefendState defend = new DefendState();
        defend.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        SingleWieldAttackState singleWieldAttack = new SingleWieldAttackState();
        singleWieldAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        singleWieldAttack.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        singleWieldAttack.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);

        DoubleHandsAttackState dualWieldAttack = new DoubleHandsAttackState();
        dualWieldAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        dualWieldAttack.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        dualWieldAttack.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);


        SpearAttackState spearAttack = new SpearAttackState();
        spearAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        RippleAttackState rippleAttack = new RippleAttackState();
        rippleAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        HeartAttackState heartAttack = new HeartAttackState();
        heartAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        StygianDesolatorState stygianDesolator = new StygianDesolatorState();
        stygianDesolator.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        IceArrowState iceArrow = new IceArrowState();
        iceArrow.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        ChoshimArrowState choshimArrow = new ChoshimArrowState();
        choshimArrow.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        ThunderBoltState thunderBolt = new ThunderBoltState();
        thunderBolt.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        PickUpState pickUp = new PickUpState();
        pickUp.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        FallState fall = new FallState();
        fall.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        RollState roll = new RollState();
        roll.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);


        AddFSMState(idle);
        AddFSMState(move);
        AddFSMState(defend);
        AddFSMState(singleWieldAttack);
        AddFSMState(dualWieldAttack);
        AddFSMState(rippleAttack);
        AddFSMState(heartAttack);
        AddFSMState(stygianDesolator);
        AddFSMState(iceArrow);
        AddFSMState(choshimArrow);
        AddFSMState(thunderBolt);
        AddFSMState(pickUp);
        AddFSMState(spearAttack);
        AddFSMState(fall);
        AddFSMState(roll);
    }


}
