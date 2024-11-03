using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Dash : Skill
{
    public bool canDash;
    [SerializeField] private UI_SkillSlot dashSlot;
    public bool canGenerateClone;
    [SerializeField] private UI_SkillSlot generateCloneSlot;
    [Space]
    [SerializeField] private float dashSpeed=15;
    [SerializeField] private float dashDuration=0.3f;
    protected override void Start()
    {
        base.Start();
        dashSlot.GetComponent<Button>().onClick.AddListener(()=>TryUnlockDash());
        generateCloneSlot.GetComponent<Button>().onClick.AddListener(()=>TryUnlockGenerateClone());
    }
    private void TryUnlockDash()
    {
        if (dashSlot.isUnlocked)
        {
            canDash= true;
        }
        else
        {
            canDash = false;
        }
    }
    private void TryUnlockGenerateClone()
    {
        if (generateCloneSlot.isUnlocked)
        {
            canGenerateClone = true;
        }
        else
        {
            canGenerateClone = false;
        }
    }
    public float getdashSpeed()
    {
        return this.dashSpeed;
    }
    public float getdashDuration() { 
        return this.dashDuration;
    }
    public void SetDashSpeed(float _value) { 
        dashSpeed= _value;
    }
}
