using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Counter :Skill
{
    public bool canCounter;
    [SerializeField] private UI_SkillSlot counterSlot;
    public bool canHealFromCounter;
    [SerializeField]private UI_SkillSlot healSlot;
    public bool canGenerateCloneFromCounter;
    [SerializeField] private UI_SkillSlot cloneSlot;

    [SerializeField] private float counterDuration;
    protected override void Start()
    {
        base.Start();
        counterSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockCounter());
        healSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockHeal());
        cloneSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockClone());
    }
    private void TryUnlockCounter()
    {
        if (counterSlot.isUnlocked)
        {
            canCounter=true;
        }
        else
        {
            canCounter=false;
        }
    }
    private void TryUnlockHeal()
    {
        if (healSlot.isUnlocked)
        {
            canHealFromCounter=true;
        }
        else
        {
            canHealFromCounter=false;
        }
    }
    private void TryUnlockClone()
    {
        if (cloneSlot.isUnlocked)
        {
            canGenerateCloneFromCounter=true;
        }
        else
        {
            canGenerateCloneFromCounter=false;
        }
    }
    public void TryHeal()
    {
        if (canHealFromCounter)
        {
            player.characterStats.IncreaseCurrentHp(player.characterStats.getMaxHpValue()/10);
            //Debug.Log("通过弹反回血");
        }
    }
    public void TryClone()
    {
        if (canGenerateCloneFromCounter)
        {
            SkillManager.instance.sk_clone.CanUseSkill();
        }
    }
    public float getCounterDuration()
    {
        return counterDuration;
    }

    protected override void UseSkill()
    {
        base.UseSkill();
    }
}
