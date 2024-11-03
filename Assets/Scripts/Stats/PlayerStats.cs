using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    [SerializeField]private int expPoint;
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();
    }

    //EXP是购买技能的货币 int类型
    public bool TryUseExp(int _Cost)
    {
        if (expPoint >= _Cost)
        {
            expPoint -= _Cost;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddExp(int _EXP)
    {
        expPoint += _EXP;
    }
    public int GetExp()
    {
        return expPoint;
    }
}
