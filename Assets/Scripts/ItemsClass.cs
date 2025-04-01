using UnityEngine;

[System.Serializable]
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public int maxStack = 1;

    public Item(int new_id, string new_itemName, string new_description, int new_maxStack) {
        this.id = new_id;
        this.itemName = new_itemName;
        this.description = new_description;
        this.maxStack = new_maxStack;
    }
}
