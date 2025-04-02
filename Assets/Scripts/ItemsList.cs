using System.Collections.Generic;

public enum ItemId
{
    NoneWeapon = 0,
    NoneArmory = 1,
    NoneAmulet = 2,

    Sword = 3,
    Dagger = 4,
    Shield = 5,
    Amulet = 6,
}

public static class ItemsList
{
    private static readonly Dictionary<ItemId, Item> items = new()
    {
        { ItemId.NoneWeapon, new(ItemId.NoneWeapon, Item.ItemType.NoneWeapon, "Nothing", "Nothing", 0) },
        { ItemId.NoneArmory, new(ItemId.NoneArmory, Item.ItemType.NoneArmory, "Nothing", "Nothing", 0) },
        { ItemId.NoneAmulet, new(ItemId.NoneAmulet, Item.ItemType.NoneAmulet, "Nothing", "Nothing", 0) },

        { ItemId.Sword, new(ItemId.Sword, Item.ItemType.Weapon, "Sword", "Sword", 1) },
        { ItemId.Dagger, new(ItemId.Dagger, Item.ItemType.Weapon, "Dagger", "Dagger", 1) },
        { ItemId.Shield, new(ItemId.Shield, Item.ItemType.Armory, "Shield", "Shield", 1) },
        { ItemId.Amulet, new(ItemId.Amulet, Item.ItemType.Amulet, "Amulet", "Amulet", 1) },
    };

    public static Item GetItem(ItemId type) => items.TryGetValue(type, out Item item) ? item : null;
}
