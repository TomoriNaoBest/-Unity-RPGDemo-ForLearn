using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private int ExpCost;
    [Space]
    public bool isUnlocked;
    [SerializeField] private UI_SkillSlot[] shouldBeUnLocked;
    [SerializeField] private UI_SkillSlot[] shouldBeLocked;
    [SerializeField]private UI_SkillSlot[] shouldRelateLock;
    [SerializeField] private Image skillImage;
    [SerializeField] private Color lockedColor;
    private void OnValidate()
    {
        gameObject.name="SkillSlot-"+skillName;
    }
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SkillSlotEvent());
    }
    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color =lockedColor;
        skillDescription += "\n需要:" + ExpCost+"EXP";
    }
    public void SkillSlotEvent()
    {
        if (!isUnlocked)
        {
            for (int i = 0; i < shouldBeUnLocked.Length; i++) {
                if (!shouldBeUnLocked[i].isUnlocked)
                {
                    Debug.Log("某个技能需要被解锁");
                    return;
                }
            }
            for (int i = 0; i < shouldBeLocked.Length; i++)
            {
                if (shouldBeLocked[i].isUnlocked)
                {
                    Debug.Log("某个技能需要被锁定");
                    return;
                }
            }
            PlayerStats playerStats=PlayerManager.instance.player.characterStats as PlayerStats;
            if (!playerStats.TryUseExp(ExpCost))
            {
                return;
            }
            GetComponentInParent<UI_Switch>().expslot.UpdateExpSlot();
            isUnlocked=true;
            skillImage.color= Color.white;
        }
        else
        {
            //已经解锁了，希望取消解锁并且退钱
            isUnlocked=false;
            skillImage.color=lockedColor;
            PlayerStats playerStats = PlayerManager.instance.player.characterStats as PlayerStats;
            playerStats.AddExp(ExpCost);
            GetComponentInParent<UI_Switch>().expslot.UpdateExpSlot();
            for (int i = 0; i < shouldRelateLock.Length; i++) {
                if (shouldRelateLock[i].isUnlocked)
                {
                    shouldRelateLock[i].GetComponent<Button>().onClick.Invoke();
                }
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Switch ui_switch= GetComponentInParent<UI_Switch>();
        ui_switch.skillToolTip.showSkillToolTip(skillName, skillDescription,transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInParent<UI_Switch>().skillToolTip.CloseSkillToolTip();
    }
}
