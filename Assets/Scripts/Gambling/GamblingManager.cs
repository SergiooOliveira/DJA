using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class GamblingManager : MonoBehaviour
{
    public static GamblingManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (Upgrades.Instance == null)
        {
            Debug.LogError("Upgrades Manager is missing");
            return;
        }
    }

    public void StartRolling()
    {
        Upgrade randomUpgrade = Upgrades.Instance.GetRandomUpgrade();
        Upgrades.Instance.ToString(randomUpgrade);
    }
}
