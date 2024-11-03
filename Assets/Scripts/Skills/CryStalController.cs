using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryStalController : MonoBehaviour
{
    private float duration;
    private bool canExplode;
    private Animator animator;
    private float attackRadius;
    private bool canGrow;
    private float growSpeed;
    private bool doGrow;
    private bool canMove;
    private float moveSpeed;
    private Transform cloestEnemy;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (duration >= 0)
        {
            duration-= Time.deltaTime;
        }
        if (duration < 0) {
            CrystalDestroy();
        }
        if (doGrow)
        {
            transform.localScale=Vector3.Lerp(transform.localScale, new Vector3(3,3),growSpeed*Time.deltaTime);
        }
        if (canMove&&cloestEnemy!=null) {
            transform.position = Vector2.MoveTowards(transform.position, cloestEnemy.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, cloestEnemy.position) < 2)
            {
                CrystalDestroy();
            }
        }
    }
    public void SetUp(float _duration, bool _canExplode, float _attackRadius, bool _canGrow, float _growSpeed,bool _canMove,float _moveSpeed,Transform _cloestEnemy,Player _player)
    {
        duration = _duration;
        canExplode = _canExplode;
        attackRadius = _attackRadius;
        canGrow = _canGrow;
        growSpeed = _growSpeed;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        cloestEnemy = _cloestEnemy;
        player = _player;
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
    }
    public void CrystalDestroy()
    {
        if (canExplode)
        {
            animator.SetBool("Idle", false);
            if (canGrow)
            {
                doGrow = true;
            }   
        }else
        {
            Destroy(gameObject);
        }
    }
    public void ExplodeAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null && !collider.GetComponent<Enemy>().nowState.Equals("Die"))
            {
                player.characterStats.DoDamage(collider.GetComponent<EnemyStats>(), true);
            }
        }
    }
    public void DestroyTrigger()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        if (canExplode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }

}
