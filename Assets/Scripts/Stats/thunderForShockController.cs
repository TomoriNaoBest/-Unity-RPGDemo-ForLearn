using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderForShockController : MonoBehaviour
{
    [SerializeField]private int Damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    private CharacterStats targetstats;
    private Animator animator;
    private bool isAttacked;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetstats=target.GetComponent<CharacterStats>();
    }
    public void SetUp(Transform _target,int _damage,float _movespeed)
    {
        target=_target;
        Damage=_damage;
        moveSpeed=_movespeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) >= 0.5f&&!isAttacked)
        {
            transform.position=Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.right = target.position - transform.position;
        }
        else
        {
            if (!isAttacked)
            {
                transform.Rotate(0,0,90);
                transform.position += new Vector3(0, 2);
                transform.localScale = new Vector2(3, 3);
                animator.SetBool("Attack", true);
                targetstats.TakeDamage(Damage);
                isAttacked = true;
                Destroy(gameObject, 0.2f);
            }
        }
    }
}
