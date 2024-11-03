using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Doge : Skill
{
    [SerializeField] private UI_SkillSlot dogeSlot;
    public bool canDoge;
    [SerializeField] private float avoidPercent;
    [SerializeField] private UI_SkillSlot generateCloneSlot;
    public bool canClone;
    

    protected override void Start()
    {
        base.Start();
        dogeSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockDoge());
        generateCloneSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockClone());
    }
    protected override void UseSkill()
    {
        base.UseSkill();
    }
    public float getSkillAvoid()
    {
        return avoidPercent;
    }
    private void TryUnlockDoge()
    {
        if (dogeSlot.isUnlocked)
        {
            if (!canDoge)
            {
                player.characterStats.evasion.AddModifier(avoidPercent);
            }
            canDoge=true;
            
        }
        else
        {
            if (canDoge)
            {
                player.characterStats.evasion.RemoveModifier(avoidPercent);
            }
            canClone=false;
        }
    }
    private void TryUnlockClone()
    {
        if (generateCloneSlot.isUnlocked)
        {
            canClone=true;
        }
        else
        {
            canClone=false;
        }
    }


}
