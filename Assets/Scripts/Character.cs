using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    // Variables
    public string Name { get; set; }
    public int Health { get; set; }
    public int Strenght { get; set; }
    public int Armor { get; set; }
    public int Level { get; set; }
    public int Xp { get; set; }
    public int MaxXp { get; set; }

    /// <summary>
    /// Call this method to Initialize the player
    /// </summary>
    /// <param name="health">Character Health</param>
    /// <param name="strenght">Character Strenght</param>
    /// <param name="armor">Character Armor</param>
    /// <param name="level">Character Level</param>
    public void Initialize(string name = Player.baseName,int health = Player.baseHealth, int strenght = Player.baseStrenght, int armor = Player.baseArmor,
                            int level = Player.baseLevel, int xp = Player.baseXp, int maxXp = Player.baseMaxXp)
    {
        this.Health = Mathf.Max(0, health);
        this.Strenght = Mathf.Max(0, strenght);
        this.Armor = Mathf.Max(0, armor);
        this.Level = level;
        this.Xp = xp;
        this.MaxXp = maxXp;
    }

    /// <summary>
    /// Call this method to attack
    /// </summary>
    /// <param name="callbackContext"></param>
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            // Debug.Log("<color=red>Attacking</color>");
            Debug.Log("Attacking");
        }
    }
}
