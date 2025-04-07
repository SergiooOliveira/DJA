using UnityEngine;
using System.Collections.Generic;

public class Inventory {

    [Header("Inventory")]
    public List<GameObject> InventoryItems { get; private set; }
    public int MaxSize { get; private set; }

    public Inventory(int maxSize) {

        MaxSize = maxSize;
        InventoryItems = new List<GameObject>(MaxSize);
    }
}
