
using Cinemachine;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public enum EquipmentType
{
    武器,
    护甲,
    饰品,
    药水瓶
}
[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
  public EquipmentType equipmentType;
    public float itemCoolDown;
    [Header("Major stats")]
    public float strength;//1点力量增加3点攻击力
    public float agility;//一点敏捷增加1%闪避
    public float intelligence;//一点智力增加3%法抗和3点法伤
    public float vitality;// 1点活力增加5点生命上限
    [Header("Offensive stats")]
    public float damage;
    public float criticalDamage;//暴击伤害 是1.5这种数
    public float criticalChance;//暴击率 百分比数0.x
    [Header("Defensive stats")]
    public float maxHp;
    public float armor;//百分比数 
    public float evasion;//回避 闪避 百分比数
    public float magicResistance;//百分比魔抗小数
    [Header("Magic stats")]
    public float fireDamage;
    public float iceDamage;
    public float lightingDamage;
    [Header("ItemEffects")]
    public ItemEffect[] itemEffects;
    private int descriptionLength;
    [Space]
    [TextArea]
    [SerializeField] private string EquipmentDescription;
    [Space]
    [Header("CraftInfo")]
    public List<InventoryItem> list_craftMaterials=new List<InventoryItem>();


    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.characterStats as PlayerStats;
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.damage.AddModifier(damage);
        playerStats.criticalDamage.AddModifier(criticalDamage);
        playerStats.criticalChance.AddModifier(criticalChance);
        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }
    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.instance.player.characterStats as PlayerStats;
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.criticalDamage.RemoveModifier(criticalDamage);
        playerStats.criticalChance.RemoveModifier(criticalChance);
        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }
    public void ExecuteEffects()
    {
        foreach(var effects in itemEffects)
        {
            effects.doItemEffect();
        }
    }
    #region Description
    public override string GetDescription()
    {
        stringBuilder.Length = 0;
        descriptionLength = 0;
        AddItemDescription(strength,"力量");
        AddItemDescription(agility, "敏捷");
        AddItemDescription(intelligence, "智慧");
        AddItemDescription(vitality, "活力");

        AddItemDescription(damage, "攻击力");
        AddItemDescription(criticalChance, "暴击率");
        AddItemDescription(criticalDamage, "暴击伤害");

        AddItemDescription(maxHp, "最大生命值");
        AddItemDescription(evasion, "闪避几率");
        AddItemDescription(armor, "护甲");
        AddItemDescription(magicResistance, "魔抗");

        AddItemDescription(fireDamage, "火焰伤害");
        AddItemDescription(iceDamage, "冰冻伤害");
        AddItemDescription(lightingDamage, "闪电伤害");

        if (EquipmentDescription.Length > 0) { 
            stringBuilder.AppendLine();
            stringBuilder.Append(EquipmentDescription);
        }

        if (descriptionLength < 3)
        {
            for (int i = 0; i < 3 - descriptionLength; i++)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("");
            }
        }
        return stringBuilder.ToString();
    }
    private void AddItemDescription(float _value,string _name)
    {
        if (_value != 0)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
                descriptionLength++;
            }
            stringBuilder.Append("+"+_value+" "+_name);
            
        }
    }
    #endregion
}
