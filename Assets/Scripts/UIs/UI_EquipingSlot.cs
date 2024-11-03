using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipingSlot :UI_ItemSlot
{
    public EquipmentType equipmentType;
    
    private void OnValidate()
    {
       gameObject.name= "EquipingSlot-"+equipmentType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            Inventory.instance.UnEquipItem(inventoryItem.itemdata as ItemData_Equipment);
            Inventory.instance.AddItem(inventoryItem.itemdata as ItemData_Equipment);
            itemToolTip.CloseToolTip();
        }
        
    }
}
