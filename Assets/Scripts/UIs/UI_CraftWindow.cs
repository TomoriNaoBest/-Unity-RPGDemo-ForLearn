using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_itemName;
    [SerializeField]private TextMeshProUGUI text_itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialsImage;
    [SerializeField] private Button craftButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupCraftWindow(ItemData_Equipment _itemdata)
    {
        craftButton.onClick.RemoveAllListeners();
        if (_itemdata.list_craftMaterials.Count > materialsImage.Length || _itemdata.list_craftMaterials.Count == 0)
        {
            Debug.LogWarning("该物品的合成配方不正确！");
        }

        //清空合成材料展示区
        for (int i = 0; i < materialsImage.Length; i++)
        {
            materialsImage[i].color = Color.clear;
            materialsImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }
        //提供信息
        for (int i = 0; i < _itemdata.list_craftMaterials.Count; i++) {
            materialsImage[i].sprite=_itemdata.list_craftMaterials[i].itemdata.icon;
            materialsImage[i].color= Color.white;
            TextMeshProUGUI text_Count = materialsImage[i].GetComponentInChildren<TextMeshProUGUI>();
            text_Count.color = Color.white;
            text_Count.text = _itemdata.list_craftMaterials[i].stackSize.ToString();

        }
        text_itemName.text = _itemdata.itemName;
        text_itemDescription.text=_itemdata.GetDescription();
        itemIcon.sprite=_itemdata.icon;
        craftButton.onClick.AddListener(() => Inventory.instance.canCraft(_itemdata));
    }
    
}
