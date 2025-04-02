using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("Icons UI")]
    public Transform icons;
    public GameObject upgradeObject;

    [Header("Level UI")]
    public TMP_Text level;
    public TMP_Text xpMax;

    [Header("Player UI")]
    public TMP_Text PlayerHP;
    public TMP_Text PlayerStrenght;
    public TMP_Text PlayerArmor;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    public TMP_Text inventoryText;

    [Header("Change Item")]
    public GameObject changeItemPanel;
    public bool isNewItem;
    public Item pendingItem; // Temporary storage for the new item

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Player.Instance.Initialize();
        UpdatePlayerStats();
        inventoryPanel.SetActive(false);
        changeItemPanel.SetActive(false);
    }

    public void UpdateUpgradesUI()
    {
        // Delete all previous icons
        foreach (Transform i in icons)
        {
            Destroy(i.gameObject);
        }

        // Showing all updgrades
        foreach (Upgrade upgrade in Upgrades.Instance.playerUpgrades)
        {
            GameObject newIcon = Instantiate(upgradeObject, icons);

            /*
             * Use this to change upgrade name as well
             * TMP_Text textComponent = newIcon.GetComponentInChildren<TMP_Text>();
             * if (textComponent != null) textComponent.text = upgrade.UpgradeName;
            */

            Image spriteComponent = newIcon.GetComponentInChildren<Image>();
            if (spriteComponent != null) spriteComponent.sprite = upgrade.Icon;
        }

    }

    public void AddStats(Upgrade upgrade)
    {
        switch (upgrade.BuffPower.Buff)
        {
            case "Health":
                Player.Instance.Health += upgrade.BuffPower.Power;                
                break;
            case "Strength":
                Player.Instance.Strenght += upgrade.BuffPower.Power;                
                break;
            case "Armor":
                Player.Instance.Armor += upgrade.BuffPower.Power;                
                break;
            default:
                Debug.LogError("Error applying buff");
                break;
        }

        UpdatePlayerStats();
    }

    public void UpdateLevelXP()
    {
        level.text = Player.Instance.Level.ToString();
        xpMax.text = (Player.Instance.Xp + "/" + Player.Instance.MaxXp);
    }

    private void UpdatePlayerStats()
    {
        PlayerHP.text = Player.Instance.Health.ToString();
        PlayerStrenght.text = Player.Instance.Strenght.ToString();
        PlayerArmor.text = Player.Instance.Armor.ToString();
    }

    public void InventoryPanel(bool activity)
    {
        inventoryText.text = Player.Instance.inventoryClass.GetItems();
        inventoryPanel.SetActive(activity);
    }

    public void ChangeItemPanel(bool activity)
    {
        changeItemPanel.SetActive(activity);
        isNewItem = false;
    }

    public void OldItemButton()
    {
        changeItemPanel.SetActive(false);
        isNewItem = false;
    }

    public void NewItemButton()
    {
        changeItemPanel.SetActive(false);
        isNewItem = true;

        InventoryClass inventory = Player.Instance.inventoryClass;

        // Use the pending item
        Item newItem = Instance.pendingItem;
        if (newItem == null)
        {
            Debug.LogError("No new item selected.");
            return;
        }

        // Find a replaceable slot
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            InventorySlot slot = inventory.slots[i];
            if (!slot.IsEmpty() && slot.CanStoreItem(newItem))
            {
                // Remove old item and replace it
                inventory.RemoveItem(i);
                slot.StoreItem(newItem.id);
                Debug.Log($"Replaced item with {newItem.itemName}.");
                break;
            }
        }

        // Clear pending item after replacement
        GameManager.Instance.pendingItem = null;

        // Update inventory display
        inventoryText.text = inventory.GetItems();
    }


}
