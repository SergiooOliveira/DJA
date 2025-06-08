using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class Character : MonoBehaviour
{
    public UnityEvent OpenItemPanel;

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
        set => strength = Mathf.Max(1, value);
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
    /// <param name="strength">Character Strenght</param>
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
                GameObject hitted = hit.collider.transform.root.gameObject;

                if (hitted.CompareTag(tag: enemyTag))
                {
                    Enemy rayCharacter = hitted.GetComponent<Enemy>();
                    Debug.Log($"rayCharacter: {rayCharacter.tag}");
                    Debug.Log($"rayCharacter: {rayCharacter.name}");
                    rayCharacter.TakeDamage(Player.Instance.Strength, rayCharacter.name);
                    AudioManager.Instance.PlaySfx(0); // Assuming 0 is the index for attack sound
                }
                else
                {
                    Debug.Log($"Hitted {hit.collider.tag.ToString()}");
                }
            }
        }
    }

    public void TakeDamage(int damage, string enemytype)
    {
        int actualDamage = damage - Armor;

        if (actualDamage <= 0) actualDamage = 1;

        Health -= actualDamage;
        Debug.Log($"{Name} took {actualDamage} damage. Current health: {Health}");
        AudioManager.Instance.PlaySfx(1);

        GameObject canvasGameObject = new GameObject("DamageTextCanvas");
        canvasGameObject.transform.SetParent(transform, true);

        Canvas dmgCanvas = canvasGameObject.AddComponent<Canvas>();
        canvasGameObject.AddComponent<CanvasScaler>();
        canvasGameObject.AddComponent<GraphicRaycaster>();

        dmgCanvas.renderMode = RenderMode.WorldSpace;
        dmgCanvas.worldCamera = Camera.main;

        float position = 1.25f;
        if(enemytype.Contains("Orc"))
            position = 2.25f;

        dmgCanvas.transform.SetPositionAndRotation(transform.position + new Vector3(0, position, 0), Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up));

        GameObject dmgText = new GameObject("DamageText");

        dmgText.transform.SetParent(dmgCanvas.transform, false);

        dmgText.AddComponent<RectTransform>();
        dmgText.AddComponent<MeshRenderer>();

        TextMeshPro textMeshPro = dmgText.AddComponent<TextMeshPro>();

        textMeshPro.text = $"-{actualDamage}";
        textMeshPro.fontSize = 1 + actualDamage * .01f;
        textMeshPro.color = Color.red;
        textMeshPro.fontStyle = FontStyles.Bold;
        textMeshPro.isOverlay = true;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        Destroy(canvasGameObject, 1 + actualDamage * .01f);
        if (Health == 0) OnDeath();
    }

    //public void Heal(int amount)
    //{
    //    Health += amount;
    //    Debug.Log($"{Name} healed {amount} health. Current health: {Health}");
    //}

    public void OnDeath()
    {
        // In case Player kills
        if (!gameObject.CompareTag("Player"))
        {
            Player.Instance.GainXp(50 + level * 3);
            Destroy(gameObject);
            Enemies.Instance.enemyCounter--;

            if (Enemies.Instance.enemyCounter == 0)
            {
                Debug.Log("Starting new wave");
                Enemies.Instance.waveCounter++;
                Enemies.Instance.CreateWave();
            }
        }
        else if (gameObject.CompareTag("Player"))
        {            
            // GameOver screen
            GameManager.Instance.GameOver();

            Invoke("InvokePause", 0.8f);
        }        
    }

    private void InvokePause()
    {
        GameManager.Instance.TogglePause();
    }
}
