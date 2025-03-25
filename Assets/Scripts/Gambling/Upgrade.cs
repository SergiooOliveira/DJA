using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    public Upgrade(string upgradeName, string upgradeDescription, int upgradeCost, Sprite icon, float weight)
    {
        this.UpgradeName = upgradeName;
        this.UpgradeDescription = upgradeDescription;
        this.UpgradeCost = upgradeCost;
        this.Icon = icon;
        this.Weight = weight;
    }

    public string UpgradeName { get; set; }
    public string UpgradeDescription { get; set; }
    public int UpgradeCost { get; set; }
    public Sprite Icon { get; set; }
    public float Weight { get; set; }
}
