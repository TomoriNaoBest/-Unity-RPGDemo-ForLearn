using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Clone : Skill
{
    [SerializeField]private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float cloneDurationInBlackHole;
     private float drySpeed;
    [Header("Unlock Block")]
    public bool canClone;
    [SerializeField] UI_SkillSlot cloneSlot;
    public bool canAttack;
    [SerializeField] UI_SkillSlot attackSlot;
    public bool canMagic;
    [SerializeField] UI_SkillSlot magicSlot;

    protected override void Start()
    {
        base.Start();
        cloneSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockClone());
        attackSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockAttack());
        magicSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockMagic());

    }

    #region TryUnlockRegion
    private void TryUnlockClone()
    {
        canClone=cloneSlot.isUnlocked;
    }
    private void TryUnlockAttack()
    {
        canAttack = attackSlot.isUnlocked;
    }
    private void TryUnlockMagic()
    {
        canMagic=magicSlot.isUnlocked;
    }
    #endregion

    //Dryspped应该是duration的1/2 保证变透明同时clone销毁
    protected override void UseSkill()
    {
        base.UseSkill();
        GameObject clone= GameObject.Instantiate(clonePrefab);
        clone.GetComponent<CloneController>().SetupClone(player.transform.position,cloneDuration,canAttack,Vector3.zero,findCloestEnemy(clone.transform,5),player);
    }
    public void UseByblackHole(Transform _enemyTransfrom,bool _canAttack,Vector3 _offset,Transform _home)
    {
        GameObject clone = GameObject.Instantiate(clonePrefab,_home);
        clone.GetComponent<CloneController>().SetupClone(_enemyTransfrom.position, cloneDurationInBlackHole, _canAttack, _offset, findCloestEnemy(clone.transform, 5),player);
    }
    public float getCloneDurationInBlackHole()
    {
        return cloneDurationInBlackHole;
    }
}
