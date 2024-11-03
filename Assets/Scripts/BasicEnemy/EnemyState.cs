using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected string animBoolName;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected bool stateTrigger;

    public EnemyState(Enemy _enemy,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.rb = _enemy.rb;
    }

    public virtual void Enter()
    {
        enemy.animator.SetBool(animBoolName, true);
        stateTrigger = false;
    }
    public virtual void Update()
    {
        if (stateTimer >= 0)
        {
            stateTimer -= Time.deltaTime;
        }

    }
    public virtual void Exit()
    {
        enemy.animator.SetBool(animBoolName, false);
    }
    public virtual string GetStateName() {
        return "";
    
    }

    public virtual void FinshTrigger()
    {
        stateTrigger = true;
    }



}
