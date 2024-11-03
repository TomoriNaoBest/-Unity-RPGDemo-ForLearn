using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState IdleState {  get; private set; }
    public SkeletonMoveState MoveState {  get; private set; }
    public SkeletonAirState AirState {  get; private set; }
    public SkeletonBattleState BattleState {  get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }
    public SkeletonDieState DieState {  get; private set; }
    #endregion

    public CapsuleCollider2D cd { get; private set; }


    [Header("AttackInfo")]
    [SerializeField] public float DashSpeed;
    private float default_dashspeed;
    [SerializeField] public float CoolWindow;
    [SerializeField] public float DashWindow;
    [SerializeField] public float SenseDistance;
    [SerializeField] public Vector2 StunnedVector;
    [SerializeField] public float StunnedDuration;
    [HideInInspector]public int lastgnum = 0; //用于“可任意碰撞”的翻转
    protected override void Awake()
    {
        base.Awake();
        MoveSpeed = 5;
        DashSpeed = 7;
        default_dashspeed=DashSpeed;
        wallCheckDistance = 0.5f;
        groundCheckDistance = 0.33f;
        playerCheckDistance = 6f;
        SenseDistance = 3f;
        attackCheckDistance = 1.5f;
        DashWindow = 5;
        CoolWindow = 2;
        cd=GetComponent<CapsuleCollider2D>();
        IdleState = new SkeletonIdleState(this, stateMachine, "Idle");
        MoveState = new SkeletonMoveState(this, stateMachine, "Move");
        AirState = new SkeletonAirState(this, stateMachine, "Move");
        BattleState = new SkeletonBattleState(this, stateMachine, "Move");
        AttackState = new SkeletonAttackState(this, stateMachine, "Attack");
        StunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned");
        DieState = new SkeletonDieState(this, stateMachine,"Die");
        stateMachine.Initialize(IdleState);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

    }
    public void finshTrigger()=>stateMachine.currentState.FinshTrigger();

    public override bool TryCounter()
    {
        if (base.TryCounter())
        {
            stateMachine.ChangeCurrentState(StunnedState);
            return true;
        }
        return false;
    }
   public override void Die()
    {
        base.Die();
        stateMachine.ChangeCurrentState(DieState);
        characterStats.enabled = false;
       GetComponentInChildren<UI_HealthyBar>().doDestroy();
    }
    public override void SlowSpeedBy(float _slowPercent, float _duration)
    {
        //注意enemy写了如果freeze时间了就不生效
        base.SlowSpeedBy(_slowPercent, _duration);
        DashSpeed *= (1 - _slowPercent);
        MoveSpeed *= (1 - _slowPercent);
        animator.speed *= (1 - _slowPercent);
        Invoke("ReturnBasicSpeed", _duration);
    }
    protected override void ReturnBasicSpeed()
    {
        base.ReturnBasicSpeed();
        DashSpeed = default_dashspeed;
        MoveSpeed=default_MoveSpeed;
    }
}
