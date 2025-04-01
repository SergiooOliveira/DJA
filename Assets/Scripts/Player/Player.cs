using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : Character
{
    // Singleton
    public static Player Instance;

    #region Base Stats
    // Fight Stats
    public const int baseHealth = 100;
    public const int baseStrenght = 5;
    public const int baseArmor = 10;

    // Level Stats
    public const int baseLevel = 1;
    public const int baseMaxXp = 0;
    public const int baseXp = 0;

    public const int inventorySize = 3;
    public InventoryClass inventoryClass;
    #endregion

    #region Variables
    // Controllers
    public CharacterController controller;
    public Animator animator;

    // Tags for interactions
    private readonly string doorTag = "Door";
    private readonly string slotTag = "SlotMachine";

    // Movement
    private Vector2 moveInput;
    public float speed = 5f;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Game logic
        SnapToGround();
        animator = GetComponent<Animator>();
        LevelUp();
        GameManager.Instance.UpdateLevelXP();

        inventoryClass = new InventoryClass(3); // Create inventory with 3 slots, all set to None.

        inventoryClass.AddItem(ItemType.Sword);  // Replaces first None (index 0)
        inventoryClass.AddItem(ItemType.Shield); // Replaces first None (index 1)
        inventoryClass.AddItem(ItemType.Sword);  // Replaces first None (index 2)
        inventoryClass.AddItem(ItemType.Shield); // Inventory is full!
    }

    private void FixedUpdate()
    {
        Vector3 movement = speed * Time.fixedDeltaTime * new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(movement);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Call the method to place the player in the ground
    /// </summary>
    private void SnapToGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            transform.position = hit.point; // Move player to the ground
        }
    }

    /// <summary>
    /// Call this method to give xp to Player
    /// </summary>
    /// <param name="xp">xp to add</param>
    private void GainXp (int xp)
    {        
        Xp += xp;         

        while (Xp >= MaxXp)
        {
            Xp -= MaxXp;
            LevelUp();

            GamblingManager.Instance.StartRolling();
        }

        GameManager.Instance.UpdateLevelXP();
    }

    /// <summary>
    /// Call this method to Level Up
    /// </summary>
    private void LevelUp()
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

    #endregion

    #region Unity Events
    /// <summary>
    /// Call this method to interact with an object
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        // print("E outside");
        if (callbackContext.started)
        {
            //print("E inside");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag(doorTag))
                {
                    hit.transform.Rotate(0, 90, 0);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                }
                else if (hit.collider.CompareTag(slotTag))
                {
                    // Activate gambling mechanics
                    GainXp(50); // Change 50 to the enemy xp                    
                }
                else
                {
                    // Case we want to do something
                    Debug.Log($"Hited {hit.collider.name}");
                }
            }
        }
    }

    /// <summary>
    /// Call this method to move
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            moveInput = callbackContext.ReadValue<Vector2>();
            animator.SetBool("isMoving", true);
        }
        else if (callbackContext.canceled)
        {
            moveInput = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
    }

    /// <summary>
    /// Call this method to open and close the inventory
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OpenInventory(InputAction.CallbackContext callbackContext)
    {
        // Should have a bool to control if the inventory is open or closed. Toggle between those states
        if (callbackContext.started)
        {

        }
    }
    #endregion

}
