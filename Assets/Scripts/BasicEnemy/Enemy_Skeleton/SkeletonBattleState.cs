using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    protected Enemy_Skeleton skeleton;
    private Transform trans_player;
    private float lastAttackTime=0;
    public SkeletonBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.skeleton = (Enemy_Skeleton)_enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.DashWindow;
        trans_player=PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();
        //如果想要让骷髅只要看见人就一直追（现在是最多追一定时间）
        if (skeleton.DetectPlayer(0))
        {
            stateTimer = skeleton.DashWindow;
        }
        if (!skeleton.DetectPlayer(0)&&stateTimer<0)
        {
            stateMachine.ChangeCurrentState(skeleton.IdleState);
            return;
        }
        float dashdir;
        if (skeleton.transform.position.x > trans_player.position.x)
        {
            dashdir = -1;
        }
        else
        {
            dashdir = 1;
        }
        if (dashdir != skeleton.facingDir)
        {
            skeleton.Flip();
        }
        if (!skeleton.DetectPlayer(1))
        {
            skeleton.animator.SetBool("Move", true);
            skeleton.animator.SetBool("Idle", false);
           
            skeleton.SetVelocity(skeleton.DashSpeed*skeleton.facingDir,0);
        }
        bool[] result= skeleton.GetGroundDetectResult();
        if (result[0] == false && result[1] == false)
        {
            stateMachine.ChangeCurrentState(skeleton.AirState);
            return;
        }
        if (skeleton.DetectPlayer(1))
        {
            if (isAttackable())
            {
            lastAttackTime=Time.time;
                skeleton.animator.SetBool("Idle", false);
                stateMachine.ChangeCurrentState(skeleton.AttackState);
            return;
            }
            else
            {
                skeleton.animator.SetBool("Idle", true);
                skeleton.animator.SetBool("Move", false);
                skeleton.SetVelocityZero();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.animator.SetBool("Idle", false);
    }

    public override string GetStateName()
    {
        return "Battle";
    }

    private bool isAttackable()
    {
        if(Time.time - lastAttackTime >= skeleton.CoolWindow)
        {
            return true;
        }
        return false;
    }


}
