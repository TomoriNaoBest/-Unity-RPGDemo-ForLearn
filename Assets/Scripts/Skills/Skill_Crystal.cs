using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Crystal : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    public bool canUseCrystal;
    [SerializeField] UI_SkillSlot crystalSlot;
    [SerializeField] private float duration;

    [SerializeField] private bool canExchange;
    [SerializeField] UI_SkillSlot exchangeSlot;

    [SerializeField] private bool canExplode;
    [SerializeField] private UI_SkillSlot explodeSlot;

    [SerializeField] private float attackRadius;
    [SerializeField] private bool canGrow;
    [SerializeField] private float growSpeed;

    [SerializeField] private bool canMove;
    [SerializeField]private UI_SkillSlot moveSlot;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRadius;
    [Space]
    [Header("Multiple Crystal")]

    [SerializeField] private bool canMultiple;
    [SerializeField] private UI_SkillSlot multipleSlot;
    [SerializeField] private int multipleSize;
    [SerializeField] private int multipleCount;
    [SerializeField] private float reloadDuration;
    [SerializeField] private float resetDuration;
    private float reloadTimer=-1;
    private float resetTimer;
    private GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();
        crystalSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockCrystal());
        exchangeSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockExchange());
        explodeSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockExplode());
        moveSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockMove());
        multipleSlot.GetComponent<Button>().onClick.AddListener(() =>TryUnlockMultiple());
    }
    #region TryUnlockRegion
    private void TryUnlockCrystal()
    {
        if (crystalSlot.isUnlocked)
        {
            canUseCrystal = true;
        }
        else
        {
            canUseCrystal = false;
        }
    }
    private void TryUnlockExchange()
    {
        if (exchangeSlot.isUnlocked)
        {
            canExchange = true;
        }
        else
        {
            canExchange = false;
        }
    }
    private void TryUnlockExplode()
    {
        if (explodeSlot.isUnlocked)
        {
            canExplode = true;
        }else
        {
            canExplode= false;
        }
    }
    private void TryUnlockMove()
    {
        if (moveSlot.isUnlocked) {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }
    private void TryUnlockMultiple()
    {
        if (multipleSlot.isUnlocked) {
            canMultiple = true;
            if (multipleCount == 0)
            {
                multipleCount = multipleSize;
            }
            if (resetTimer == 0)
            {
                resetTimer = resetDuration;
            }
        }
        else
        {
            canMultiple = false;
        }
    }
    #endregion


    protected override void Update()
    {
        base.Update();
        if (reloadTimer >= 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        if (reloadTimer < 0&&multipleCount==0)
        {
            multipleCount = multipleSize;
        }
        if (resetTimer >= 0 && multipleCount>0 && multipleCount<multipleSize)
        {
            resetTimer -= Time.deltaTime;
        }
        if (resetTimer < 0 && reloadTimer<0)
        {
            multipleCount=multipleSize;
            resetTimer=resetDuration;
        }
       
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (canMultiple)
        {
            if (multipleCount > 0)
            {
                GameObject nowCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
                CryStalController controller = nowCrystal.GetComponent<CryStalController>();
                controller.SetUp(duration, canExplode, attackRadius, canGrow, growSpeed, canMove, moveSpeed, findCloestEnemy(nowCrystal.transform, moveRadius),player);
                multipleCount--;
            }
            if (multipleCount == 0 && reloadTimer<0)
            {
                reloadTimer=reloadDuration;
            }
            return;
        }
        if (currentCrystal == null)
        {
            currentCrystal=Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
            CryStalController controller=currentCrystal.GetComponent<CryStalController>();
            controller.SetUp(duration,canExplode,attackRadius,canGrow,growSpeed,canMove,moveSpeed,findCloestEnemy(currentCrystal.transform,moveRadius), player);
        }
        else
        {
            if (canExchange)
            {
                Vector3 playerPosition= player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPosition;
            }
            else
            {
                player.transform.position= currentCrystal.transform.position;
            }
        }
    }

}
