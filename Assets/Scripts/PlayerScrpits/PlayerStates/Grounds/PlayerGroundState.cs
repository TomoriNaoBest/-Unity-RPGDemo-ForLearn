using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _playerStateMachine, global::System.String _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

        if (Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeCurrentState(player.AttackState);
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeCurrentState(player.JumpState);
            return;
        }
        
        if (SkillManager.instance.sk_counter.canCounter)
        {
            if (Input.GetKeyDown(KeyCodeManager.instance.use_counter))
            {
                stateMachine.ChangeCurrentState(player.CounterState);
            } 
            return;
        }

        if (!player.isGroundDetected())
        {
            stateMachine.ChangeCurrentState(player.AirState);
            return;
        }

        if (SkillManager.instance.sk_sword.canThrow)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)&&!player.sword)
            {
                stateMachine.ChangeCurrentState(player.AimSwordState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && player.sword)
            {
                player.sword.GetComponent<SwordController>().SwordReturn();
                return;
            }
        }

        if (SkillManager.instance.sk_blackhole.canBlackHole)
        {
            if (Input.GetKeyDown(KeyCodeManager.instance.use_BlackHole))
            {
                stateMachine.ChangeCurrentState(player.BlackHoleState);
                return;
            }
        }

    }
}
