using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffEffect", menuName = "Data/ItemEffect/BuffEffect")]
public class ItemEffect_Buff : ItemEffect
{
    private CharacterStats playerStats;
    [SerializeField]private StatType statType;
    [SerializeField] private float amount;
    [SerializeField] private float duration;
    
    public override void doItemEffect()
    {
        playerStats = PlayerManager.instance.player.characterStats;
        playerStats.ChangeStatFor(amount, duration, playerStats.getStat(statType));
        Inventory.instance.RefreshStatSlotsAfter(duration);
    }
   

}
