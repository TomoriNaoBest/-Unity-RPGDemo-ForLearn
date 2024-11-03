using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("PlayerDetect")]
    [SerializeField] protected LayerMask whatisPlayer;
    [SerializeField] protected float playerCheckDistance;
    [Header("AttackInfo")]
    [SerializeField] protected float attackCheckDistance;
    [SerializeField]protected GameObject CounterImage;
    protected bool CounterWindow=false;
    #region States
    protected EnemyStateMachine stateMachine;
    #endregion
    protected string lastGpoint;
    #region MoveInfo
    [Header("MoveInfo")]
    [SerializeField] public float MoveSpeed;
    protected float default_MoveSpeed;
    private bool isFreeze;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine=new EnemyStateMachine();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //注意movespeed是在awake里配置的，所以这个得写start里
        default_MoveSpeed=MoveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        nowState=stateMachine.currentState.GetStateName();
        stateMachine.currentState.Update();
       
    }
    public bool[] GetGroundDetectResult()
    {
        bool g1, g2;
        g1 = Physics2D.Raycast(gcPoint1.position, Vector2.down, groundCheckDistance, whatisGround);
        g2 = Physics2D.Raycast(gcPoint2.position, Vector2.down, groundCheckDistance, whatisGround);
        return new bool[] { g1, g2 };

    }

    public virtual RaycastHit2D DetectPlayer(int _model)
    {
        //0模式检测追击 1模式检测到达攻击距离
        if (_model == 0)
        {
          return Physics2D.Raycast(wcPoint.position, Vector2.right * facingDir, playerCheckDistance, whatisPlayer);
        }
        if (_model == 1) {
            return Physics2D.Raycast(wcPoint.position, Vector2.right * facingDir, attackCheckDistance, whatisPlayer);
        }
        return new RaycastHit2D();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wcPoint.position, new Vector2(wcPoint.position.x + playerCheckDistance * facingDir, wcPoint.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wcPoint.position, new Vector2(wcPoint.position.x + attackCheckDistance * facingDir, wcPoint.position.y));
    }

    public void DamageEffect()
    {
        entityFx.StartCoroutine("FlashFX");
        this.StartCoroutine("KnockBack");
    }
    #region CounterRegion
    public void OpenCounterWindow()
    {
        CounterWindow = true;
        CounterImage.SetActive(true);
    }
    public void CloseCounterWindow() {
        CounterWindow = false;
        CounterImage.SetActive(false);
    }
    public virtual bool TryCounter()
    {
        if (CounterWindow)
        {
            CounterWindow = false;
            CounterImage.SetActive(false);
            return true;
        }
        return false;

    }
    #endregion
    #region FreezeTime
    public virtual void FreezeTime(bool _isFreeze)
    {
        if (_isFreeze)
        {
            MoveSpeed = 0;
            animator.speed= 0;
            isFreeze = true;
        }
        else
        {
            MoveSpeed=default_MoveSpeed;
            animator.speed = 1;
            isFreeze = false;
        }
    }
    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }
    #endregion
    public virtual void Die()
    {

    }
    public override void SlowSpeedBy(float _slowPercent, float _duration)
    {
        base.SlowSpeedBy(_slowPercent, _duration);
        if (isFreeze)
        {
            return;
        }
    }
}
