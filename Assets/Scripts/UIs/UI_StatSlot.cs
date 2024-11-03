using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private StatType statType;
    [SerializeField]private TextMeshProUGUI StatValueText;
    [TextArea]
    [SerializeField] private string description;
    private UI_Switch ui_Switch;
    void Start()
    {
        ui_Switch=GetComponentInParent<UI_Switch>();
        UpdateStatSlot();
    }
    private void OnValidate()
    {
        gameObject.name="Stat-"+statType.ToString();
    }
    public void UpdateStatSlot()
    {
        PlayerStats playerStats = (PlayerStats)PlayerManager.instance.player.characterStats;
        StatValueText.text = playerStats.getStat(statType).getValue().ToString();
        switch (statType) { 
            case StatType.damage:
                StatValueText.text = playerStats.getLeastDamage().ToString();
                break;
            case StatType.maxHp:
                StatValueText.text=playerStats.getMaxHpValue().ToString();
                break;
            case StatType.magicRes:
                StatValueText.text = playerStats.getLeastMagicRes().ToString();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui_Switch.statToolTip.ShowStatToolTip(description,transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui_Switch.statToolTip.CloseStatToolTip();
    }
}
