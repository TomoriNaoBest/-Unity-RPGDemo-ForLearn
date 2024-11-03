using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAirState : EnemyState
{
    private Enemy_Skeleton skeleton;
    public SkeletonAirState(Enemy _enemy, EnemyStateMachine _stateMachine, global::System.String _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
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
        bool g1, g2;
        bool[] group=skeleton.GetGroundDetectResult();
        g1=group[0];
        g2=group[1];
        if (g1 && g2)
        {
            stateMachine.ChangeCurrentState(skeleton.IdleState);
        }
    }

    public override string GetStateName()
    {
        return "Air";
    }
}
