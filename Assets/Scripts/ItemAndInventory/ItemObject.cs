using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject :MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private Rigidbody2D rb=>GetComponent<Rigidbody2D>();
    private void OnValidate()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite=itemData.icon;
        gameObject.name="ItemObject-"+itemData.itemName;
    }
   public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
    public void SetUpItem(ItemData item,Vector2 _velocity)
    {
        itemData=item;
        rb.velocity=_velocity;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "ItemObject-" + itemData.itemName;

    }
}
