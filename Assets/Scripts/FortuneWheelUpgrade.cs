using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FortuneWheelUpgrade : MonoBehaviour
{
    [HideInInspector] public Upgrade powerUp;
    [SerializeField] private string upgradeName = "";

    private void Start()
    {
        powerUp = Upgrades.Instance.upgradeList
            .FirstOrDefault(u => u.UpgradeName == upgradeName);

        // Fix: Ensure this line is inside a method (e.g., Start) and properly formatted
        var imageComponent = gameObject.GetComponent<Image>();
        imageComponent.sprite = powerUp.Icon;
    }
}
