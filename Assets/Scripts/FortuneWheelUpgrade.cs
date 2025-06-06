using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FortuneWheelUpgrade : MonoBehaviour
{
    public Upgrade powerUp;

    private void Start()
    {
        //powerUp = Upgrades.Instance.upgradeList.FirstOrDefault(u => u.UpgradeName == upgradeName);

        //var imageComponent = gameObject.GetComponent<Image>();
        //imageComponent.sprite = powerUp.Icon;
    }
}
