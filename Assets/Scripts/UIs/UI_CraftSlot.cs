using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    [SerializeField] private TextMeshProUGUI text_itemName;

    public void SetUpCraftSlot(ItemData_Equipment _itemData)
    {
        inventoryItem.itemdata = _itemData;
        image.sprite=_itemData.icon;
        text_itemName.text = _itemData.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData=inventoryItem.itemdata as ItemData_Equipment;
        UI_CraftWindow ui_craftWindow=GetComponentInParent<UI_Switch>().ui_CraftWindow;
        ui_craftWindow.SetupCraftWindow(craftData);
    }
}
