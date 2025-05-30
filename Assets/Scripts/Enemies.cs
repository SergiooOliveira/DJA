using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public static Enemies Instance;

    private List<Character> enemies = new List<Character>();
    private GameObject spawnPoints;
    public List<GameObject> SP = new List<GameObject>();

    [Header("Enemies Prefabs")]
    // Create various prefabs for enemies
    public Character goblin;
    public Character orc;
    public Character dragon;

    int playerLevel;
    int waveCounter = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Call this method to start a wave
    /// </summary>
    public void StartWave(Transform spawnPoints)
    {
        if (SP.Count > 0) SP.Clear();

        foreach (Transform t in spawnPoints.transform)
        {
            SP.Add(t.gameObject);
        }

        playerLevel = Player.Instance.Level;
        print(playerLevel);
        CreateWave();
    }

    /// <summary>
    /// Call this method to populate the wave
    /// </summary>
    private void CreateWave()
    {
        ClearWave();
        switch (waveCounter)
        {
            case 0:
                AddToWave(goblin, 3);
                break;
            case 1:
                AddToWave(goblin, 3);
                AddToWave(orc, 2);
                break;
            case 2:
                AddToWave(dragon, 1);
                break;
        }
        SpawnEnemies();
    }

    /// <summary>
    /// Call this method to add enemies to the list
    /// </summary>
    /// <param name="character">Enemies to add</param>
    /// <param name="amount">Amount of enemies to add</param>
    public void AddToWave(Character character, int amount)
    {
        for (int i = 0; i < amount; i++)
        {            
            enemies.Add(character);
        }

        Debug.Log($"Added {amount} x {character.Name} to the wave");
    }

    /// <summary>
    /// Call this method to view all enemies in the list
    /// </summary>
    public void ViewAllWave()
    {
        Debug.Log($"Wave has {enemies.Count} enemies.");
        foreach (Character character in enemies)
        {
            if (character == null)
                Debug.LogWarning($"Null enemy in list");
            else

                Debug.Log($"{character.Name}");
        }
    }

    /// <summary>
    /// Call this method to clear Enemies List
    /// </summary>
    private void ClearWave()
    {
        enemies.Clear();
    }

    /// <summary>
    /// Call this method to spawn the enemies
    /// </summary>
    private void SpawnEnemies ()
    {
        // Enemie counter to control the position
        int enemieCounter = 0;

        Debug.Log("Total enemies: " + enemies.Count);

        foreach (Character character in enemies)
        {
            Debug.Log("Processing: " + character.Name);

            // Just in case we have too many enemies for the amout of SpawnPoints available
            if (enemieCounter >= SP.Count)
            {
                Debug.LogWarning("No spawn point for enemy #" + enemieCounter);
                break;
            }

            // Encapsulation of position and rotation
            Vector3 spawnPos = SP[enemieCounter].transform.position;
            Quaternion spawnRot = SP[enemieCounter].transform.rotation;

            if (character.Name.StartsWith("Goblin"))
            {
                Character newGoblin = Instantiate(goblin, spawnPos, spawnRot);
                newGoblin.Initialize("Goblin", 20 * playerLevel, 20 * playerLevel, 5 * playerLevel, playerLevel + 1, 10, 0);
            }
            else if (character.Name.StartsWith("Orc"))
            {
                Character newOrc = Instantiate(orc, spawnPos, spawnRot);
                newOrc.Initialize("Orc", 100 * playerLevel, 5 * playerLevel, 15 * playerLevel, playerLevel + 2, 30, 0);
            }
            else if (character.Name.StartsWith("Dragon"))
            {
                Character newDragon = Instantiate(dragon, spawnPos, spawnRot);
                newDragon.Initialize("Dragon", 20 * playerLevel, 20 * playerLevel, 10 * playerLevel, playerLevel + 5, 500, 0);
            }
            
            Debug.Log($"{enemieCounter}: {character.Name} at {spawnPos} with {character.Health} HP");
            enemieCounter++;
        }
    }
}
