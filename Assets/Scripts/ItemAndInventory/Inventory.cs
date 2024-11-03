using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //inventory是装备待定仓库 stash是合成用材料仓库 equipingItem是已经装备的表 只有四个位置存储信息
    public static Inventory instance;
    public List<ItemData> StartItemList=new List<ItemData>();
    public List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> invetoryDictionary = new Dictionary<ItemData, InventoryItem>();
    public List<InventoryItem> stashItemList= new List<InventoryItem>();
    public Dictionary<ItemData,InventoryItem> stashDictionary=new Dictionary<ItemData, InventoryItem>();
    public List<InventoryItem> equipingItemList= new List<InventoryItem>();
    public Dictionary<ItemData_Equipment,InventoryItem>equipingDictionary=new Dictionary<ItemData_Equipment,InventoryItem>();
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipingSlotParent;
    [SerializeField] private Transform statslotParent;
    private UI_ItemSlot[] UI_invenroty_slot;
    private UI_ItemSlot[] UI_stash_slot;
    private UI_EquipingSlot[] UI_equiping_slot;
    private UI_StatSlot[] UI_StatSlots;

    private float lastTimeUseFlask;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        UI_invenroty_slot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        UI_stash_slot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        UI_equiping_slot = equipingSlotParent.GetComponentsInChildren<UI_EquipingSlot>();
        UI_StatSlots= statslotParent.GetComponentsInChildren<UI_StatSlot>();
        LoadStartItems();
        UpdateSlotUI();
    }

    private void LoadStartItems()
    {
        foreach (var item in StartItemList)
        {
            AddItem(item);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < UI_equiping_slot.Length; i++)
        {
            UI_equiping_slot[i].ClearSlot();
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> kvp in equipingDictionary)
            {
                if (kvp.Key.equipmentType == UI_equiping_slot[i].equipmentType)
                {
                    UI_equiping_slot[i].UpdateSlot(kvp.Value);
                    break;
                }
            }
        }
        for (int i = 0; i < UI_invenroty_slot.Length; i++)
        {
            UI_invenroty_slot[i].ClearSlot();
        }
        for (int i = 0; i < UI_stash_slot.Length; i++)
        {
            UI_stash_slot[i].ClearSlot();
        }
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            UI_invenroty_slot[i].UpdateSlot(inventoryItemList[i]);
        }

        for (int i = 0; i < stashItemList.Count; i++)
        {
            UI_stash_slot[i].UpdateSlot(stashItemList[i]);
        }

        RefreshStatSlotsAfter(0);

    }
    #region RefreshStatSlots
    public void RefreshStatSlotsAfter(float _seconds)
    {
        StartCoroutine("DorefreshStatSlots", _seconds);  
    }
    private IEnumerator DorefreshStatSlots(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        for (int i = 0; i < UI_StatSlots.Length; i++)
        {
            UI_StatSlots[i].UpdateStatSlot();
        }
    }
    #endregion

    public void AddItem(ItemData _item)
    {
        if (invetoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else if(stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            stashValue.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            switch (_item.itemType)
            {
                case ItemType.Material:
                    stashItemList.Add(newItem);
                    stashDictionary.Add(_item, newItem);
                    break;
                case ItemType.Equipment:
                    inventoryItemList.Add(newItem);
                    invetoryDictionary.Add(_item, newItem);
                    break;
            }
        }
        UpdateSlotUI();
    }
    public void RemoveItem(ItemData _item)
    {
        if (invetoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItemList.Remove(value);
                invetoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
            UpdateSlotUI();
            return;
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= 1)
            {
                stashItemList.Remove(value);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashvalue.RemoveStack();
            }
            UpdateSlotUI();
            return;
        }
        

    }
    public void EquipItem(ItemData_Equipment _equipment)
    {
       foreach(KeyValuePair<ItemData_Equipment,InventoryItem> kvp in equipingDictionary)
        {
            if (kvp.Key.equipmentType == _equipment.equipmentType)
            {
                UnEquipItem(kvp.Key);
                AddItem(kvp.Key);
                break;
            }
        }
        InventoryItem newItem = new InventoryItem(_equipment);
        equipingItemList.Add(newItem);
        equipingDictionary.Add(_equipment, newItem);
        _equipment.AddModifier();
        RemoveItem(_equipment);

    }
    public void UnEquipItem(ItemData_Equipment itemData_Equipment)
    {
        InventoryItem item= null;
        if(equipingDictionary.TryGetValue(itemData_Equipment,out item))
        {
            equipingItemList.Remove(item);
        }
        equipingDictionary.Remove(itemData_Equipment);
        itemData_Equipment.RemoveModifier();
    }
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> kvp in equipingDictionary)
        {
            if (kvp.Key.equipmentType == _type)
            {
                return kvp.Key;
            }
        }
        return null;
    }
    public void UseFlask()
    {
        ItemData_Equipment currentFlask=GetEquipment(EquipmentType.药水瓶);
        if (currentFlask != null) {
            bool canUseFlask = Time.time >= lastTimeUseFlask + currentFlask.itemCoolDown;
            if (canUseFlask)
            {
                currentFlask.ExecuteEffects();
                lastTimeUseFlask = Time.time;
                RefreshStatSlotsAfter(0);
            }
        }
    }

    public bool canCraft(ItemData_Equipment _targetData)
    {
        int enoughCount = 0;
        foreach(InventoryItem materials in _targetData.list_craftMaterials)
        {
            for (int i = 0; i < stashItemList.Count; i++) {
                if (materials.itemdata == stashItemList[i].itemdata)
                {
                    if (stashItemList[i].stackSize >= materials.stackSize)
                    {
                        enoughCount++;
                    }
                }
            }
        }
        if (enoughCount == _targetData.list_craftMaterials.Count) {
            //变更库存内容，实现合成
            doCraft(_targetData);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void doCraft(ItemData_Equipment _targetData)
    {
        List<InventoryItem> list_craftMaterials=_targetData.list_craftMaterials;
        for (int i = 0; i < list_craftMaterials.Count; i++)
        {
            for (int j = 0; j < list_craftMaterials[i].stackSize; j++)
            {
                RemoveItem(list_craftMaterials[i].itemdata);
            }
        }
        AddItem(_targetData);
    }
   


}
