using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        if (!player.isWallDetected())
        {
            stateMachine.ChangeCurrentState(player.IdleState);
        }
        if (yInput >= 0)
        {
            player.SetVelocity(rb.velocity.x, rb.velocity.y * 0.7f);
        }
        else
        {
            player.SetVelocity(rb.velocity.x, yInput * 8f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(xInput * player.facingDir < 0)
            {
                stateMachine.ChangeCurrentState(player.WallJumpState);
            }
            else
            {
                player.SetVelocity(-50f * player.facingDir, rb.velocity.y);
                
            }
           
        }


        if (player.isGroundDetected())
        {
            stateMachine.ChangeCurrentState(player.IdleState);
        }
        
    }


}
