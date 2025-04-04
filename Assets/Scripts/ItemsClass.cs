using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;

    public enum ItemType
    {
        NoneWeapon = 0,
        Weapon = 1,

        NoneArmory = 2,
        Armory = 3,

        NoneAmulet = 4,
        Amulet = 5
    };

    public ItemType itemType;
    public string itemName;
    public string description;
    public int maxStack = 1;

    public Item(int new_id, ItemType new_itemType, string new_itemName, string new_description, int new_maxStack)
    {
        this.id = new_id;
        this.itemType = new_itemType;
        this.itemName = new_itemName;
        this.description = new_description;
        this.maxStack = new_maxStack;
    }
}
