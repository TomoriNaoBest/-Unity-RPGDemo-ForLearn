using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, global::System.String _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = SkillManager.instance.sk_dash.getdashDuration(); //1
        if (SkillManager.instance.sk_dash.canGenerateClone)
        {
            SkillManager.instance.sk_clone.CanUseSkill();
        }
    }

    public override void Exit()
    {
        player.SetVelocity(0, rb.velocity.y);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(SkillManager.instance.sk_dash.getdashSpeed() * player.dashDir, 0);//2
        if (stateTimer <= 0)
        {
            stateMachine.ChangeCurrentState(player.IdleState);
        }
    }
}
