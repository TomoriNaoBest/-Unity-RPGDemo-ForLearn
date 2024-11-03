using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if (sword.position.x < player.transform.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        if (sword.position.x > player.transform.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
        player.SetVelocity(SkillManager.instance.sk_sword.getReturnImpact() * -player.facingDir, rb.velocity.y);
    }
    public override void Update()
    {
        base.Update();
        if (stateTrigger)
        {
            stateMachine.ChangeCurrentState(player.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }

}
