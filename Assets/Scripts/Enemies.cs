using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public static Enemies Instance;

    public List<Character> enemies = new List<Character>();

    [Header("Enemies Prefabs")]
    // Create various prefabs for enemies
    [SerializeField] private Character goblinPrefab;
    [SerializeField] private Character orcPrefab;
    [SerializeField] private Character dragonPrefab;

    Character goblin;
    Character orc;   
    Character dragon;

    int playerLevel;
    //int waveCounter = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {   
                
    }

    /// <summary>
    /// Call this method to start a wave
    /// </summary>
    public void StartWave()
    {
        playerLevel = Player.Instance.Level;
        CreateEnemies();
        CreateWave();
    }

    /// <summary>
    /// Call this method to populate the wave
    /// </summary>
    private void CreateWave()
    {
        // Wave 1
        AddToWave(goblin, 3);

        //ViewAllWave();
        //ClearWave();

        // Wave 2        
        AddToWave(goblin, 3);
        AddToWave(orc, 2);

        //ViewAllWave();
        //ClearWave();

        // Wave 3
        AddToWave(dragon, 1);

        //ViewAllWave();
        //ClearWave();
    }

    /// <summary>
    /// Call this method to create all types of enemies
    /// </summary>
    private void CreateEnemies()
    {
        goblin = Instantiate(goblinPrefab);
        orc = Instantiate(orcPrefab);
        dragon = Instantiate(dragonPrefab);

        goblin.Initialize("Goblin", 10 * playerLevel, 20 * playerLevel, 10 * playerLevel, playerLevel + 1, 10, 0);
        orc.Initialize("Orc", 100 * playerLevel, 5 * playerLevel, 30 * playerLevel, playerLevel + 2, 30, 0);
        dragon.Initialize("Dragon", 10 * playerLevel, 20 * playerLevel, 10 * playerLevel, playerLevel + 5, 500, 0);
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
            Character newCharacter = Instantiate(character);
            enemies.Add(newCharacter);
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
}
