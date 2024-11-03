using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    #region Components
    private Player player;
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D circleCollider;
    #endregion

    private bool canRotate = true;
    private bool isReturning=false;
    private float returnSpeed;
    private float freezeTime;
    private enum SwordType
    {
        General,
        Bounce,
        Pierce,
        Spin
    }
    private SwordType swordType;
    //bounces
    private int bounceTime;
    private float bounceSpeed;
    private float bounceRadius;
    private List<Transform> enemyList;
    private int bounceTag=1;
    //pierces
    private int piercesTime;
    //spins
    private float maxTraveDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCoolWindow;
    private float spinDir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyList= new List<Transform>();
    }

    #region SetUps
    public void SetUpBasic(Player _player, Vector2 _vector0, float _gravity, float _returnSpeed,float _freezeTime)
    {
        player = _player;
        rb.gravityScale = _gravity;
        rb.velocity = _vector0;
        returnSpeed = _returnSpeed;
        freezeTime = _freezeTime;
        swordType = SwordType.General;
        animator.SetBool("Idle", true);
    }
    public void SetUpBounceInfo(int _bouceTime, float _bounceSpeed, float _bounceRadius)
    {
        swordType = SwordType.Bounce;
        bounceTime = _bouceTime;
        bounceSpeed = _bounceSpeed;
        bounceRadius = _bounceRadius;
        animator.SetBool("Idle", false);
    }
    public void SetUpPierceInfo(int _pierceTime, float _pierceGravity)
    {
        swordType = SwordType.Pierce;
        animator.SetBool("Idle", true);
        piercesTime = _pierceTime;
        rb.gravityScale = _pierceGravity;
    }
    public void SetUpSpinInfo(float _MaxTraveDistance, float _spinDuration, float _hitCoolWindow)
    {
        spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);
        swordType = SwordType.Spin;
        maxTraveDistance = _MaxTraveDistance;
        spinDuration = _spinDuration;
        hitCoolWindow = _hitCoolWindow;
        animator.SetBool("Idle", false);
        isSpinning = true;
        wasStopped = false;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        if (Vector2.Distance(transform.position, player.transform.position) > 30)
        {
            SwordReturn();
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed);
            if (Vector2.Distance(transform.position, player.transform.position) < 0.8f)
            {
                isReturning = false;
                player.CatchSword();
            }
            return;
        }
        SpinLogic();
        BounceLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTraveDistance && !wasStopped)
            {
                wasStopped = true;
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                spinTimer = spinDuration;
            }
        }
        if (wasStopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), 1.5f * Time.deltaTime);
            if (spinTimer >= 0)
            {
                spinTimer -= Time.deltaTime;
            }
            if (spinTimer < 0)
            {
                SwordReturn();
                isSpinning = false;
            }
            if (hitTimer >= 0)
            {
                hitTimer -= Time.deltaTime;
            }
            if (hitTimer < 0)
            {
                hitTimer = hitCoolWindow;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        AttackEnemy(hit.GetComponent<Enemy>(), true);
                    }
                }
            }
        }
    }

    private void BounceLogic()
    {
        if (enemyList.Count >= 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyList[bounceTag].position, bounceSpeed);
            if (Vector2.Distance(transform.position, enemyList[bounceTag].position) < 0.8f)
            {
                bounceTag += 1;
                bounceTime -= 1;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        AttackEnemy(hit.GetComponent<Enemy>(), true);
                    }
                }
            }
            if (bounceTag > enemyList.Count - 1)
            {
                bounceTag = 0;
            }
            if (bounceTime <= 0)
            {
                SwordReturn();
            }

        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (piercesTime > 0 && collision.GetComponent<Enemy>() != null)
        {
            piercesTime--;
            AttackEnemy(collision.GetComponent<Enemy>(), true);
            return;
        }

        if (isSpinning)
        {
            wasStopped = true;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            spinTimer = spinDuration;
            return;
        }

        rb.velocity = new Vector2(0, 0);
        circleCollider.enabled = false;
        rb.isKinematic = true;

        if (swordType == SwordType.Bounce)
        {  
            if (collision.GetComponent<Enemy>() != null)
            {
                player.characterStats.DoDamage(collision.GetComponent<EnemyStats>(),false);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceRadius);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyList.Add(hit.transform);
                    }
                }
                if (enemyList.Count >= 2)
                {
                    return;
                }
               
            }
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        canRotate = false;
        transform.parent=collision.transform;
        animator.SetBool("Idle",true);
        if (collision.GetComponent<Enemy>() != null) {
        AttackEnemy(collision.GetComponent<Enemy>(), true);
        }
    }

    private void AttackEnemy(Enemy _enemy,bool _isFreeze)
    {
        if (_enemy.nowState.Equals("Die"))
        {
            return;
        }
        player.characterStats.DoDamage(_enemy.GetComponent<EnemyStats>(),false);
        if (SkillManager.instance.sk_sword.canWeaken)
        {
            _enemy.GetComponent<EnemyStats>().ChangeStatFor(-SkillManager.instance.sk_sword.getWeakenPercent(), SkillManager.instance.sk_sword.getWekanTime(), _enemy.GetComponent<EnemyStats>().armor);
        }
        
        if (SkillManager.instance.sk_sword.canStopTime)
        {
            if (_isFreeze) {
                _enemy.StartCoroutine("FreezeTimeFor",freezeTime);
            }
        }
    }


    public void SwordReturn()
    {
        rb.velocity= new Vector2(0, 0);
        canRotate = true;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.parent = null;
        isReturning = true;
        if (swordType == SwordType.Spin||swordType==SwordType.Pierce)
        {
            animator.SetBool("Idle", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,bounceRadius);
        Gizmos.DrawWireSphere(transform.position, 0.8f);
    }
}
