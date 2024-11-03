
using Cinemachine;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public enum EquipmentType
{
    ����,
    ����,
    ��Ʒ,
    ҩˮƿ
}
[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
  public EquipmentType equipmentType;
    public float itemCoolDown;
    [Header("Major stats")]
    public float strength;//1����������3�㹥����
    public float agility;//һ����������1%����
    public float intelligence;//һ����������3%������3�㷨��
    public float vitality;// 1���������5����������
    [Header("Offensive stats")]
    public float damage;
    public float criticalDamage;//�����˺� ��1.5������
    public float criticalChance;//������ �ٷֱ���0.x
    [Header("Defensive stats")]
    public float maxHp;
    public float armor;//�ٷֱ��� 
    public float evasion;//�ر� ���� �ٷֱ���
    public float magicResistance;//�ٷֱ�ħ��С��
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
        AddItemDescription(strength,"����");
        AddItemDescription(agility, "����");
        AddItemDescription(intelligence, "�ǻ�");
        AddItemDescription(vitality, "����");

        AddItemDescription(damage, "������");
        AddItemDescription(criticalChance, "������");
        AddItemDescription(criticalDamage, "�����˺�");

        AddItemDescription(maxHp, "�������ֵ");
        AddItemDescription(evasion, "���ܼ���");
        AddItemDescription(armor, "����");
        AddItemDescription(magicResistance, "ħ��");

        AddItemDescription(fireDamage, "�����˺�");
        AddItemDescription(iceDamage, "�����˺�");
        AddItemDescription(lightingDamage, "�����˺�");

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
