using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.2f;
        player.Flip();
        player.SetVelocity(0, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer > 0) { 
        player.SetVelocity(10f*player.facingDir,rb.velocity.y);
        }
        if (System.Math.Abs( rb.velocity.x)<= player.moveSpeed*0.7f||player.isWallDetected()||player.isGroundDetected())
        {
        stateMachine.ChangeCurrentState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
