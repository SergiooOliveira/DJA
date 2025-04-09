using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class Player : Character
{
    // Singleton
    public static Player Instance;

    #region Base Stats
    [Header("Base Stats")]
    // Fight Stats
    public const int baseHealth = 100;
    public const int baseStrenght = 5;
    public const int baseArmor = 10;

    // Level Stats
    public const int baseLevel = 1;
    public const int baseMaxXp = 0;
    public const int baseXp = 0;

    #endregion

    #region Variables
    // Controllers
    [Header("Controllers")]
    public CharacterController controller;
    public Animator animator;

    // Tags for interactions
    [Header("Tags")]
    private readonly string doorTag = "Door";
    private readonly string slotTag = "SlotMachine";
    private readonly string itemTag = "Item";

    // Movement
    [Header("Movement")]
    public float speed = 5f;
    private Vector2 moveInput;

    // Inventory
    [Header("Inventory")]
    public GameObject mainHand;
    public GameObject offHand;
    public GameObject helmet;
    public GameObject chestPlate;
    public List<GameObject> legPlate;
    public List<GameObject> footWear;
    public GameObject amulet;
    // public List<GameObject> BodyList = new() { mainHand, offHand, helmet, chestPlate, chestPlate, legPlate, footWear, amulet };
    public Inventory inventory = new(10);
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
    private void GainXp(int xp)
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
                else if (hit.collider.CompareTag(itemTag))
                {

                    GameObject original = hit.collider.gameObject;
                    ItemClass itemClass = original.GetComponent<ItemClass>();

                    inventory.AddInventoryItem(original);
                    Destroy(original);

                    GameObject new_item = null;
                    Transform originalParent = null;

                    bool isArrayItem = false;
                    List<GameObject> targetArray = null;
                    int inventoryIndex = -1;

                    switch (itemClass.Type)
                    {
                        case ItemType.MainHand:
                            originalParent = mainHand.transform.parent;
                            mainHand = inventory.InventorySlots[0].item;
                            break;

                        case ItemType.OffHand:
                            originalParent = offHand.transform.parent;
                            offHand = inventory.InventorySlots[1].item;
                            break;

                        case ItemType.Helmet:
                            originalParent = helmet.transform.parent;
                            helmet = inventory.InventorySlots[2].item;
                            break;

                        case ItemType.ChestPlate:
                            originalParent = chestPlate.transform.parent;
                            chestPlate = inventory.InventorySlots[3].item;
                            break;

                        case ItemType.LegsPlate:
                            isArrayItem = true;
                            targetArray = legPlate;
                            inventoryIndex = 4;
                            break;

                        case ItemType.FootWear:
                            isArrayItem = true;
                            targetArray = footWear;
                            inventoryIndex = 5;
                            break;

                        case ItemType.Amulet:
                            originalParent = amulet.transform.parent;
                            amulet = inventory.InventorySlots[6].item;
                            break;
                    }

                    if (isArrayItem && targetArray != null && inventoryIndex != -1)
                    {
                        for (int i = 0; i < targetArray.Capacity; i++)
                        {
                            Transform parent = targetArray[i]?.transform?.parent;
                            new_item = Instantiate(inventory.InventorySlots[inventoryIndex].item, parent);
                            targetArray[i] = new_item;

                            new_item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                            new_item.transform.localScale = Vector3.one;
                        }
                    }
                    else if (originalParent != null)
                    {
                        new_item = Instantiate(original, originalParent);
                        new_item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                        new_item.transform.localScale = Vector3.one;
                    }

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
