using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    private bool CanAttack;
    private float cloneTimer;
    private float drySpeed;

    private SpriteRenderer sr;
    private Animator animator;

    private bool isfacingRight = true;
    private float facingDir = 1;
    private Transform closestEnemy;
    private Player player;

    [SerializeField]private Transform acPoint;
    [SerializeField]private float attackCheckRadius;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cloneTimer >= 0)
        {
            cloneTimer -= Time.deltaTime;
        }
        sr.color=new Color(1,1,1,sr.color.a-(Time.deltaTime*drySpeed));
        if (cloneTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetupClone(Vector3 _position,float _duration,bool _Canattack,Vector3 _offset,Transform _closestEnemy,Player _player)
    {
        animator = GetComponent<Animator>();
        this.transform.position = _position+_offset;
        cloneTimer= _duration;
        drySpeed=1/_duration;
        CanAttack = _Canattack;
        closestEnemy = _closestEnemy;
        player= _player;
        if (CanAttack)
        {
            FacingClosestEnemy();
            animator.SetInteger("AttackKind",Random.Range(1,4));
        }
        else
        {
            LookAtEnemy(PlayerManager.instance.player.transform);
        }
    }

   

    public void finishTrigger()
    {
        animator.SetInteger("AttackKind", 0);
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(acPoint.position, attackCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null&&!collider.GetComponent<Enemy>().nowState.Equals("Die"))
            {  
               player.characterStats.DoDamage(collider.GetComponent<EnemyStats>(),SkillManager.instance.sk_clone.canMagic);
            }
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        isfacingRight = !isfacingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FacingClosestEnemy()
    {
        if (closestEnemy == null)
        {
            LookAtPlayer();
        }
        else
        {
          LookAtEnemy(closestEnemy);
        }
    }

    private void LookAtPlayer()
    {
        if (isfacingRight != PlayerManager.instance.player.isfacingRight)
        {
            Flip();
        }
    }

    private void LookAtEnemy(Transform _target)
    {
        if (_target.position.x < transform.position.x && isfacingRight || _target.position.x > transform.position.x && !isfacingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(acPoint.position, attackCheckRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
