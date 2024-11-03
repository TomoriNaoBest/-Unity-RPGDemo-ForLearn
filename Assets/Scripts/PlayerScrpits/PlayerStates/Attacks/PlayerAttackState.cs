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
        //���������ÿ�ζ���֮������޷��ѡ�񹥻�����
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
        //�ý���ǰ���ٶȷ���0.x������ã�Ӫ����Եĸо���
        //����ٶȹ�0����update��д����enter��дû�ã���Ϊ���빥����groundstate��ģ�movestate������ٶȡ�
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
