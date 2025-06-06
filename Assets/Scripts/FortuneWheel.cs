using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField] private List<FortuneWheelUpgrade> fortuneWheelUpgradeList;

    [Header("Fortune Wheel Settings")]

    private float fortuneWheeMinSpeed = 7500f;
    private float fortuneWheelMaxSpeed = 20000f;
    private float fortuneWheelDeceleration = 0.002f;

    private bool fortuneWheelStopSpinning = false;
    private bool fortuneWheelSpinned = false;

    private float fortuneWheelCurrentSpeed = 0f;
    private float fortuneWheelCurrentDeceleration = 0f;


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
            GetHighestUpgradePosition();
            Destroy(this); // Destroy the FortuneWheel script after spinning
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
        GameManager.Instance.AddStats(highestUpgrade.GetComponent<FortuneWheelUpgrade>().powerUp);
    }
}
