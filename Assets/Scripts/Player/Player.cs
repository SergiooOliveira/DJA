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
    public int baseHealth = 100;
    public int baseStrenght = 5;
    public int baseArmor = 10;

    // Level Stats
    public int baseLevel = 1;
    public int baseMaxXp = 0;
    public int baseXp = 0;

    public string baseName = "Player";
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
    private void SnapToGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
    }

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

    private void LevelUp()
    {
        Level++;
        MaxXp = CalculateMaxXp(Level);
        Debug.Log($"Player is Level = {Level} with a MaxXp of {MaxXp}");
    }

    private int CalculateMaxXp(int level)
    {
        return (int)(100 * Math.Pow(1.2, level - 1));
    }
    #endregion

    #region Unity Events
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
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
                    GainXp(50); // Dummy XP gain
                }
                else if (hit.collider.CompareTag(itemTag))
                {
                    GameObject original = hit.collider.gameObject;
                    ItemClass itemClass = original.GetComponent<ItemClass>();

                    if (!itemClass.isCollected)
                    {
                        inventory.AddInventoryItem(original);
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

                        GameObject new_item;
                        if (isArrayItem && targetArray != null && inventoryIndex != -1)
                        {
                            for (int i = 0; i < targetArray.Count; i++)
                            {
                                Transform parent = targetArray[i]?.transform?.parent;
                                new_item = Instantiate(inventory.InventorySlots[inventoryIndex].item, parent);
                                targetArray[i] = new_item;

                                new_item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                                new_item.transform.localScale = Vector3.one;
                                new_item.GetComponent<ItemClass>().isCollected = true;
                            }
                        }
                        else if (originalParent != null)
                        {
                            new_item = Instantiate(original, originalParent);
                            new_item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                            new_item.transform.localScale = Vector3.one;
                            new_item.GetComponent<ItemClass>().isCollected = true;
                        }

                        Destroy(original);
                    }
                }
                else
                {
                    Debug.Log($"Hited {hit.collider.name}");
                }
            }
        }
    }

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

    public void OpenInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            // Toggle logic to be implemented
        }
    }
    #endregion
}
