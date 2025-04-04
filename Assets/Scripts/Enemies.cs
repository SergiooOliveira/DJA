using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public List<Character> enemies;

    // Create various prefabs for enemies
    Character goblin;
    Character orc;
    Character dragon;

    int level = 0; // Player.Instance.Level;
    int waveCounter = 0;

    private void Start()
    {                
        CreateEnemies();
        enemies = new List<Character>();
    }

    /// <summary>
    /// Call this method to start a wave
    /// </summary>
    public void StartWave()
    {
        CreateWave();
    }

    /// <summary>
    /// Call this method to populate the wave
    /// </summary>
    private void CreateWave()
    {
        // Wave 1
        AddToWave(goblin, 3);

        ViewAllWave();
        ClearWave();

        // Wave 2
        AddToWave(goblin, 3);
        AddToWave(orc, 2);

        ViewAllWave();
        ClearWave();

        // Wave 3
        AddToWave(dragon, 1);

        ViewAllWave();
        ClearWave();
    }

    /// <summary>
    /// Call this method to create all types of enemies
    /// </summary>
    private void CreateEnemies()
    {
        goblin.Initialize("Goblin", 10 * level, 20 * level, 10 * level, level + 1, 10, 0);
        orc.Initialize("Orc", 100 * level, 5 * level, 30 * level, level + 2, 30, 0);
        dragon.Initialize("Dragon", 10 * level, 20 * level, 10 * level, level + 5, 500, 0);
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
    }

    /// <summary>
    /// Call this method to view all enemies in the list
    /// </summary>
    public void ViewAllWave()
    {
        foreach(Character character in enemies)
        {
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
