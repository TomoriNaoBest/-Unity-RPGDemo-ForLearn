using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public int comboCounter {  get; private set; }
    private float comboWindow=0.3f;
    private float lastAttackTime;
    public PlayerAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.1f;
        if (comboCounter > 2 ||Time.time>lastAttackTime+comboWindow)
        {
            comboCounter = 0;
        }
        player.animator.SetInteger("comboCounter", comboCounter);
        //允许玩家在每段动作之间近乎无缝的选择攻击方向。
        float AttackDir = player.GetDirbyXinput();
        player.FlipController(AttackDir);
        player.SetVelocity(player.AttackMovement[comboCounter] * AttackDir, 0);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        //让进入前的速度发挥0.x秒的作用，营造惯性的感觉。
        //这个速度归0必须update里写，在enter里写没用，因为进入攻击是groundstate里的，movestate还会给速度。
        if (stateTimer <= 0)
        {
            player.SetVelocityZero();
        }
        if (stateTrigger)
        {
            comboCounter++;
            stateMachine.ChangeCurrentState(player.IdleState);
        }
    }
}
