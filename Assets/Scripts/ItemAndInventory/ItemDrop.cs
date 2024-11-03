using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private List<ItemData> ItemDropList = new List<ItemData>();
    private List<ItemData> DoDropList = new List<ItemData>();
    [SerializeField] private int maxDropNum;
    
    public void GenerateDrop()
    {
        int dropNum =Random.Range(0,maxDropNum+1);
        while (DoDropList.Count < dropNum)
        {
            for (int i = 0; i < ItemDropList.Count; i++)
            {
                if (Random.Range(0, 101) <= ItemDropList[i].dropChance * 100)
                {
                    DoDropList.Add(ItemDropList[i]);
                    if (DoDropList.Count == dropNum)
                    {
                        break;
                    }
                }
            }
        }
        for (int i = 0;i<DoDropList.Count; i++)
        {
            DropItem(DoDropList[i]);
        }
        
    }
    
    private void DropItem(ItemData _item)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(5, 10));
        newDrop.GetComponent<ItemObject>().SetUpItem(_item, randomVelocity);
    }
}
