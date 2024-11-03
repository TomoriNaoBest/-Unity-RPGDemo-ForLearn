using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    protected Enemy_Skeleton skeleton;
    public SkeletonAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
    public override void Update()
    {
        base.Update();
        skeleton.SetVelocityZero();
        if (stateTrigger)
        {

            stateMachine.ChangeCurrentState(skeleton.IdleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.CloseCounterWindow();
    }

    public override string GetStateName()
    {
        return "Attack";
    }

}
