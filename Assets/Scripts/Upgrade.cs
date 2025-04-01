using UnityEngine;

public class Upgrade : Upgrades
{
    /// <summary>
    /// Constructor to initialize a 
    /// </summary>
    /// <param name="upgradeName"></param>
    /// <param name="upgradeDescription"></param>
    /// <param name="upgradeCost"></param>
    public Upgrade(string upgradeName, string upgradeDescription, int upgradeCost)
    {
        this.UpgradeName = upgradeName;
        this.UpgradeDescription = upgradeDescription;
        this.UpgradeCost = upgradeCost;
    }

    public string UpgradeName { get; set; }

    public string UpgradeDescription { get; set; }

    public int UpgradeCost { get; set; }
}
