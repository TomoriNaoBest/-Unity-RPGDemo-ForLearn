using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState :PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

    public override void Update()
    {
        base.Update();
        player.FlipController(rb.velocity.x);
        //以groundcheck为依据切成闲置状态
        player.SetVelocity(player.moveSpeed * 0.7f * xInput, rb.velocity.y);
        if (player.isGroundDetected())
        {
            stateMachine.ChangeCurrentState(player.IdleState);
            return;
        }
        if (player.isWallDetected())
        {
            stateMachine.ChangeCurrentState(player.WallSlideState);
        }

    }
}
