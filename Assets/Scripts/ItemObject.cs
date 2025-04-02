using UnityEngine;

[SerializeField]
public class ItemObject : MonoBehaviour
{
    public ItemId id;

    public ItemObject(ItemId new_id)
    {
        ItemsList.GetItem(new_id);
    }
}
