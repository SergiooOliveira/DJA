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

    // Sounds Effects
    [Header("Sound Effects")]
    public GameObject click;
    public GameObject hitHurt;
    public GameObject powerUp;

    public AudioSource clickSource;
    public AudioSource hitHurtSource;
    public AudioSource powerUpSource;

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

        click = new GameObject("ClickSound");
        hitHurt = new GameObject("HitHurtSound");
        powerUp = new GameObject("PowerUpSound");

        click.transform.SetParent(this.transform);
        hitHurt.transform.SetParent(this.transform);
        powerUp.transform.SetParent(this.transform);

        clickSource = click.AddComponent<AudioSource>();
        hitHurtSource = hitHurt.AddComponent<AudioSource>();
        powerUpSource = powerUp.AddComponent<AudioSource>();

        clickSource.clip = Resources.Load<AudioClip>("Sfx/click");
        hitHurtSource.clip = Resources.Load<AudioClip>("Sfx/hitHurt");
        powerUpSource.clip = Resources.Load<AudioClip>("Sfx/powerUp");

        clickSource.playOnAwake = false;
        hitHurtSource.playOnAwake = false;
        powerUpSource.playOnAwake = false;
    }

    /// <summary>
    /// Call this method to Level Up
    /// </summary>
    public void LevelUp()
    {
        Player.Instance.Level++;
        Player.Instance.MaxXp = CalculateMaxXp(Player.Instance.Level);

        //Debug.Log($"Player is Level = {Player.Instance.Level} with a MaxXp of {Player.Instance.MaxXp}");
        OpenItemPanel?.Invoke();
        powerUp.GetComponent<AudioSource>().Play();

        //Upgrades.Instance.playerPowerUp.Add(Upgrades.Instance.GetRandomPowerUp());
        //Upgrades.Instance.playerPowerUp.Add(Upgrades.Instance.GetRandomPowerUp());
        //Upgrades.Instance.playerPowerUp.Add(Upgrades.Instance.GetRandomPowerUp());
        //GameManager.Instance.ShowPowerUpSelector();
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
        //Debug.Log($"Player got {xp} xp ({Player.Instance.Xp})");

        while (Player.Instance.Xp >= Player.Instance.MaxXp)
        {
            Player.Instance.Xp -= Player.Instance.MaxXp;
            LevelUp();
            //Debug.Log($"Player level up ({Player.Instance.Level})");

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
                GameObject hitted = hit.collider.transform.root.gameObject;

                if (hitted.CompareTag(tag: enemyTag))
                {
                    Enemy rayCharacter = hitted.GetComponent<Enemy>();
                    Debug.Log($"rayCharacter: {rayCharacter.tag}");
                    rayCharacter.TakeDamage(Player.Instance.Strength);
                    hitHurtSource.Play();
                }
                else
                {
                    Debug.Log($"Hitted {hit.collider.tag.ToString()}");
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = damage - Armor;

        if (actualDamage <= 0) actualDamage = 1;

        Health -= actualDamage;
        Debug.Log($"{Name} took {actualDamage} damage. Current health: {Health}");
        hitHurtSource.Play();

        GameObject canvasGameObject = new GameObject("DamageTextCanvas");

        canvasGameObject.transform.SetParent(transform, true);

        Canvas dmgCanvas = canvasGameObject.AddComponent<Canvas>();
        CanvasScaler canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
        GraphicRaycaster graphicRaycaster = canvasGameObject.AddComponent<GraphicRaycaster>();

        dmgCanvas.renderMode = RenderMode.WorldSpace;
        dmgCanvas.worldCamera = Camera.main;
        dmgCanvas.transform.SetPositionAndRotation(transform.position + new Vector3(0, 1.25f + actualDamage * .005f, 0), Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up));

        GameObject dmgText = new GameObject("DamageText");

        dmgText.transform.SetParent(dmgCanvas.transform, false);

        RectTransform textRect = dmgText.AddComponent<RectTransform>();
        MeshRenderer textRenderer = dmgText.AddComponent<MeshRenderer>();
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
        GainXp(50 +level *3);
        Destroy(gameObject);
        Enemies.Instance.enemyCounter--;

        if (Enemies.Instance.enemyCounter == 0)
        {
            Debug.Log("Starting new wave");
            Enemies.Instance.waveCounter++;
            Enemies.Instance.CreateWave();
        }
    }
}
