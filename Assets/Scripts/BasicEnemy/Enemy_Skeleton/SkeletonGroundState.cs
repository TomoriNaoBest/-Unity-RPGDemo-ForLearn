using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected Enemy_Skeleton skeleton;
    private bool g1, g2;
    public SkeletonGroundState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
        bool[] group = skeleton.GetGroundDetectResult();
        g1 = group[0];
        g2 = group[1];
        if (g1 == false && g2 == false)
        {
            stateMachine.ChangeCurrentState(skeleton.AirState);
            return;
        }
        RealFlipController();
        if (skeleton.DetectPlayer(0)||isSenseAble())
        {
            stateMachine.ChangeCurrentState(skeleton.BattleState);
        }
    }

    private bool isSenseAble()
    {
        Transform trans_player = PlayerManager.instance.player.transform;
        if (Vector2.Distance(trans_player.position, skeleton.transform.position) <= skeleton.SenseDistance)
        {
            return true;
        }
        return false;
    }

    private void RealFlipController()
    {
        if (g1 == true && g2 == false || g2 == true && g1 == false)
        {
            int nowgnum;
            if (!g1)
            {
                nowgnum = 1;
            }
            else
            {
                nowgnum = 2;
            }
            if (nowgnum == skeleton.lastgnum || skeleton.lastgnum == 0)
            {
                skeleton.Flip();
                skeleton.lastgnum = nowgnum;

            }
        }
    }


}
