using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]protected Image image;
    [SerializeField] private TextMeshProUGUI AmountText;
    [SerializeField] protected Sprite defaultSprite;
    public InventoryItem inventoryItem;
    protected UI_ItemToolTip itemToolTip;

    protected virtual void Start()
    {
        itemToolTip= GetComponentInParent<UI_Switch>().itemToolTip;
    }
    public void UpdateSlot(InventoryItem _newItem)
    {
        inventoryItem = _newItem;
        if (inventoryItem != null)
        {
            image.color=Color.white;
            image.sprite = inventoryItem.itemdata.icon;
            if (inventoryItem.stackSize > 1)
            {
                AmountText.text = inventoryItem.stackSize.ToString();
            }
            else
            {
                AmountText.text = "";
            }
        }
    }
    public void ClearSlot()
    {
        inventoryItem = null;
        if (defaultSprite == null)
        {
            image.color = Color.clear;
        }
        else
        {
            image.sprite = defaultSprite;
        }
        AmountText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            if (inventoryItem.itemdata as ItemData_Equipment)
            {
                Inventory.instance.EquipItem(inventoryItem.itemdata as ItemData_Equipment);
                itemToolTip.CloseToolTip();
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            if (inventoryItem.itemdata as ItemData_Equipment)
            {
                itemToolTip.ShowToolTip(inventoryItem.itemdata as ItemData_Equipment,transform.position);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            if (inventoryItem.itemdata as ItemData_Equipment)
            {
                itemToolTip.CloseToolTip();
            }
        }
    }
}
