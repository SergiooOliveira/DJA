using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryClass
{
    public int size;
    public List<ItemType> Items; // Store ItemType instead of Item objects

    public InventoryClass(int new_size)
    {
        size = new_size;
        Items = new List<ItemType>();
    }

    public void AddItem(ItemType itemType)
    {
        if (Items.Count < size)
        {
            Items.Add(itemType);
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < Items.Count)
        {
            return ItemsList.GetItem(Items[index]);
        }
        return null;
    }
}
