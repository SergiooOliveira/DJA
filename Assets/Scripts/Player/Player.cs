using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : Character
{
	// Singleton
	public static Player Instance;

	#region Base Stats
	// Fight Stats
	[HideInInspector] public string baseName = "Player";
	[HideInInspector] public int baseHealth = 100;
	[HideInInspector] public int baseStrenght = 5;
	[HideInInspector] public int baseArmor = 10;

	// Level Stats
	[HideInInspector] public int baseLevel = 1;	
	[HideInInspector] public int baseXp = 0;
    public int baseMaxXp = 100;
    #endregion

    #region Variables
    // Controllers
    public CharacterController controller;
	public Animator animator;

	// Tags for interactions
	private readonly string doorTag = "Door";
	private readonly string slotTag = "SlotMachine";
	private readonly string itemTag = "Item";
	
	// Movement
	private Vector2 moveInput;
	private const float maxSpeed = 6f;
	private const float accelerationSpeed = maxSpeed * 4;
    private const float decelerationSpeed = accelerationSpeed * 1.5f;
	private float movementX, movementY;

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
        // Game logic        
        SnapToGround();
		animator = GetComponent<Animator>();
		//LevelUp();
        GameManager.Instance.UpdateLevelXP();
    }

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        float angleRad = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float rotationX = Mathf.Cos(angleRad);
        float rotationZ = Mathf.Sin(angleRad);

        Vector3 inputDirection = new Vector3(
            movementX * rotationX + movementY * rotationZ,
            0.0f,
            movementY * rotationX - movementX * rotationZ
        ).normalized;

        bool hasInput = (movementX != 0 || movementY != 0);

        if (hasInput)
        {
            // Accelerate in the input direction
            velocity += accelerationSpeed * Time.fixedDeltaTime * inputDirection;

            // Clamp velocity to maxSpeed
            if (velocity.magnitude > maxSpeed)
                velocity = velocity.normalized * maxSpeed;
        }
        else
        {
            // Decelerate naturally
            if (velocity.magnitude > 0)
            {
                Vector3 decel = decelerationSpeed * Time.fixedDeltaTime * velocity.normalized;
                if (decel.magnitude > velocity.magnitude)
                    velocity = Vector3.zero;
                else
                    velocity -= decel;
            }
        }

        controller.Move(velocity * Time.fixedDeltaTime);
    }

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Triggered Spawning");

		// Destroy other triggers of the scene
		GameObject triggerParent = other.transform.parent.gameObject;
		foreach (Transform triggers in triggerParent.transform)
		{
			Destroy(triggers.gameObject);
		}

		// Lock doors

		// Trigger spawns
		Enemies.Instance.StartWave();
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
			Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit, 2f))
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
		movementX = moveInput.x;
		movementY = moveInput.y;
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
