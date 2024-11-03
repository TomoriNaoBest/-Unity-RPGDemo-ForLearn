using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    private string animBoolName;
    protected float stateTimer=0;
    protected bool stateTrigger;
    

    public PlayerState(Player _player,PlayerStateMachine _playerStateMachine,string _animBoolName)
    {
        this.player=_player;
        this.stateMachine=_playerStateMachine;
        this.animBoolName=_animBoolName;
        this.rb = _player.rb;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName,true);
        stateTrigger = false;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        player.animator.SetFloat("yVelocity",rb.velocity.y);
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }

    }
    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false );
    }

    public virtual string GetanimBoolName()
    {
        return animBoolName;
    }

    public virtual void finishTrigger() 
    {
        stateTrigger = true;
    }
}
