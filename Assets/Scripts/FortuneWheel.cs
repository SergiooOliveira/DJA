using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField] private List<FortuneWheelUpgrade> fortuneWheelUpgradeList;

    [Header("Fortune Wheel Settings")]

    private float fortuneWheeMinSpeed = 7500f;
    private float fortuneWheelMaxSpeed = 20000f;
    private float fortuneWheelDeceleration = 0.001f;

    private bool fortuneWheelStopSpinning = false;
    private bool fortuneWheelSpinned = false;

    private float fortuneWheelCurrentSpeed = 0f;
    private float fortuneWheelCurrentDeceleration = 0f;

    private void Start()
    {
        foreach (FortuneWheelUpgrade go in fortuneWheelUpgradeList)
        {
            go.GetComponent<Image>().sprite = go.powerUp.Icon;
        }
    }

    public void SpinWheel()
    {
        if (!fortuneWheelStopSpinning)
        {
            fortuneWheelSpinned = true;
            fortuneWheelStopSpinning = true;

            // Start spinning the wheel
            fortuneWheelCurrentDeceleration = fortuneWheelDeceleration;
            fortuneWheelCurrentSpeed = Random.Range(fortuneWheeMinSpeed, fortuneWheelMaxSpeed);
        }
    }

    private void Update()
    {
        if (fortuneWheelCurrentDeceleration > 0)
        {
            // Apply deceleration
            fortuneWheelCurrentSpeed -= fortuneWheelCurrentSpeed * fortuneWheelCurrentDeceleration;
            Debug.Log("Current Speed: " + fortuneWheelCurrentSpeed + " - Deceleration: " + fortuneWheelCurrentDeceleration);
            // Rotate the wheel
            transform.Rotate(0, fortuneWheelCurrentSpeed * Time.deltaTime, 0);
            // Stop the wheel if speed is low enough
            if (fortuneWheelCurrentSpeed < 2f)
            {
                fortuneWheelCurrentSpeed = 0f;
                fortuneWheelCurrentDeceleration = 0f;
            }
        }
        else if(fortuneWheelSpinned)
        {
            fortuneWheelStopSpinning = false;
            fortuneWheelSpinned = false;
            GetHighestUpgradePosition();
        }
    }

    // This method serves to get the highest Upgrade position from the list
    void GetHighestUpgradePosition()
    {
        if (fortuneWheelUpgradeList.Count == 0) return;
        FortuneWheelUpgrade highestUpgrade = fortuneWheelUpgradeList[0];
        foreach (FortuneWheelUpgrade go in fortuneWheelUpgradeList)
        {
            if (go.transform.position.y > highestUpgrade.transform.position.y)
            {
                highestUpgrade = go;
            }
        }
        Upgrades.Instance.playerPowerUp.Add(highestUpgrade.GetComponent<FortuneWheelUpgrade>().powerUp);
        Debug.Log("Added PowerUp: "
            + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].UpgradeName
            + " - " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].UpgradeDescription
            + " - " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].UpgradeType
            + " - " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].UpgradeCost
            + " - " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].Icon.name
            + " - " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].Weight
            + " - Buff: " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].BuffPower.Buff
            + " Power: " + Upgrades.Instance.playerPowerUp[Upgrades.Instance.playerPowerUp.Count - 1].BuffPower.Power
            + " to Player's PowerUps as PowerUp Number " + Upgrades.Instance.playerPowerUp.Count);
    }
}
