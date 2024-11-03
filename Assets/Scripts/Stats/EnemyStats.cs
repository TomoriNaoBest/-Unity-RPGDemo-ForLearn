using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    [Header("Level Details")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;
    private ItemDrop myDrop;
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        enemy.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDrop.GenerateDrop();
    }
    private void ModifyByLevel(Stat _stat)
    {
        for (int i = 1; i < level; i++) { 
            float modifier=_stat.getValue()*percantageModifier;
            _stat.AddModifier(modifier);
        }
    }

    protected override void Start()
    {
        ModifyByLevel(maxHp);
        ModifyByLevel(damage);
        base.Start();
        enemy = GetComponent<Enemy>();
        myDrop= GetComponent<ItemDrop>();
    }

    protected override void Update()
    {
        base.Update();
    }
}
