using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    private Enemy_Skeleton skeleton;

    public SkeletonDieState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.SetVelocityZero();
        rb.gravityScale = 10;
        //skeleton.animator.SetBool("Idle",false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        bool[] result = skeleton.GetGroundDetectResult();
        if (result[0] || result[1])
        {
            skeleton.enabled = false;
        }
    }

    public override string GetStateName()
    {
        return "Die";
    }
}
