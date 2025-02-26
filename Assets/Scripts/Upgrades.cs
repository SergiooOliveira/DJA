using System;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades
{
    // Variables
    public List<Upgrade> upgradeList = new List<Upgrade>();

    public Upgrades ()
    {

    }

    /// <summary>
    /// Call this function at the start to set all upgrades
    /// </summary>
    public void SetAllUpgrades()
    {
        CreateUpgrade();
        // Code to set all upgrades in a list
        foreach (Upgrade u in upgradeList)
        {
            //Debug.Log(u.ToString(u));
        }
    }

    public void CreateUpgrade()
    {
        Upgrade upgrade1 = new Upgrade("Upgrade1", "Upgrade1-Description", 1);
        Upgrade upgrade2 = new Upgrade("Upgrade2", "Upgrade2-Description", 2);
        Upgrade upgrade3 = new Upgrade("Upgrade3", "Upgrade3-Description", 3);
        Upgrade upgrade4 = new Upgrade("Upgrade4", "Upgrade4-Description", 4);
        Upgrade upgrade5 = new Upgrade("Upgrade5", "Upgrade5-Description", 5);
        Upgrade upgrade6 = new Upgrade("Upgrade6", "Upgrade6-Description", 6);
        Upgrade upgrade7 = new Upgrade("Upgrade7", "Upgrade7-Description", 7);
        Upgrade upgrade8 = new Upgrade("Upgrade8", "Upgrade8-Description", 8);
        Upgrade upgrade9 = new Upgrade("Upgrade9", "Upgrade9-Description", 9);
        Upgrade upgrade10 = new Upgrade("Upgrade10", "Upgrade10-Description", 10);

        upgradeList.Add(upgrade1);
        upgradeList.Add(upgrade2);
        upgradeList.Add(upgrade3);
        upgradeList.Add(upgrade4);
        upgradeList.Add(upgrade5);
        upgradeList.Add(upgrade6);
        upgradeList.Add(upgrade7);
        upgradeList.Add(upgrade8);
        upgradeList.Add(upgrade9);
        upgradeList.Add(upgrade10);
    }

    public string ToString(Upgrade u)
    {
        return $"Name: {u.UpgradeName}\tDescription: {u.UpgradeDescription}\tCost: {u.UpgradeCost}\n";
    }
}
