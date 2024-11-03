using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(Player _player, PlayerStateMachine _playerStateMachine, global::System.String _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void finishTrigger()
    {
        base.finishTrigger();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocityZero();
    }
}
