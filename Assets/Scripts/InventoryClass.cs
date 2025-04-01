using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryClass
{
    public List<InventorySlot> slots;

    public InventoryClass()
    {
        slots = new List<InventorySlot>
        {
            new InventorySlot(Item.ItemType.Weapon),  // Slot 0: Weapon
            new InventorySlot(Item.ItemType.Armory),  // Slot 1: Armor
            new InventorySlot(Item.ItemType.Armory),  // Slot 2: Armor
            new InventorySlot(Item.ItemType.Amulet)   // Slot 3: Amulet
        };
    }

    public void AddItem(ItemId itemId)
    {
        Item newItem = ItemsList.GetItem(itemId);
        if (newItem == null)
        {
            Debug.LogError($"Invalid itemId: {itemId}");
            return;
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty() && slot.CanStoreItem(newItem))
            {
                slot.StoreItem(itemId);
                Debug.Log($"Added {newItem.itemName} to inventory.");
                return;
            }
        }

        Debug.Log("Inventory is full! Cannot add " + newItem.itemName);
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count && !slots[slotIndex].IsEmpty())
        {
            Debug.Log($"Removed {slots[slotIndex].storedItemId} from inventory.");
            slots[slotIndex].ClearSlot();
        }
        else
        {
            Debug.Log("Slot is already empty or invalid.");
        }
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < slots.Count && !slots[index].IsEmpty())
        {
            return ItemsList.GetItem(slots[index].storedItemId.Value);
        }
        return null;
    }
}
