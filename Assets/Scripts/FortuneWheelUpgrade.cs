using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheelUpgrade : MonoBehaviour
{
    [HideInInspector] public Upgrade powerUp;
    [SerializeField] private int upgradeindex = 0;

    private void Start()
    {
        if (Upgrades.Instance == null)
        {
            Debug.LogError("Upgrades.Instance is null!");
            return;
        }
        if (Upgrades.Instance.upgradeList == null)
        {
            Debug.LogError("Upgrades.Instance.upgradeList is null!");
            return;
        }
        if (upgradeindex < 0 || upgradeindex >= Upgrades.Instance.upgradeList.Count)
        {
            Debug.LogError($"upgradeindex {upgradeindex} is out of bounds!");
            return;
        }
        powerUp = Upgrades.Instance.upgradeList[upgradeindex];
        Debug.Log($"PowerUp: {powerUp.UpgradeName}" +
            $" - {powerUp.UpgradeDescription}" +
            $" - {powerUp.UpgradeType}" +
            $" - {powerUp.UpgradeCost}" +
            $" - {powerUp.Icon.name}" +
            $" - {powerUp.Weight}" +
            $" - Buff: {powerUp.BuffPower.Buff}," +
            $" Power: {powerUp.BuffPower.Power}");
    }
}
