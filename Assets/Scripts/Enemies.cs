using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    public static Enemies Instance;

    private List<Enemy> enemies = new List<Enemy>();
    public int enemyCounter;
    public int waveCounter;

    private GameObject spawnPoints;
    public List<GameObject> SP = new List<GameObject>();

    // Create various prefabs for enemies
    [Header("Enemies Prefabs")]
    public Enemy goblin;
    public Enemy orc;
    public Enemy dragon;

    int playerLevel;

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
        enemyCounter = 0;
        waveCounter = 0;

        CreateWave();
    }

    /// <summary>
    /// Call this method to create all types of enemies
    /// </summary>
    private void CreateEnemies()
    {
        goblin.Initialize("Goblin", 20 + 4 * playerLevel, 20 + 2 * playerLevel, 1 * playerLevel, playerLevel + 1, 10, 0);
        orc.Initialize("Orc", 100 + 20 * playerLevel, 5 + 1 * playerLevel, 3 + 1 * playerLevel, playerLevel + 2, 30, 0);
        dragon.Initialize("Dragon", 20 + 4 * playerLevel, 20 + 2 * playerLevel, 1 * playerLevel, playerLevel + 5, 500, 0);
    }

    /// <summary>
    /// Call this method to populate the wave
    /// </summary>
    public void CreateWave()
    {
        CreateEnemies();
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
    /// <param name="enemy">Enemies to add</param>
    /// <param name="amount">Amount of enemies to add</param>
    public void AddToWave(Enemy enemy, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Call this method to view all enemies in the list
    /// </summary>
    public void ViewAllWave()
    {
        //Debug.Log($"Wave has {enemies.Count} enemies.");
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
                Debug.LogWarning($"Null enemy in list");
            //else
            //    Debug.Log($"{character.Name}");
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
    private void SpawnEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            //Debug.Log("Processing: " + character.Name);

            // Just in case we have too many enemies for the amout of SpawnPoints available
            if (enemyCounter >= SP.Count)
            {
                //Debug.LogWarning("No spawn point for enemy #" + enemieCounter);
                break;
            }

            // Encapsulation of position and rotation
            Vector3 spawnPos = SP[enemyCounter].transform.position;
            Quaternion spawnRot = SP[enemyCounter].transform.rotation;

            if (enemy.Name.StartsWith("Goblin"))
            {
                Enemy newGoblin = Instantiate(goblin, spawnPos, spawnRot);

                NavMeshAgent newGoblinAgent = newGoblin.GetComponent<NavMeshAgent>();

                if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    newGoblinAgent.Warp(hit.position);
                    newGoblinAgent.enabled = true;
                }
            }
            else if (enemy.Name.StartsWith("Orc"))
            {
                Enemy newOrc = Instantiate(orc, spawnPos, spawnRot);

                NavMeshAgent newOrcAgent = newOrc.GetComponent<NavMeshAgent>();

                if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    newOrcAgent.Warp(hit.position);
                    newOrcAgent.enabled = true;
                }

            }
            else if (enemy.Name.StartsWith("Dragon"))
            {
                Enemy newDragon = Instantiate(dragon, spawnPos, spawnRot);

                NavMeshAgent newDragonAgent = newDragon.GetComponent<NavMeshAgent>();

                if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    newDragonAgent.Warp(hit.position);
                    newDragonAgent.enabled = true;
                }
            }

            enemyCounter++;
        }
    }
}