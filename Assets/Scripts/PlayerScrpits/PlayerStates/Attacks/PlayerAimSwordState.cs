using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.instance.sk_sword.SetDotsActive(true);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocityZero();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeCurrentState(player.IdleState);
        }
        Vector2 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < player.transform.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        if (mousePosition.x > player.transform.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
        SkillManager.instance.sk_sword.SetDotsActive(false);
        player.StartCoroutine("BusyFor", .2f);
    }

}
