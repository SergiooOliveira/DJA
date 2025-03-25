using UnityEngine;

public class Upgrade : Upgrades
{

    public Upgrade(string upgradeName, string upgradeDescription, int upgradeCost, Sprite icon, float weight)
    {
        this.UpgradeName = upgradeName;
        this.UpgradeDescription = upgradeDescription;
        this.UpgradeCost = upgradeCost;     
        this.Icon = icon;
        this.weight = weight;
    }

    public string UpgradeName { get; set; }

    public string UpgradeDescription { get; set; }

    public int UpgradeCost { get; set; }

    public Sprite Icon { get; set; }

    public float weight { get; set; }
}
