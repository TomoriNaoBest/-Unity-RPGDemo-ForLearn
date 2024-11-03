using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator:MonoBehaviour
{
    private Player player=>GetComponentInParent<Player>();
   private void finishTrigger()
    {
        player.finishTrigger();
    }
   private void AttackTrigger()
    {
        Inventory.instance.GetEquipment(EquipmentType.ÎäÆ÷)?.ExecuteEffects();
        Collider2D[] colliders=Physics2D.OverlapCircleAll(player.acPoint.position,player.attackCheckRadius);
        foreach(Collider2D collider in colliders)
        {
            if(collider.GetComponent<Enemy>() != null&&!collider.GetComponent<Enemy>().nowState.Equals("Die"))
            {
                EnemyStats targetStats= collider.GetComponent<EnemyStats>();
                player.characterStats.DoDamage(targetStats,true);
                
            }
        }
    }
    private void GenerateSword()
    {
        SkillManager.instance.sk_sword.CanUseSkill();
    }
}
