using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("State")]
    [SerializeField] public string nowState;

    #region FacingInfo
    [Header("FacingInfo")]
    [SerializeField] public float facingDir = 1;
    [SerializeField] public bool isfacingRight = true;
    [SerializeField] public float xvelocity;
    #endregion

    #region CollisionDetect
    [Header("Collision Info")]
    [SerializeField] protected Transform gcPoint1;
    [SerializeField] protected Transform gcPoint2;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatisGround;
    [Space]
    [SerializeField] protected Transform wcPoint;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatisWall;
    [Space]
    [SerializeField] public Transform acPoint;
    [SerializeField] public float attackCheckRadius;
    #endregion

    #region KnockBack
    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 knockbackVector;
    private bool isKnocked = false;
    [SerializeField] protected float knockbackDuration;
    #endregion


    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFx entityFx { get; private set; }
    public CharacterStats characterStats { get; private set; }
    #endregion

    public System.Action OnFlipped;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        entityFx = GetComponentInChildren<EntityFx>();
        characterStats=GetComponent<CharacterStats>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        xvelocity = rb.velocity.x;
    }

    public virtual void SlowSpeedBy(float _slowPercent,float _duration)
    {

    }
    protected virtual void ReturnBasicSpeed()
    {
        animator.speed = 1;
    }

    #region CollisionDetect
    public virtual bool isWallDetected() => Physics2D.Raycast(wcPoint.position, Vector2.right * facingDir, wallCheckDistance, whatisWall);
    protected virtual  void OnDrawGizmos()
    {
        Gizmos.DrawLine(gcPoint1.position, new Vector3(gcPoint1.position.x, gcPoint1.position.y - groundCheckDistance));
        Gizmos.DrawLine(gcPoint2.position, new Vector3(gcPoint2.position.x, gcPoint2.position.y - groundCheckDistance));
        Gizmos.DrawLine(wcPoint.position, new Vector3(wcPoint.position.x + wallCheckDistance, wcPoint.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(acPoint.position,attackCheckRadius);
    }
    #endregion

    #region FlipBlocks
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        isfacingRight = !isfacingRight;
        transform.Rotate(0, 180, 0);
        if (OnFlipped != null)
        {
            OnFlipped();
        }
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !isfacingRight || _x < 0 && isfacingRight)
        {
            Flip();
        }
    }
    #endregion

    #region VelocityControl
    public virtual void SetVelocity(float _xVelocity,float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity=new Vector2(_xVelocity, _yVelocity);
    }
    public virtual void SetVelocityZero()
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    public IEnumerator KnockBack()
    {
        isKnocked= true;
        rb.velocity = new Vector2(knockbackVector.x*-facingDir,knockbackVector.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
}
