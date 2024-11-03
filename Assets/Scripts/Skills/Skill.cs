using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolWindow;
    protected float coolTimer;
    protected Player player;

    protected virtual void Start()
    {
        player=PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        if(coolTimer >= 0)
        {
            coolTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUseSkill()
    {
        if(coolTimer < 0)
        {
            UseSkill();
            coolTimer = coolWindow;
            return true;
        }
        return false;
    }

    protected virtual void UseSkill()
    {
        //实施具体的技能效果
    }
    protected virtual Transform findCloestEnemy(Transform _sourceTransform,float _radius)
    {
        Enemy closestEnemy = null;
        float minusDistance = Mathf.Infinity;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_sourceTransform.position, _radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                Enemy nowEnemy = collider.GetComponent<Enemy>();
                float nowDistance = Vector2.Distance(nowEnemy.transform.position, _sourceTransform.position);
                if (nowDistance < minusDistance)
                {
                    closestEnemy = nowEnemy;
                    minusDistance = nowDistance;
                }
            }
        }
        if (closestEnemy != null)
        {
            return closestEnemy.transform;
        }
        return null;
       
    }
}
