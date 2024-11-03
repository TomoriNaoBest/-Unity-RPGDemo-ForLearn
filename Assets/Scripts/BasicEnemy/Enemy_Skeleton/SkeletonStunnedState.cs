using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton skeleton;
    public SkeletonStunnedState(Enemy _enemy, EnemyStateMachine _stateMachine, global::System.String _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.StunnedDuration;
        skeleton.entityFx.InvokeRepeating("RedColorBlink", 0, .1f);
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.entityFx.Invoke("CancleColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        skeleton.SetVelocity(skeleton.StunnedVector.x*-skeleton.facingDir, skeleton.StunnedVector.y);
        if (stateTimer < 0)
        {
            stateMachine.ChangeCurrentState(skeleton.IdleState);
        }

    }

    public override string GetStateName()
    {
        return "Stunned";
    }
}
