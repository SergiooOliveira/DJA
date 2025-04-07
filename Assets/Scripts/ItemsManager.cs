using UnityEngine;
using System.Collections.Generic;

public class ItemsInicialization : MonoBehaviour {
    private void Awake() {
        AllItems.AddItem(new Item(ItemType.MainHand, "Sword", "Sword"));
        AllItems.AddItem(new Item(ItemType.MainHand, "Dagger", "Dagger"));

        AllItems.AddItem(new Item(ItemType.OffHand, "Small Shield", "Small Shield"));
        AllItems.AddItem(new Item(ItemType.OffHand, "Big Shield", "Big Shield"));

        AllItems.AddItem(new Item(ItemType.Helmet, "Helmet", "Helmet"));
        AllItems.AddItem(new Item(ItemType.ChestPlate, "Chest Plate", "Chest Plate"));
        AllItems.AddItem(new Item(ItemType.LegsPlate, "Legs Plate", "Legs Plate"));
        AllItems.AddItem(new Item(ItemType.FootWear, "Foot Wear", "Foot Wear"));
    }
}

public enum ItemType {
    MainHand,
    OffHand,
    Helmet,
    ChestPlate,
    LegsPlate,
    FootWear
}
public class Item {
    public ItemType Type { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Item(ItemType type, string name, string description) {
        Type = type;
        Name = name;
        Description = description;
    }
}

public static class AllItems
{
    public static List<Item> allItems = new();

    public static void AddItem(Item newItem) => allItems.Add(newItem);

    public static Item GetItem(int index) => allItems[index];
}

public class Inventory {
    private readonly List<Item> inventoryItems;
    private readonly int maxSize;

    public Inventory(int newSize) {
        maxSize = newSize;
        inventoryItems = new List<Item>(newSize);
    }

    public void AddItem(Item newItem) {
        if (inventoryItems.Count < maxSize) inventoryItems.Add(newItem);
    }

    public string GetItems() {
        string s = "";

        for (int i = 0; i < inventoryItems.Count; i++) {
            s += "Item " + i;
            s += "\n";
            s += "- Name: " + inventoryItems[i].Name;
            s += "\n";
            s += "- Type: " + inventoryItems[i].Type;
            s += "\n";
            s += "- Description: " + inventoryItems[i].Description;
            s += "\n";
            s += "\n";
        }

        return s;
    }
}
