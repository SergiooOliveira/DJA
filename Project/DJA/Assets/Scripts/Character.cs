using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Variables
    public int life;
    public int strenght;
    public int armor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="life">Character Life</param>
    /// <param name="strenght">Character Strenght</param>
    /// <param name="armor">Character Armor</param>
    public void Initialize(int life, int strenght, int armor) {
        this.life = Mathf.Max(0, life);
        this.strenght = Mathf.Max(0, strenght);
        this.armor = Mathf.Max(0, armor);
    }

    public virtual void Attack()
    {
        Debug.Log("Attacking");
        // Variables        
        int layerMask = LayerMask.GetMask("Enemy");        

        // Attack animation
        // animator.SetTrigger("Attack");

        // Detect enemies in range
        // Physics.OverlapSphere();
    }
}
