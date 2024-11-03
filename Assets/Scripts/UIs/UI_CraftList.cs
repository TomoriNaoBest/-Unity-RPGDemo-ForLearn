using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField]private List<ItemData_Equipment> list_craftEquipment;
    [SerializeField] private List<UI_CraftSlot> list_craftSlots;
    [SerializeField]bool isFirst=false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (isFirst)
        {
            SetUpCraftList();
        }
    }
    public void SetUpCraftList()
    {
        AssignCraftSlots();
        for (int i = 0; i < list_craftSlots.Count; i++)
        {
            Destroy(list_craftSlots[i].gameObject);
        }
        list_craftSlots = new List<UI_CraftSlot>();
        for (int i = 0;i<list_craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(list_craftEquipment[i]);
        }
    }
    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            list_craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
       SetUpCraftList();
    }
}
