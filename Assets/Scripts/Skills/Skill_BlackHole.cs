using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_BlackHole : Skill
{
    [Header("Unlock Info")]
    public bool canBlackHole;
    [SerializeField] private UI_SkillSlot blackholeSlot;
    [Header("BlackHole Info")]
    [SerializeField] private GameObject BlackHolePrefab;
    [SerializeField]private float maxSize;
    [SerializeField]private float growSpeed;
    [SerializeField]private float shrinkSpeed;
    [SerializeField] private int attackTimes;
    [SerializeField] private float ExistDuration;
    [SerializeField] private List<KeyCode> KeyList;
    [Header("PlayerBlackHoleState Info")]
    [SerializeField] private float flyDuration;
    [SerializeField] private float flySpeed;
    protected override void Start()
    {
        base.Start();
        blackholeSlot.GetComponent<Button>().onClick.AddListener(()=>TryUnlockBlackHole());
    }
    private void TryUnlockBlackHole()
    {
        canBlackHole=blackholeSlot.isUnlocked;
    }
    protected override void UseSkill()
    {
       base.UseSkill();
       GameObject blackhole= GameObject.Instantiate(BlackHolePrefab);
       BlackHoleController controller= blackhole.GetComponent<BlackHoleController>();
        List<KeyCode> KeyListTemp = new List<KeyCode>(KeyList);
       controller.SetUp(player.transform,maxSize,growSpeed,shrinkSpeed,attackTimes,KeyListTemp, ExistDuration);
    }
    public float getFlyDuration()
    {
        return flyDuration;
    }
    public float getFlySpeed()
    {
        return flySpeed;
    }
}
