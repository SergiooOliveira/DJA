using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance;

    // Variables
    public List<Upgrade> upgradeList;
    public List<Upgrade> playerUpgrades;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        upgradeList = new List<Upgrade>();
        CreateUpgrades();
        ConcedeUpgrades();
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
            // Name - Description - Cost - Sprite - Weight
            new Upgrade("Common Upgrade", "Common description", "Common", 10, "Sprites/Cyan", 100),
            new Upgrade("Rare Upgrade", "Rare description", "Rare", 10, "Sprites/Blue", 20),
            new Upgrade("Epic Upgrade", "Epic description", "Epic", 10, "Sprites/Pink", 10),
            new Upgrade("Legendary Upgrade", "Legendary description", "Legendary", 10, "Sprites/Yellow", 7),
            new Upgrade("Mythic Upgrade", "Mythic description", "Mythic", 10, "Sprites/Red", 3)
        };
    }

    public void ConcedeUpgrades()
    {
        playerUpgrades.Add(upgradeList[2]);
    }

    #region Overrides
    /// <summary>
    /// Call this method get the Name, Description and Cost of a Upgrade
    /// </summary>
    /// <param name="u">Upgrade to check</param>
    public void ToString(Upgrade u)
    {
        Debug.Log($"<color=red>Name: </color><color=yellow>{u.UpgradeName}</color>\t" +
                     $"<color=red>Description: </color><color=yellow>{u.UpgradeDescription}</color>\t" +
                     $"<color=red>Cost: </color><color=yellow>{u.UpgradeCost}</color>");
    }
    #endregion
}
