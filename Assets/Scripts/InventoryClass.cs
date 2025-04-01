using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryClass
{
    public int size;
    public List<ItemId> Items;

    public InventoryClass(int new_size)
    {
        size = new_size;
        Items = new List<ItemId>(new_size);

        // Fill inventory with "None" in every slot
        for (int i = 0; i < size; i++)
        {
            Items.Add((i == 0) ? ItemId.NoneWeapon : (i < 3) ? ItemId.NoneArmory : ItemId.NoneAmulet);
        }
    }

    public void AddItem(ItemId itemId)
    {
        Item item = ItemsList.GetItem(itemId);

        if (item == null)
        {
            Debug.LogError($"Invalid itemId: {itemId}");
            return;
        }

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == ItemId.NoneWeapon && item.itemType == Item.ItemType.Weapon)
            {
                Items[i] = itemId;
                Debug.Log($"Added {itemId} to inventory at slot {i}.");
                return;
            }
            else if ((Items[i] == ItemId.NoneArmory || Items[i] == ItemId.NoneAmulet) &&
                     item.itemType != Item.ItemType.Weapon)
            {
                Items[i] = itemId;
                Debug.Log($"Added {itemId} to inventory at slot {i}.");
                return;
            }
        }

        Debug.Log("Inventory is full! Cannot add " + itemId);
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
