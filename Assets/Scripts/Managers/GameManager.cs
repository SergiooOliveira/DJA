using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance; // Singleton

    private GameObject canvas; // Main Canvas object

    [Header("Upgrade Icons UI")]
    public Transform icons;
    public GameObject upgradeObject;

    [Header("PowerUps UI")]
    public Transform powerUps;
    public GameObject commonPowerUpObject;

    [Header("Level UI")]
    public TMP_Text level;
    public TMP_Text xp;
    public TMP_Text xpMax;
    public Slider xpSlider;

    [Header("Player UI")]
    public TMP_Text PlayerHP;
    public TMP_Text PlayerStrenght;
    public TMP_Text PlayerArmor;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    public TMP_Text inventoryText;

    [Header("Change Item")]
    public GameObject changeItemPanel;

    // Flag to stop and resume the game
    [HideInInspector] public bool isPaused;
    #endregion

    #region MonoBehaviour
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
        Player.Instance.Initialize(Player.Instance.baseName, Player.Instance.baseHealth, 
                                    Player.Instance.baseStrenght, Player.Instance.baseArmor,
                                    Player.Instance.baseLevel, Player.Instance.baseXp,
                                    Player.Instance.baseMaxXp);
        Player.Instance.OpenItemPanel.AddListener(OpenPanel);

        UpdatePlayerStats();

        canvas = powerUps.transform.parent.gameObject;
        isPaused = false;

        // Enemies.Instance.StartWave();
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    ///  Call this method to add an upgrade to Player
    /// </summary>
    /// <param name="upgrade">Upgrade object</param>
    public void AddStats(Upgrade upgrade)
    {
        switch (upgrade.BuffPower.Buff)
        {
            case "Health":
                Player.Instance.Health += upgrade.BuffPower.Power;                
                break;
            case "Strength":
                Player.Instance.Strength += upgrade.BuffPower.Power;                
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

    /// <summary>
    /// Listener (??)
    /// </summary>
    public void OpenPanel()
    {
        Time.timeScale = 0f; // Pause the game
        changeItemPanel.SetActive(true);
    }

    /// <summary>
    /// Call this method to pause the game
    /// </summary>
    /// <param name="pause"></param>
    public void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
    #endregion

    #region UI Update Methods (Maybe change these to Listeners)
    /// <summary>
    /// Call this method everytime the UpgradesUi needs to Update
    /// </summary>
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

    /// <summary>
    /// Call this method to Update the Level and Xp on UI
    /// </summary>
    public void UpdateLevelXP()
    {
        level.text = "Lv: " + Player.Instance.Level.ToString();
        xp.text = (Player.Instance.Xp).ToString();
        xpMax.text = (Player.Instance.MaxXp).ToString();
        xpSlider.value = (float)Player.Instance.Xp;
        xpSlider.maxValue = (float)Player.Instance.MaxXp;
    }

    /// <summary>
    /// Call this method to Update the Player Stats
    /// </summary>
    private void UpdatePlayerStats()
    {
        PlayerHP.text = "HP: " + Player.Instance.Health.ToString();
        PlayerStrenght.text = "Strength: " + Player.Instance.Strength.ToString();
        PlayerArmor.text = "Armor: " + Player.Instance.Armor.ToString();
    }
    #endregion

    #region PowerUps
    /// <summary>
    /// Call this method to make the PowerUp canvas appear
    /// </summary>
    public void ShowPowerUpSelector()
    {
        // Get the gameObject of the Transform
        GameObject powerUpGameObject = powerUps.transform.gameObject;

        powerUpGameObject.SetActive(true);
        powerUps.transform.SetAsLastSibling();

        foreach (Upgrade powerUp in Upgrades.Instance.playerPowerUp)
        {
            GameObject newPowerUp = Instantiate(commonPowerUpObject, powerUps);

            Transform powerUpInfo = newPowerUp.transform.Find("PowerUp-Info");
            Image spriteComponent = powerUpInfo.GetComponentInChildren<Image>();

            if (spriteComponent != null) spriteComponent.sprite = powerUp.Icon;

            TMP_Text[] powerUpTexts = powerUpInfo.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text text in powerUpTexts)
            {
                if (text.name == "PowerUp-Name" && text != null) text.text = powerUp.UpgradeName;                 
                if (text.name == "PowerUp-Description" && text != null) text.text = powerUp.UpgradeDescription;                 
            }
        }
    }
    #endregion
}
