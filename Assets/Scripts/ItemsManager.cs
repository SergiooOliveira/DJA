using UnityEngine;
using System.Collections.Generic;

public class InventorySlot
{
    public ItemType Type;
    public GameObject Item; // Can be null or reference a real item prefab
}

public class Inventory
{
    public List<InventorySlot> InventorySlots { get; private set; }
    public int MaxSize { get; private set; }

    public Inventory(int maxSize)
    {
        MaxSize = maxSize;
        InventorySlots = new List<InventorySlot>(MaxSize);

        for (int i = 0; i < MaxSize; i++)
        {
            InventorySlots.Add(new InventorySlot
            {
                Type = i switch
                {
                    0 => ItemType.MainHand,
                    1 => ItemType.OffHand,
                    2 => ItemType.Helmet,
                    3 => ItemType.ChestPlate,
                    4 => ItemType.LegsPlate,
                    5 => ItemType.FootWear,
                    6 => ItemType.Amulet,
                    _ => ItemType.Amulet,
                },
                Item = null
            });
        }
    }

    public void AddInventoryItem(GameObject itemObject)
    {
        var itemClass = itemObject.GetComponent<ItemClass>();

        foreach (var slot in InventorySlots)
        {
            if (slot.Type == itemClass.Type && slot.Item == null)
            {
                slot.Item = itemObject;
                itemObject.SetActive(false);
                return;
            }
        }

        Debug.LogWarning("No empty slot for item: " + itemClass.Name);
    }


}
