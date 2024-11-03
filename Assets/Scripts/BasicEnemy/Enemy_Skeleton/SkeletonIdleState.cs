using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, global::System.String _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 4;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeCurrentState(skeleton.MoveState);
        }
        skeleton.SetVelocityZero();
    }

    public override string GetStateName()
    {
        return "Idle";
    }
}
