using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimatior :MonoBehaviour
{
  private Enemy_Skeleton skeleton=>GetComponentInParent<Enemy_Skeleton>();
  public void finshTrigger()
    {
        skeleton.finshTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.acPoint.position, skeleton.attackCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Player>() != null)
            {
                PlayerStats stats = collider.GetComponent<PlayerStats>();
                skeleton.characterStats.DoDamage(stats,true);
            }
        }
    }

    private void openCounterWindow()
    {
        skeleton.OpenCounterWindow();
    }
    private void closeCounterWindow() { 
        skeleton.CloseCounterWindow();
    }
}
