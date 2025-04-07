using UnityEngine;
using UnityEngine.UI;

public enum ItemType {

    MainHand,
    OffHand,
    Helmet,
    ChestPlate,
    LegsPlate,
    FootWear,
    Amulet,
}

public class ItemClass : MonoBehaviour {

    [Header("Item Variables")]
    public ItemType Type;
    public string Name;
    public string Description;
    public Image Image;
}