using UnityEngine;
using UnityEngine.UI;

public enum ItemType {

    MainHand,
    OffHand,
    Helmet,
    ChestPlate,
    LegsPlate,
    FootWear
}

public class ItemClass : MonoBehaviour {

    [Header("Item Variables")]
    public ItemType Type;
    public string Name;
    public string Description;
    public Image Image;

    public ItemClass(ItemType type, string name, string description)     {
        Type = type;
        Name = name;
        Description = description;
    }
}