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
        Items = new List<ItemType>(new_size); // Initialize with capacity

        // Fill inventory with "None" in every slot
        for (int i = 0; i < size; i++)
        {
            Items.Add(ItemType.None);
        }
    }

    public void AddItem(ItemType itemType)
    {
        // Find the first empty (None) slot and replace it
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == ItemType.None)
            {
                Items[i] = itemType;
                Debug.Log($"Added {itemType} to inventory at slot {i}.");
                return;
            }
        }

        Debug.Log("Inventory is full! Cannot add " + itemType);
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
