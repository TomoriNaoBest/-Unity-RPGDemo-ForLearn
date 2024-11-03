using System.Text;
using UnityEngine;
public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData :ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    [Range(0f,1f)]
    public float dropChance=0;

    protected StringBuilder stringBuilder=new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}
