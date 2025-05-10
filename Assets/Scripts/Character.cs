using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
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
    public int Strenght {
        get => strength;
        set => strength = Mathf.Max(0, value);
    }
    public int Armor {
        get => armor;
        set => armor = Mathf.Max(0, value);
    }
    public int Level {
        get => level;
        set => level = Mathf.Max(0, value);
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
    public void Initialize(string name,int health, int strenght, int armor,
                            int level, int xp, int maxXp)
    {
        this.Name = name;
        this.Health = health;
        this.Strenght = strenght;
        this.Armor = armor;
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
            //Debug.Log("Attacking");
        }
    }
}