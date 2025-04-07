using TMPro;
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
    public ItemObject pendingItem; // Temporary storage for the new item

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

    public void ChangeItemPanel(bool activity, ItemObject itemObject)
    {
        pendingItem = itemObject;

        Time.timeScale = 0f;
        changeItemPanel.SetActive(activity);
    }

    public void OldItemButton()
    {
        FinalizeChange();
    }

    private bool IsNoneItemFor(ItemId slotItemId, Item pending)
    {
        switch (pending.itemType)
        {
            case Item.ItemType.Weapon:
                return slotItemId == ItemId.NoneWeapon;
            case Item.ItemType.Armory:
                return slotItemId == ItemId.NoneArmory;
            case Item.ItemType.Amulet:
                return slotItemId == ItemId.NoneAmulet;
            default:
                return false;
        }
    }

    private void FinalizeChange()
    {
        Time.timeScale = 1f;
        changeItemPanel.SetActive(false);
    }

    public void NewItemButton()
    {
        InventoryClass inventory = Player.Instance.inventoryClass;
        Item pending = ItemsList.GetItem(pendingItem.id);

        int fallbackSlotIndex = -1;

        // Step 1: Scan all slots to find the best one to replace
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            var slot = inventory.slots[i];

            // Only process slots that can store the pending item
            if (!slot.CanStoreItem(pending))
                continue;

            // Check if the slot holds a default/None item that matches the pending item's type
            if (slot.storedItemId.HasValue && IsNoneItemFor(slot.storedItemId.Value, pending))
            {
                // This is an ideal empty slot; use it immediately
                inventory.RemoveItem(i); // or fallbackSlotIndex
                inventory.SetItemInSlot(i, pendingItem.id);
                FinalizeChange();
                return;
            }
            // If we haven't saved a fallback yet, record this slot as a candidate
            else if (fallbackSlotIndex == -1)
            {
                fallbackSlotIndex = i;
            }
        }

        // Step 2: No matching "None" slot found, use fallback slot if available
        if (fallbackSlotIndex != -1)
        {
            inventory.RemoveItem(fallbackSlotIndex);
            inventory.AddItem(pendingItem.id);
        }

        FinalizeChange();
    }


}
