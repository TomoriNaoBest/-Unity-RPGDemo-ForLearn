using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_itemName;
    [SerializeField]private TextMeshProUGUI text_itemType;
    [SerializeField] private TextMeshProUGUI text_itemDescription;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowToolTip(ItemData_Equipment _itemdata,Vector2 _slotPosition)
    {
        if (_itemdata != null)
        {
            text_itemName.text = _itemdata.itemName;
            text_itemType.text=_itemdata.equipmentType.ToString();
            text_itemDescription.text = _itemdata.GetDescription();

            
            float xOffset = -200;
            transform.position = new Vector2(_slotPosition.x + xOffset, _slotPosition.y + 120);
            gameObject.SetActive(true);
        }
    }
    public void CloseToolTip()
    {
        gameObject.SetActive(false);
    }
}
