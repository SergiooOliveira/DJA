using System.Collections.Generic;
using UnityEngine;

public class Enemies : Character
{
    public List<Character> enemies = new List<Character>();

    // Create various prefabs for enemies
    Character goblin;
    Character orc;
    Character dragon;

    int level = Player.Instance.Level;

    private void Start()
    {                
        CreateEnemies();
    }

    /// <summary>
    /// Call this method to start a wave
    /// </summary>
    public void StartWave()
    {

    }

    /// <summary>
    /// Call this method to populate the wave
    /// </summary>
    private void CreateWave()
    {
        //Player.Instance.Level;
        
    }

    /// <summary>
    /// Call this method to create all types of enemies
    /// </summary>
    private void CreateEnemies()
    {
        goblin.Initialize(10 * level, 20 * level, 10 * level, level + 1, 10, 0);
        orc.Initialize(100 * level, 5 * level, 30 * level, level + 2, 30, 0);
        dragon.Initialize(10 * level, 20 * level, 10 * level, level + 5, 500, 0);

    }

}
