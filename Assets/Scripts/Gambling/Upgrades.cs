using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance;

    // Variables
    public List<Upgrade> upgradeList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        upgradeList = new List<Upgrade>();
        CreateUpgrades();
    }

    /// <summary>
    /// Get a random Upgrade
    /// </summary>
    public Upgrade GetRandomUpgrade()
    {
        float totalWeight = 0f;

        foreach (Upgrade upgrade in upgradeList)
        {
            //Debug.Log($"<color=yellow>{totalWeight}</color> += <color=lime>{upgrade.Weight}</color>");
            totalWeight += upgrade.Weight;
        }

        float randomValue = Random.Range(0, totalWeight);

        //Debug.Log($"<color=yellow>{ randomValue }</color>");

        float cumulativeWeight = 0f;

        foreach (Upgrade upgrade in upgradeList)
        {
            cumulativeWeight += upgrade.Weight;
            if (randomValue < cumulativeWeight)
                return upgrade;
        }

        return upgradeList[0]; // Fallback
    }

    public void CreateUpgrades()
    {
        upgradeList = new List<Upgrade>
        {
            new Upgrade("Common", "Common description", 10, null, 100),
            new Upgrade("Rare", "Rare description", 10, null, 20),
            new Upgrade("Epic", "Epic description", 10, null, 10),
            new Upgrade("Legendary", "Legendary description", 10, null, 7),
            new Upgrade("Mythic", "Mythic description", 10, null, 3)
        };
    }

    public string ToString(Upgrade u)
    {
        return $"Name: {u.UpgradeName} Description: {u.UpgradeDescription} Cost: {u.UpgradeCost}\n";
    }
}
