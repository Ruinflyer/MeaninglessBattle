using System.Collections;
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
        
        NetworkManager.SendUpdatePlayerInfo(characterStatus.HP,transform.position, transform.rotation.eulerAngles,0,0,0, animationManager.LayerCur, animationManager.animCur);
    }

    protected override void FSMUpdate()
    {
        characterStatus = controller.characterStatus;

        if(Input.GetKeyDown(KeyCode.H))
        {
            controller.GetDeBuffInTime(BuffType.Freeze,5);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            controller.GetDeBuffInTime(BuffType.Blind, 5);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            controller.GetDeBuffInTime(BuffType.SlowDown, 5);
        }
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
        idle.AddTransition(FSMTransitionType.CanPickUp, FSMStateType.PickUp);
        idle.AddTransition(FSMTransitionType.UsingThunderBolt, FSMStateType.ThunderBolt);
        idle.AddTransition(FSMTransitionType.AttackWithSpear, FSMStateType.SpearAttack);
        idle.AddTransition(FSMTransitionType.CanDefend, FSMStateType.Defend);
        idle.AddTransition(FSMTransitionType.Falling, FSMStateType.Fall);

        MoveState move = new MoveState();
        move.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        move.AddTransition(FSMTransitionType.CanBeJump, FSMStateType.Jump);
        move.AddTransition(FSMTransitionType.AttackWithSingleWield, FSMStateType.SingleWieldAttack);
        move.AddTransition(FSMTransitionType.AttackWithDoubleHands, FSMStateType.DoubleHandsAttack);
        move.AddTransition(FSMTransitionType.AttackWithSpear, FSMStateType.SpearAttack);

        JumpState jump = new JumpState();
        jump.AddTransition(FSMTransitionType.IsIdle, FSMStateType.Idle);
        jump.AddTransition(FSMTransitionType.CanBeMove, FSMStateType.Move);

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


        AddFSMState(idle);
        AddFSMState(move);
        AddFSMState(jump);
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
    }


}
