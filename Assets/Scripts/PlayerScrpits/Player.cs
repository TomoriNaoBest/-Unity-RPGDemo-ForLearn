using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class Player : Entity
{
    #region State
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerCounterState CounterState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackHoleState BlackHoleState { get; private set; }
    public PlayerDieState DieState { get; private set; }
    #endregion

    public GameObject sword {  get; private set; }=null;

    [Header("MoveInfo")]
    #region MoveInfo
    public float moveSpeed;
    public float jumpForce;
    public float dashDir;
    private float default_movespeed;
    private float default_jumpforce;
    private float default_dashSpeed;
    #endregion
    [Header("AttackInfo")]
    public float[] AttackMovement;


    public bool isBusy { get; private set; } = false;

    //---------------------------------------------------\\
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 7;
        jumpForce = 13;
        groundCheckDistance = 0.3f;
        wallCheckDistance = 0.15f;
        AttackMovement = new float[] { 3, 3, 6 };
        
        default_movespeed=moveSpeed;
        default_jumpforce=jumpForce;
        //注意必须先拿到rb再初始化状态机 否则状态机有的rb属性就是空
        stateMachine=new PlayerStateMachine();
        IdleState=new PlayerIdleState(this,stateMachine,"Idle");
        MoveState = new PlayerMoveState(this, stateMachine, "Move");
        JumpState = new PlayerJumpState(this, stateMachine, "Jump");
        AirState = new PlayerAirState(this, stateMachine, "Air");
        DashState = new PlayerDashState(this, stateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        AttackState = new PlayerAttackState(this, stateMachine, "Attack");
        CounterState = new PlayerCounterState(this, stateMachine, "Counter");
        AimSwordState = new PlayerAimSwordState(this, stateMachine, "SwordAim");
        CatchSwordState = new PlayerCatchSwordState(this, stateMachine, "SwordCatch");
        BlackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        DieState = new PlayerDieState(this, stateMachine, "Die");
        

    }

    // Start is called before the first frame update 
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(IdleState);
       
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        xvelocity = rb.velocity.x;
        stateMachine.currentState.Update();
        nowState = stateMachine.currentState.GetanimBoolName();
        DashController();
        if (SkillManager.instance.sk_crystal.canUseCrystal) {
            if (Input.GetKeyDown(KeyCodeManager.instance.use_crystal))
            {
                SkillManager.instance.sk_crystal.CanUseSkill();
            }
        }
        if (Input.GetKeyDown(KeyCodeManager.instance.use_Flask))
        {
            Inventory.instance.UseFlask();
        }
    }

    public void DashController()
    {
        if (SkillManager.instance.sk_dash.canDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.sk_dash.CanUseSkill() && !isWallDetected())
            {
                dashDir = GetDirbyXinput();
                stateMachine.ChangeCurrentState(this.DashState);
            }
        }  
    }


    public virtual bool isGroundDetected()
    {
        bool g1, g2;
        g1 = Physics2D.Raycast(gcPoint1.position, Vector2.down, groundCheckDistance, whatisGround);
        g2 = Physics2D.Raycast(gcPoint2.position, Vector2.down, groundCheckDistance, whatisGround);
        return g1 || g2;
    }


    public void finishTrigger() => stateMachine.currentState.finishTrigger();

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public float GetDirbyXinput()
    {
        float dir = facingDir;
        float xinput = Input.GetAxisRaw("Horizontal");
        if (xinput != 0)
        {
            if (xinput > 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
        }
        return dir;
    }

    public void DamageEffect()
    {
        entityFx.StartCoroutine("FlashFX");
    }
    public void AssignSword(GameObject _sword)
    {
        sword = _sword;
    }
    public void CatchSword()
    {
        stateMachine.ChangeCurrentState(CatchSwordState);
        Destroy(sword);
    }
    public void ExitBlackHoleState()
    {
        stateMachine.ChangeCurrentState(AirState);
    }

    public void Die()
    {
        stateMachine.ChangeCurrentState(DieState);
    }
    #region ChillRegion
    public override void SlowSpeedBy(float _slowPercent, float _duration)
    {
        base.SlowSpeedBy(_slowPercent, _duration);
        default_dashSpeed = SkillManager.instance.sk_dash.getdashSpeed();
        moveSpeed *= (1 - _slowPercent);
        jumpForce *= (1 - _slowPercent);
        animator.speed *= (1 - _slowPercent);
        SkillManager.instance.sk_dash.SetDashSpeed(SkillManager.instance.sk_dash.getdashSpeed() * (1 - _slowPercent));
        Invoke("ReturnBasicSpeed", _duration);
    }
    protected override void ReturnBasicSpeed()
    {
        base.ReturnBasicSpeed();
        moveSpeed=default_movespeed;
        jumpForce=default_jumpforce;
        SkillManager.instance.sk_dash.SetDashSpeed(default_dashSpeed);
    }
    #endregion
    
}

