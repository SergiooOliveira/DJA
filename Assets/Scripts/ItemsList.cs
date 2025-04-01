using System.Collections.Generic;

public enum ItemId
{
    NoneWeapon = 0,
    NoneArmory = 1,
    NoneAmulet = 2,

    Sword = 3,
    Shield = 4,
}

public static class ItemsList
{
    private static readonly Dictionary<ItemId, Item> items = new()
    {
        { ItemId.NoneWeapon, new Item(0, Item.ItemType.NoneWeapon, "Nothing", "Nothing", 0) },
        { ItemId.NoneArmory, new Item(0, Item.ItemType.NoneArmory, "Nothing", "Nothing", 0) },
        { ItemId.NoneAmulet, new Item(0, Item.ItemType.NoneAmulet, "Nothing", "Nothing", 0) },

        { ItemId.Sword, new Item(0, Item.ItemType.Weapon, "Sword", "Sword", 1) },
        { ItemId.Shield, new Item(0, Item.ItemType.Armory, "Shield", "Shield", 1) },
    };

    public static Item GetItem(ItemId type) => items.TryGetValue(type, out Item item) ? item : null;
}
