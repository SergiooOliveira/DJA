using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Upgrade
{
    public Upgrade(string upgradeName, string upgradeDescription, string upgradeType, int upgradeCost, string iconPath, float weight)
    {
        this.UpgradeName = upgradeName;
        this.UpgradeDescription = upgradeDescription;
        this.UpgradeType = upgradeType;
        this.UpgradeCost = upgradeCost;
        this.Icon = LoadSprite(iconPath);
        this.Weight = weight;

    }

    public string UpgradeName { get; set; }
    public string UpgradeDescription { get; set; }
    public string UpgradeType { get; set; }
    public int UpgradeCost { get; set; }
    public Sprite Icon { get; set; }
    public float Weight { get; set; }

    #region Methods
    private Sprite LoadSprite(string path)
    {
        Sprite loadedSprite = Resources.Load<Sprite>(path);

        //Debug.LogWarning($"Path: {path}");
        if (loadedSprite == null) Debug.LogError($"Error loading {path}");        

        return loadedSprite;
    }
    #endregion
}
