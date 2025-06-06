using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance; // Singleton

    private GameObject canvas; // Main Canvas object
    public GameObject gameOverCanvas;
    public TMP_Text skillPointsText;

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
        isPaused = true;
        TogglePause();

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
    public void AddUpgrade(Upgrade upgrade)
    {
        Upgrades.Instance.playerUpgrades.Add(upgrade);

        switch (upgrade.BuffPower.Buff)
        {
            case "Health":
                Player.Instance.Health += upgrade.BuffPower.Power;
                Debug.Log($"Adding {upgrade.BuffPower.Power} to Health");
                break;
            case "Strength":
                Player.Instance.Strength += upgrade.BuffPower.Power;
                Debug.Log($"Adding {upgrade.BuffPower.Power} to Strength");
                break;
            case "Armor":
                Player.Instance.Armor += upgrade.BuffPower.Power;
                Debug.Log($"Adding {upgrade.BuffPower.Power} to Armor");
                break;
            default:
                Debug.LogError("Error applying buff");
                break;
        }

        UpdatePlayerStats();
        UpdateUpgradesUI();
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
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }

    /// <summary>
    /// Call this method to make the gameOver canvas appear
    /// </summary>
    public void GameOver()
    {
        gameOverCanvas.SetActive(true);

        Animator animator = gameOverCanvas.GetComponent<Animator>();
        animator.SetBool("playerDied", true);

        animator.Play("GameOver");

        animator.SetBool("animationPlayed", true);
    }

    /// <summary>
    /// Call this method to retry the run again
    /// </summary>
    public void Retry()
    {
        // Restart the run
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    /// <summary>
    /// Call this method to go back to main menu
    /// </summary>
    public void GoBack()
    {
        // Call main menu
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
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

            Image spriteComponent = newIcon.GetComponentInChildren<Image>();
            if (spriteComponent != null) spriteComponent.sprite = upgrade.Icon;

            if (upgrade.UpgradeName.Contains("Downgrade")) spriteComponent.color = Color.red;
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
    public void UpdatePlayerStats()
    {
        PlayerHP.text = Player.Instance.Health.ToString();
        PlayerStrenght.text = Player.Instance.Strength.ToString();
        PlayerArmor.text = Player.Instance.Armor.ToString();
    }

    /// <summary>
    /// Call this method to update the remaining skill points in the skill tree menu
    /// </summary>
    public void UpdateSkillPoints()
    {
        skillPointsText.text = "Skill Points: " + Player.Instance.skillPoints.ToString();
    }
    #endregion
}
