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
            new(Item.ItemType.Weapon),  // Slot 00: Weapon

            new(Item.ItemType.Armory),  // Slot 01: Armor
            new(Item.ItemType.Armory),  // Slot 02: Armor
            new(Item.ItemType.Armory),  // Slot 03: Armor
            new(Item.ItemType.Armory),  // Slot 04: Armor

            new(Item.ItemType.Amulet),  // Slot 05: Amulet
            new(Item.ItemType.Amulet),  // Slot 06: Amulet
            new(Item.ItemType.Amulet),  // Slot 07: Amulet
            new(Item.ItemType.Amulet),  // Slot 08: Amulet
            new(Item.ItemType.Amulet),  // Slot 09: Amulet
            new(Item.ItemType.Amulet),  // Slot 10: Amulet
        };
    }

    public void SetItemInSlot(int slotIndex, ItemId itemId)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count)
        {
            slots[slotIndex].StoreItem(itemId);
            Debug.Log($"Set slot {slotIndex} to item {itemId}.");
        }
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

    public string GetItems()
    {
        string s = "";

        foreach(InventorySlot slot in slots)
        {
            s += "- id: " + slot.storedItemId.ToString();
            s += "\n";
        }

        return s;
    }

}
