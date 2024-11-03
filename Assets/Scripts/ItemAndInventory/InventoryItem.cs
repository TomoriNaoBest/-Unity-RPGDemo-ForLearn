using System;
[Serializable]
public class InventoryItem
{
    public ItemData itemdata;
    public int stackSize=0;
    public InventoryItem(ItemData _itemdata)
    {
        itemdata = _itemdata;
        stackSize++;
    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
