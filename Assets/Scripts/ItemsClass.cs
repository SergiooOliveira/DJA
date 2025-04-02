using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ItemId id;

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

    public Item(ItemId new_id, ItemType new_itemType, string new_itemName, string new_description, int new_maxStack)
    {
        id = new_id;
        itemType = new_itemType;
        itemName = new_itemName;
        description = new_description;
        maxStack = new_maxStack;
    }
}
