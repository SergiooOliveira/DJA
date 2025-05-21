using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    private readonly string enemyTag = "Enemy";
    private string characterName;
    private int health;
    private int strength;
    private int armor;
    private int level;
    private int xp;
    private int maxXp;

    // Variables
    public string Name {
        get => characterName; 
        set => characterName = value; 
    }
    public int Health { 
        get => health;
        set => health = Mathf.Max(0, value);
    }
    public int Strength {
        get => strength;
        set => strength = Mathf.Max(0, value);
    }
    public int Armor {
        get => armor;
        set => armor = Mathf.Max(0, value);
    }
    public int Level {
        get => level;
        set => level = Mathf.Max(1, value);
    }
    public int Xp {
        get => xp;
        set => xp = Mathf.Max(0, value);
    }
    public int MaxXp {
        get => maxXp;
        set => maxXp = Mathf.Max(0, value);
    }

    /// <summary>
    /// Call this method to Initialize the player
    /// </summary>
    /// <param name="health">Character Health</param>
    /// <param name="strenght">Character Strenght</param>
    /// <param name="armor">Character Armor</param>
    /// <param name="level">Character Level</param>
    public void Initialize(string name, int health, int strength, int armor,
                            int level, int xp, int maxXp)
    {
        this.Name = name;
        this.Health = health;
        this.Strength = strength;
        this.Armor = armor;
        this.Level = level;
        this.Xp = xp;
        this.MaxXp = maxXp;
    }

    /// <summary>
    /// Call this method to Level Up
    /// </summary>
    public void LevelUp()
    {
        Level++;
        MaxXp = CalculateMaxXp(Level);

        Debug.Log($"Player is Level = {Level} with a MaxXp of {MaxXp}");
    }

    /// <summary>
    /// Calculates max xp exponentially
    /// </summary>
    /// <param name="level">Current Level of the Player</param>
    /// <returns></returns>
    private int CalculateMaxXp(int level)
    {
        return (int)(100 * Math.Pow(1.2, level - 1));
    }

    /// <summary>
    /// Call this method to give xp to Player
    /// </summary>
    /// <param name="xp">xp to add</param>
    private void GainXp(int xp)
    {
        Player.Instance.Xp += xp;
        Debug.Log($"Player got {xp} xp ({Player.Instance.Xp})");

        while (Player.Instance.Xp >= Player.Instance.MaxXp)
        {
            Player.Instance.Xp -= Player.Instance.MaxXp;
            LevelUp();
            Debug.Log($"Player level up ({Player.Instance.Level})");

            GamblingManager.Instance.StartRolling();
        }

        GameManager.Instance.UpdateLevelXP();
    }

    /// <summary>
    /// Call this method to attack
    /// </summary>
    /// <param name="callbackContext"></param>
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos: UnityEngine.Input.mousePosition);

            if (Physics.Raycast(
                ray: ray,
                hitInfo: out RaycastHit hit,
                maxDistance: 2f))
            {
                if (hit.collider.CompareTag(tag: enemyTag))
                {
                    Character rayCharacter = hit.collider.gameObject.GetComponent<Character>();
                    TakeDamage(hittedCharacter: rayCharacter, attackerCharacter: this);
                }
            }
        }
    }

    public void TakeDamage(Character hittedCharacter, Character attackerCharacter)
    {
        int actualDamage = attackerCharacter.Strength - hittedCharacter.Armor;
        if (actualDamage < 1)
        {
            actualDamage = 1;
        }
        hittedCharacter.Health -= actualDamage;
        if (hittedCharacter.Health <= 0)
        {
            hittedCharacter.OnDeath(killedCharacter: hittedCharacter, KillerCharacter: attackerCharacter);
        }
        Debug.Log($"{hittedCharacter.Name} took {actualDamage} damage. Remaining health: {hittedCharacter.Health}");
    }

    //public void Heal(int amount)
    //{
    //    Health += amount;
    //    Debug.Log($"{Name} healed {amount} health. Current health: {Health}");
    //}

    public void OnDeath()
    {        
        GainXp(50);
        Destroy(gameObject);
    }
}
