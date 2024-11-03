using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, global::System.String _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 5;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        skeleton.SetVelocity(skeleton.MoveSpeed*skeleton.facingDir,0);
        if (skeleton.isWallDetected())
        {
            skeleton.Flip();
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeCurrentState(skeleton.IdleState);
        }
    }

    public override string GetStateName()
    {
        return "Move";
    }
}
