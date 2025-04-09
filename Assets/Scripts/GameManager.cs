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
        Player.Instance.Initialize(Player.Instance.baseName, Player.Instance.baseHealth, Player.Instance.baseStrenght,
                                    Player.Instance.baseArmor, Player.Instance.baseLevel, Player.Instance.baseXp, Player.Instance.baseMaxXp);
        UpdatePlayerStats();

        Enemies.Instance.StartWave();
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
}
