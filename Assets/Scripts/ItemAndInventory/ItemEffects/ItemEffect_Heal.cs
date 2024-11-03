using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealEffect", menuName = "Data/ItemEffect/HealEffect")]
public class ItemEffect_Heal : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;
    public override void doItemEffect()
    {
        CharacterStats characterStats = PlayerManager.instance.player.characterStats;
        int healAmount=Mathf.RoundToInt(characterStats.getMaxHpValue()*healPercent);
        characterStats.IncreaseCurrentHp(healAmount);
    }
}
