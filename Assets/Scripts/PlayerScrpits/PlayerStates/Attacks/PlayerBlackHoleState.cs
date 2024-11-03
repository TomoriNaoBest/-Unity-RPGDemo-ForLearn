using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyDuration;
    private float flySpeed;
    private bool isSkillUsed;
    private float defaultG;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        flyDuration=SkillManager.instance.sk_blackhole.getFlyDuration();
        flySpeed=SkillManager.instance.sk_blackhole.getFlySpeed();
        defaultG = rb.gravityScale;
        rb.gravityScale = 0;
        isSkillUsed=false;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultG;
        player.entityFx.MakeTrasnParent(false);
    }

    public override void Update()
    {
        base.Update();
        if (flyDuration >= 0)
        {
            player.SetVelocity(0, flySpeed*Time.deltaTime);
            flyDuration-=Time.deltaTime;
        }
        if (flyDuration < 0&&!isSkillUsed)
        {
            SkillManager.instance.sk_blackhole.CanUseSkill();
            isSkillUsed=true;
        }
        if (isSkillUsed)
        {
            player.SetVelocity(0, -0.1f * Time.deltaTime);
        }

    }
}
