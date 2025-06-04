using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance;

    // Variables
    /// <summary>
    /// upgradeList is used to store all the upgrades that the player can have
    /// </summary>
    [HideInInspector] public List<Upgrade> upgradeList;

    /// <summary>
    /// playerUpgrades is used to storeall the current Upgrades the player has
    /// </summary>
    [HideInInspector] public List<Upgrade> playerUpgrades;

    /// <summary>
    /// powerUpList is used to store all the powerUps that the player can have
    /// </summary>
    [HideInInspector] public List<Upgrade> powerUpList;

    /// <summary>
    /// playerPowerUp is used to storeall the current powerUps the player has
    /// </summary>
    [HideInInspector] public List<Upgrade> playerPowerUp;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        upgradeList = new List<Upgrade>();
        CreateUpgrades();
        CreatePowerUps();
    }


    #region Upgrade Methods
    /// <summary>
    /// Use this method to create static Upgrades
    /// </summary>
    public void CreateUpgrades()
    {
        upgradeList = new List<Upgrade>
        {
            //              Name                            Description              Type         Cost     Sprite      Weight  Tuple(Buff, BuffPower)

            // ----- Health Buffs -----
            new Upgrade("Health Common Upgrade",       "Common description",       "Common",       10, "Sprites/HpBuff", 100,  ("Health", 10)),
            new Upgrade("Health Rare Upgrade",         "Rare description",         "Rare",         10, "Sprites/HpBuff", 20,   ("Health", 20)),
            new Upgrade("Health Epic Upgrade",         "Epic description",         "Epic",         10, "Sprites/HpBuff", 10,   ("Health", 30)),
            new Upgrade("Health Legendary Upgrade",    "Legendary description",    "Legendary",    10, "Sprites/HpBuff", 7,    ("Health", 40)),
            new Upgrade("Health Mythic Upgrade",       "Mythic description",       "Mythic",       10, "Sprites/HpBuff", 3,    ("Health", 50)),

            // ----- Strength Buffs -----
            new Upgrade("Strength Common Upgrade",       "Common description",       "Common",       10, "Sprites/AttackBuff", 100,  ("Strength", 10)),
            new Upgrade("Strength Rare Upgrade",         "Rare description",         "Rare",         10, "Sprites/AttackBuff", 20,   ("Strength", 20)),
            new Upgrade("Strength Epic Upgrade",         "Epic description",         "Epic",         10, "Sprites/AttackBuff", 10,   ("Strength", 30)),
            new Upgrade("Strength Legendary Upgrade",    "Legendary description",    "Legendary",    10, "Sprites/AttackBuff", 7,    ("Strength", 40)),
            new Upgrade("Strength Mythic Upgrade",       "Mythic description",       "Mythic",       10, "Sprites/AttackBuff", 3,    ("Strength", 50)),

            // ----- Armor Buffs -----
            new Upgrade("Armor Common Upgrade",       "Common description",       "Common",       10, "Sprites/Cyan", 100,  ("Armor", 10)),
            new Upgrade("Armor Rare Upgrade",         "Rare description",         "Rare",         10, "Sprites/Cyan", 20,   ("Armor", 20)),
            new Upgrade("Armor Epic Upgrade",         "Epic description",         "Epic",         10, "Sprites/Cyan", 10,   ("Armor", 30)),
            new Upgrade("Armor Legendary Upgrade",    "Legendary description",    "Legendary",    10, "Sprites/Cyan", 7,    ("Armor", 40)),
            new Upgrade("Armor Mythic Upgrade",       "Mythic description",       "Mythic",       10, "Sprites/Cyan", 3,    ("Armor", 50)),
        };
    }

    /// <summary>
    /// Call this method to get a random Upgrade
    /// </summary>
    public Upgrade GetRandomUpgrade()
    {
        float totalWeight = 0f;

        foreach (Upgrade upgrade in upgradeList)
        {
            //Debug.Log($"<color=yellow>{totalWeight}</color> += <color=lime>{upgrade.Weight}</color>");
            totalWeight += upgrade.Weight;
        }

        float randomValue = Random.Range(0, totalWeight);

        //Debug.Log($"<color=yellow>{ randomValue }</color>");

        float cumulativeWeight = 0f;

        foreach (Upgrade upgrade in upgradeList)
        {
            cumulativeWeight += upgrade.Weight;
            if (randomValue < cumulativeWeight)
                return upgrade;
        }

        return upgradeList[0]; // Fallback
    }
    #endregion

    #region PowerUps Methods
    /// <summary>
    /// Call this method to create all PowerUps
    /// </summary>
    public void CreatePowerUps()
    {
        powerUpList = new List<Upgrade>
        {
            // Name - Description - Type - Cost - Sprite - Weight - Tuple(Buff, BuffPower)
            new Upgrade("Angry Pepe", "When angry Pepe get's mad he unveils his pistol and massacrates like a little kid in USA", "Common", 0, "PowerUps/Example", 0, (null, 0))
        };
    }

    /// <summary>
    /// Call this method to retrive a random PowerUp
    /// </summary>
    /// <returns></returns>
    public Upgrade GetRandomPowerUp()
    {
        foreach (Upgrade upgrade in powerUpList)
        {
            return upgrade;
        }        

        return powerUpList[0];
    }
    #endregion

    #region Overrides
    /// <summary>
    /// Call this method get the Name, Description and Cost of a Upgrade
    /// </summary>
    /// <param name="u">Upgrade to check</param>
    public void ToString(Upgrade u)
    {
        Debug.Log($"<color=red>Name: </color><color=yellow>{u.UpgradeName}</color>\t" +
                     $"<color=red>Description: </color><color=yellow>{u.UpgradeDescription}</color>\t" +
                     $"<color=red>Cost: </color><color=yellow>{u.UpgradeCost}</color>\n" +
                     $"<color=red>Buff: </color><color=yellow>{u.BuffPower.Buff}</color>\t" +
                     $"<color=red>Power: </color><color=yellow>{u.BuffPower.Power}</color>");
    }
    #endregion
}
