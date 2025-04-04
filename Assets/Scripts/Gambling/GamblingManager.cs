using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GamblingManager : MonoBehaviour
{
    public static GamblingManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (Upgrades.Instance == null)
        {
            Debug.LogError("Upgrades Manager is missing");
            return;
        }
    }

    /// <summary>
    ///  Call this method to Get a random Upgrade and apply it to Player
    /// </summary>
    public void StartRolling()
    {
        // Get a random Upgrade
        Upgrade randomUpgrade = Upgrades.Instance.GetRandomUpgrade();

        // Add upgrade on playerUpgrades List (Only for visual purposes right now)
        Upgrades.Instance.playerUpgrades.Add(randomUpgrade);

        // Add stats to the player
        GameManager.Instance.AddStats(randomUpgrade);

        // Show Player Upgrades UI
        GameManager.Instance.UpdateUpgradesUI();        
    }
}
