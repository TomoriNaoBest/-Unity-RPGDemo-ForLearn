using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterState : PlayerState
{
    public PlayerCounterState(Player _player, PlayerStateMachine _playerStateMachine, global::System.String _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=SkillManager.instance.sk_counter.getCounterDuration();
        player.animator.SetBool("CounterSuccess", false);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0 || stateTrigger) {
            stateMachine.ChangeCurrentState(player.IdleState);
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.acPoint.position, player.attackCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                if (collider.GetComponent<Enemy>().TryCounter()) {
                    stateTimer = 10;
                    player.animator.SetBool("CounterSuccess",true);
                    SkillManager.instance.sk_counter.TryHeal();
                    SkillManager.instance.sk_counter.TryClone();
                }
                
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
