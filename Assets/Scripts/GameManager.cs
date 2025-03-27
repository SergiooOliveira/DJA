using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("Icons UI")]
    public Transform icons;
    public GameObject upgradeObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateUpgradesUI()
    {
        // Delete all previous icons
        foreach(Transform i in icons)
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
}
