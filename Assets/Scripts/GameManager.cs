using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Upgrades upgrades = new Upgrades();
        upgrades.SetAllUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
