using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    // Variables
    public int Health;
    public int Strenght;
    public int Armor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="health">Character Health</param>
    /// <param name="strenght">Character Strenght</param>
    /// <param name="armor">Character Armor</param>
    public void Initialize(int health = 100, int strenght = 5, int armor = 10) {
        this.Health = Mathf.Max(0, health);
        this.Strenght = Mathf.Max(0, strenght);
        this.Armor = Mathf.Max(0, armor);
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
