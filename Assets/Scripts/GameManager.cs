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
    }

    public void UpdateLevelXP()
    {
        level.text = Player.Instance.Level.ToString();
        xpMax.text = (Player.Instance.Xp + "/" + Player.Instance.MaxXp);
    }
}
