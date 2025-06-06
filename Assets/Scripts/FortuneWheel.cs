using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    #region Variables
    private List<Upgrade> fortuneWheelUpgradeList;
    public GameObject powerUpsCanvas;
    [SerializeField] private Transform topPointer;

    [Header("Fortune Wheel Settings")]
    private float fortuneWheeMinSpeed = 500f;
    private float fortuneWheelMaxSpeed = 1500f;
    private float fortuneWheelDeceleration = 0.005f;

    private bool fortuneWheelStopSpinning = false;
    private bool fortuneWheelSpinned = false;

    private float fortuneWheelCurrentSpeed = 0f;
    private float fortuneWheelCurrentDeceleration = 0f;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        fortuneWheelUpgradeList = new List<Upgrade>();
        PopulateWheel();
    }

    private void FixedUpdate()
    {
        if (fortuneWheelCurrentDeceleration > 0)
        {
            // Apply deceleration
            fortuneWheelCurrentSpeed -= fortuneWheelCurrentSpeed * fortuneWheelCurrentDeceleration;
            // Rotate the wheel
            transform.Rotate(0, fortuneWheelCurrentSpeed * Time.deltaTime, 0);
            // Stop the wheel if speed is low enough
            if (fortuneWheelCurrentSpeed < 5f)
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
    #endregion

    #region Methods
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

    // This method serves to get the highest Upgrade position from the list
    void GetHighestUpgradePosition()
    {
        if (powerUpsCanvas.transform.childCount == 0)
        {
            Debug.LogWarning("No wheel segments found!");
            return;
        }

        Vector3 canvasCenter = powerUpsCanvas.transform.position;
        Vector3 topDirection = topPointer.forward; // or .up if that's the correct direction

        Transform topSegment = null;
        float maxDot = float.MinValue;

        foreach (Transform segment in powerUpsCanvas.transform)
        {
            Vector3 dirToSegment = (segment.position - canvasCenter).normalized;
            float dot = Vector3.Dot(topDirection.normalized, dirToSegment);

            if (dot > maxDot)
            {
                maxDot = dot;
                topSegment = segment;
            }
        }

        if (topSegment != null)
        {
            var upgrade = topSegment.GetComponent<FortuneWheelUpgrade>();
            if (upgrade != null && upgrade.powerUp != null)
            {
                Debug.Log($"Winner: {upgrade.powerUp.UpgradeName} (Dot: {maxDot})");
                StartCoroutine(HighlightWinner(topSegment));
            }
            else Debug.LogError("Missing upgrade component!");
        }
    }

    IEnumerator HighlightWinner(Transform winner)
    {
        Image img = winner.GetComponent<Image>();
        if (img != null)
        {
            Color original = img.color;
            img.color = Color.green;
            yield return new WaitForSeconds(2f);
            img.color = original;
        }
    }

    private void PopulateWheel()
    {
        Color color = Color.black;

        for (int i = 0; i < 8; i++)
        {
            Upgrade randomUpgrade = Upgrades.Instance.GetRandomUpgrade();
            fortuneWheelUpgradeList.Add(randomUpgrade);

            Transform powerUpSlot = powerUpsCanvas.transform.GetChild(i);
            FortuneWheelUpgrade ftu = powerUpSlot.GetComponent<FortuneWheelUpgrade>();

            if (ftu != null)
            {
                ftu.powerUp = randomUpgrade;
                
                if (ftu.powerUp.UpgradeName.Contains("Downgrade"))
                {
                    // It's a Downgrade
                    color = Color.red;
                }
                else if (ftu.powerUp.UpgradeName.Contains("Common"))
                {
                    color = Color.white;
                }
                else if (ftu.powerUp.UpgradeName.Contains("Rare"))
                {
                    color = Color.blue;
                }
                else if (ftu.powerUp.UpgradeName.Contains("Epic"))
                {
                    color = Color.magenta;
                }
                else if (ftu.powerUp.UpgradeName.Contains("Legendary"))
                {
                    color = Color.yellow;
                }
                else if (ftu.powerUp.UpgradeName.Contains("Mythic"))
                {
                    color = Color.cyan;
                }

                ftu.GetComponent<Image>().sprite = randomUpgrade.Icon;
                ftu.GetComponent<Image>().color = color;
            }
        }                         
    }
    #endregion
}
