using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{
    public static GamblingManager Instance;

    Upgrades upgrades = new Upgrades();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get all possible gamble arrays
        // Get upgrades list        
        upgrades.SetAllUpgrades();
    }

    /// <summary>
    /// Call this method to roll through a list of Upgrades
    /// </summary>
    public void SpinRoll()
    {
        int randomChoice = Random.Range(0, upgrades.upgradeList.Count);

        Debug.Log($"Chosen Upgrade:\nName: {upgrades.upgradeList[randomChoice].UpgradeName.ToString() }");
    }
}
