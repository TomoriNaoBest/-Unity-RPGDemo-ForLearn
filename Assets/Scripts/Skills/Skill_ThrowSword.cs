using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_ThrowSword : Skill
{
    [Header("Lock Control")]
    public bool canThrow;
    [SerializeField] private UI_SkillSlot throwSlot;
    public bool canStopTime;
    [SerializeField] private UI_SkillSlot stopTimeSlot;
    public bool canWeaken;
    [SerializeField] private UI_SkillSlot weakenSlot;
    [SerializeField] private UI_SkillSlot spinSlot;
    [SerializeField] private UI_SkillSlot pierceSlot;
    [SerializeField] private UI_SkillSlot bounceSlot;


    [Header("Skill Info")]
    [SerializeField]private SwordType swordType;
    [SerializeField]private GameObject swordPrefab;
    [SerializeField] private Vector2 lanuchForce;
    [SerializeField] private float gravity;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float returnImpact;
    [SerializeField] private float freezeTime;
    [SerializeField] private float weakenPercent;
    [SerializeField]private float weakenTime;
    [Header("Bounce Info")]
    [SerializeField] private int bounceTime;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceRadius;
    [Header("Pierce Info")]
    [SerializeField] private int pierceTime;
    [SerializeField] private float pierceGravity;
    [Header("Spin Info")]
    [SerializeField]private float maxTraveDistance;
    [SerializeField]private float spinDuration;
    [SerializeField] private float hitCoolWindow;
    private Vector2 finalForece;

    [Header("Aim Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    [SerializeField] private int numberOfdots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField]private Transform setPoint;
    private GameObject[] dots;

    private enum SwordType
    {
        General,
        Bounce,
        Pierce,
        Spin
    }


    protected override void Start()
    {
        base.Start();
        GenerateDots();
        throwSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockThrow());
        stopTimeSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockTimeStop());
        weakenSlot.GetComponent<Button>().onClick.AddListener(() => TryUnlockWeaken());
        spinSlot.GetComponent<Button>().onClick.AddListener(() => TryModel());
        pierceSlot.GetComponent<Button>().onClick.AddListener(() => TryModel());
        bounceSlot.GetComponent<Button>().onClick.AddListener(() => TryModel());

    }

    #region TryUnlockRegion
    private void TryUnlockThrow()
    {
       canThrow=throwSlot.isUnlocked;
    }
    private void TryUnlockTimeStop()
    {
        canStopTime=stopTimeSlot.isUnlocked;
    }
    private void TryUnlockWeaken()
    {
        canWeaken=weakenSlot.isUnlocked;
    }
    private void TryModel()
    {
        if (spinSlot.isUnlocked)
        {
            swordType=SwordType.Spin;
            return;
        }
        if (pierceSlot.isUnlocked)
        {
            swordType = SwordType.Pierce;
            return;
        }
        if (bounceSlot.isUnlocked)
        {
            swordType=SwordType.Bounce;
            return;
        }
        swordType=SwordType.General;
    }
    
    #endregion


    protected override void Update()
    {
        base.Update();
        if (!canThrow)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        { 
           finalForece=new Vector2(AimDirection().normalized.x*lanuchForce.x,AimDirection().normalized.y*lanuchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < numberOfdots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    protected override void UseSkill()
    {
       base.UseSkill();
        GameObject newSword = Instantiate(swordPrefab, setPoint.position, transform.rotation);
        player.AssignSword(newSword);
        SwordController swordController=newSword.GetComponent<SwordController>();
        swordController.SetUpBasic(player,finalForece,gravity,returnSpeed,freezeTime);
        
        switch (swordType)
        {
            case SwordType.Pierce:
                swordController.SetUpPierceInfo(pierceTime, pierceGravity);
                break;
            case SwordType.Bounce:
                swordController.SetUpBounceInfo(bounceTime, bounceSpeed, bounceRadius);
                break;
            case SwordType.Spin:
                swordController.SetUpSpinInfo(maxTraveDistance,spinDuration,hitCoolWindow);
                break;
        }
    }
    #region AimRegion
    private Vector2 AimDirection()
    {
        Vector2 playerPosition=setPoint.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction=mousePosition- playerPosition;
        return direction;
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfdots];
        for (int i = 0; i < numberOfdots; i++)
        {
            dots[i] = Instantiate(dotPrefab,setPoint.position,Quaternion.identity,dotsParent);
            dots[i].SetActive(false);
        }

    }

    public void SetDotsActive(bool _isActive)
    {
        for (int i = 0; i < numberOfdots; i++) {
            dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        float g;
        if (swordType==SwordType.Pierce)
        {
            g = pierceGravity;
        }
        else
        {
            g = gravity;
        }
        Vector2 position= (Vector2)setPoint.position + new Vector2(
            AimDirection().normalized.x*lanuchForce.x,
            AimDirection().normalized.y * lanuchForce.y) * t + .5f * (Physics2D.gravity*g)*t*t;
        return position;
    }
    #endregion
    public float getReturnImpact()
    {
        return returnImpact;
    }
    public float getWeakenPercent()
    {
        return weakenPercent;
    }
    public float getWekanTime()
    {
        return weakenTime;
    }
   
}
