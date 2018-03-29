using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class PlayerFSM :BaseFSM
{
    public int playerID;

    public override void LoadCharacterStatus()
    {
            string path="Player/"+"player" + playerID + ".json";
            characterStatus = MeaninglessJson.LoadJsonFromFile<CharacterStatus>(MeaninglessJson.Path_StreamingAssets+ path);
    }

    protected override void Initialize()
    {
        comboCount = 0;
        animationManager = GetComponent<AnimationManager>();
        controller = GetComponent<MeaninglessCharacterController>();
        ConstructFSM();
        LoadCharacterStatus();
    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(this);
        CurrentState.Act(this);
    }

    private void ConstructFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);
        idle.AddTransition(FSMTransitionType.CanBeJump, FSMStateType.Jump);
        idle.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        idle.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        idle.AddTransition(FSMTransitionType.UsingRipple, FSMStateType.RippleAttack);
        idle.AddTransition(FSMTransitionType.UsingHeartAttack, FSMStateType.HeartAttack);
        idle.AddTransition(FSMTransitionType.UsingStygianDesolator, FSMStateType.StygianDesolator);
        idle.AddTransition(FSMTransitionType.UsingIceArrow, FSMStateType.IceArrow);
        idle.AddTransition(FSMTransitionType.UsingChoshimArrow, FSMStateType.ChoshimArrow);

        MoveState move = new MoveState();
        move.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        move.AddTransition(FSMTransitionType.CanBeJump, FSMStateType.Jump);
        move.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        move.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);

        JumpState jump = new JumpState();
        jump.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        jump.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);

        SingleWieldAttackState singleWieldAttack = new SingleWieldAttackState();
        singleWieldAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        singleWieldAttack.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        singleWieldAttack.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);

        DoubleHandsAttackState dualWieldAttack = new DoubleHandsAttackState();
        dualWieldAttack.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        dualWieldAttack.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        dualWieldAttack.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);

        RippleAttackState rippleAttackState = new RippleAttackState();
        rippleAttackState.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        HeartAttackState heartAttackState = new HeartAttackState();
        heartAttackState.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        StygianDesolatorState stygianDesolatorState = new StygianDesolatorState();
        stygianDesolatorState.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        IceArrowState iceArrowState = new IceArrowState();
        iceArrowState.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        ChoshimArrowState choshimArrowState = new ChoshimArrowState();
        choshimArrowState.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);

        AddFSMState(idle);
        AddFSMState(move);
        AddFSMState(jump);
        AddFSMState(singleWieldAttack);
        AddFSMState(dualWieldAttack);
        AddFSMState(rippleAttackState);
        AddFSMState(heartAttackState);
        AddFSMState(stygianDesolatorState);
        AddFSMState(iceArrowState);
        AddFSMState(choshimArrowState);
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }
}
