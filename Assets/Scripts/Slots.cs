using UnityEngine;

public class Slots : MonoBehaviour
{
    [Header("Empty GameObjects (slots on hands)")]
    public Transform rightHandSlot;  
    public Transform leftHandSlot;   

    [Header("Item Prefabs")]
    public GameObject weaponPrefab;
    public GameObject shieldPrefab;

    private GameObject equippedWeapon;
    private GameObject equippedShield;

    public InventoryClass inventory;

    public void UpdateVisuals()
    {
        Item weaponItem = inventory.GetItem(0);
        if (weaponItem != null && weaponItem.itemType == Item.ItemType.Weapon)
        {
            if (equippedWeapon != null)
                Destroy(equippedWeapon);

            if (weaponItem.itemName == "Sword")
            {
                equippedWeapon = Instantiate(weaponPrefab, rightHandSlot);
                equippedWeapon.transform.localPosition = Vector3.zero;
                equippedWeapon.transform.localRotation = Quaternion.identity;
            }
        }

        Item shieldItem = inventory.GetItem(1);
        if (shieldItem != null && shieldItem.itemType == Item.ItemType.Armory)
        {
            if (equippedShield != null)
                Destroy(equippedShield);

            if (shieldItem.itemName == "Shield")
            {
                equippedShield = Instantiate(shieldPrefab, leftHandSlot);
                equippedShield.transform.localPosition = Vector3.zero;
                equippedShield.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
