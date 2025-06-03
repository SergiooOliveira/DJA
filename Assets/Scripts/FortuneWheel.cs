using System.Collections.Generic;
using UnityEngine;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectList;

    [Header("Fortune Wheel Settings")]

    private float fortuneWheelSpeed = 10000f;
    private float fortuneWheelDeceleration = 0.99f;

    private bool fortuneWheelStopSpinning = false;

    float fortuneWheelCurrentSpeed = 0f;
    float fortuneWheelCurrentDeceleration = 0f;

    public void SpinWheel()
    {
        if (!fortuneWheelStopSpinning)
        {
            fortuneWheelStopSpinning = true;

            // Start spinning the wheel
            fortuneWheelCurrentDeceleration = fortuneWheelDeceleration;
            fortuneWheelCurrentSpeed = Random.Range(fortuneWheelSpeed * 0.75f, fortuneWheelSpeed);
        }
    }

    private void Update()
    {
        if (fortuneWheelCurrentDeceleration > 0)
        {
            // Apply deceleration
            fortuneWheelCurrentSpeed *= fortuneWheelCurrentDeceleration;
            // Rotate the wheel
            transform.Rotate(0, fortuneWheelCurrentSpeed * Time.deltaTime, 0);
            // Stop the wheel if speed is low enough
            if (fortuneWheelCurrentSpeed < 0.1f)
            {
                fortuneWheelCurrentSpeed = 0f;
                fortuneWheelCurrentDeceleration = 0f;
            }
        }
        else
        {
            fortuneWheelStopSpinning = false;
            GetHighestUpgradePosition();
        }
    }

    // This method serves to get the highest Upgrade position from the list
    void GetHighestUpgradePosition()
    {
        if (gameObjectList.Count == 0) return;
        GameObject highestUpgrade = gameObjectList[0];
        foreach (GameObject go in gameObjectList)
        {
            if (go.transform.position.y > highestUpgrade.transform.position.y)
            {
                highestUpgrade = go;
            }
        }
        Debug.Log("Highest Upgrade Position: " + highestUpgrade.name);
    }
}
