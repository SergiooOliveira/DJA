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
            new(Item.ItemType.Weapon),  // Slot 0: Weapon
            new(Item.ItemType.Armory),  // Slot 1: Armor
            new(Item.ItemType.Armory),  // Slot 2: Armor
            new(Item.ItemType.Amulet)   // Slot 3: Amulet
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

        GameManager.Instance.ChangeItemPanel(GameManager.Instance.changeItemPanel.activeSelf == false);
        if (GameManager.Instance.isNewItem)
        {
            foreach (var slot in slots)
            {
                switch(slot.allowedType)
                {
                    case Item.ItemType.NoneWeapon:
                        if (Item.ItemType.Weapon == newItem.itemType)
                        {
                            slot.StoreItem(itemId);
                            Debug.Log($"Added {newItem.itemName} to inventory.");
                            return;
                        }
                        break;
                    case Item.ItemType.NoneArmory:
                        if (Item.ItemType.Armory == newItem.itemType)
                        {
                            slot.StoreItem(itemId);
                            Debug.Log($"Added {newItem.itemName} to inventory.");
                            return;
                        }
                        break;
                    case Item.ItemType.NoneAmulet:
                        if (Item.ItemType.Amulet == newItem.itemType)
                        {
                            slot.StoreItem(itemId);
                            Debug.Log($"Added {newItem.itemName} to inventory.");
                            return;
                        }
                        break;
                }
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
