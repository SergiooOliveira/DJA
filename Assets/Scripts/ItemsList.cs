using System.Collections.Generic;

public enum ItemType
{
    None = 0,
    Sword = 1,
    Shield = 2,
}

public static class ItemsList
{
    private static readonly Dictionary<ItemType, Item> items = new()
    {
        { ItemType.None, new Item(0, "Nothing", "Nothing", 0) },
        { ItemType.Sword, new Item(1, "Sword", "Sword", 1) },
        { ItemType.Shield, new Item(2, "Shield", "Shield", 1) }
    };

    public static Item GetItem(ItemType type) => items.TryGetValue(type, out Item item) ? item : null;
}
