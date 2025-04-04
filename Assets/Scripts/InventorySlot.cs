[System.Serializable]
public class InventorySlot
{
    public Item.ItemType allowedType;  // Determines what can be stored
    public ItemId? storedItemId;  // Nullable to represent empty slots

    public InventorySlot(Item.ItemType type)
    {
        allowedType = type;
        storedItemId = null;  // Empty slot
    }

    public bool IsEmpty() => storedItemId == null;

    public bool CanStoreItem(Item newItem)
    {
        return newItem != null && newItem.itemType == allowedType;
    }

    public void StoreItem(ItemId itemId)
    {
        storedItemId = itemId;
    }

    public void ClearSlot()
    {
        storedItemId = null;
    }
}
