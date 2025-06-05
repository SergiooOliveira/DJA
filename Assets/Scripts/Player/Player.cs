using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : Character
{
    // Singleton
    public static Player Instance;
    public static StatesMachine statesMachine = new();
    private State idleState;
    private State runningState;
    private State attackState;

    #region Base Stats
    // Fight Stats
    [HideInInspector] public string baseName = "Player";
    [HideInInspector] public int baseHealth = 100;
    [HideInInspector] public int baseStrenght = 500;
    [HideInInspector] public int baseArmor = 10;

    // Level Stats
    [HideInInspector] public int baseLevel = 1;
    [HideInInspector] public int baseMaxXp = 100;
    [HideInInspector] public int baseXp = 0;

    [HideInInspector] public int skillPoints = 0;
    #endregion

    #region Variables
    // Controllers
    public CharacterController controller;
    public static Animator animator;

    // Tags for interactions
    private readonly string doorTag = "Door";
    private readonly string itemTag = "Item";
    private readonly string fortuneWheelTag = "FortuneWheel";

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
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        if (OpenItemPanel == null)
            OpenItemPanel = new UnityEvent();
    }

    private void Start()
    {
        // Game logic
        animator = GetComponent<Animator>();
        //LevelUp();

        idleState = new PlayerIdleState(fsm: statesMachine, player: Instance);
        runningState = new PlayerRunningState(fsm: statesMachine, player: Instance);
        attackState = new PlayerAttackState(fsm: statesMachine, player: Instance);
        statesMachine?.ChangeState(idleState);

        GameManager.Instance.UpdateLevelXP();
    }

    private void Update()
    {
        statesMachine?.Update();
    }

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        SnapToGround();
        statesMachine?.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Lock doors

        //Debug.Log(message: "Triggered Spawning");

        GameObject triggerParent = other.transform.parent.gameObject;
        foreach (Transform triggers in triggerParent.transform)
        {
            Destroy(triggers.gameObject);
        }

        // Get the current room spawnPoints
        GameObject room = triggerParent.transform.parent.gameObject;
        Transform spawnPoints = room.transform.Find("SpawnPoints");

        if (spawnPoints == null)
        {
            print("SpawnPoints null");
            return;
        }

        // Trigger spawns
        Enemies.Instance.StartWave(spawnPoints);
    }
    #endregion

    #region Unity Events
    /// <summary>
    /// Call this method to interact with an object
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        animator = GetComponent<Animator>();
        if (callbackContext.started)
        {
            Vector3 pos = UnityEngine.Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(pos);

            if (Physics.Raycast(
                ray: ray,
                hitInfo: out RaycastHit hit,
                maxDistance: 2f))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag(tag: doorTag))
                    {
                        hit.transform.rotation = new Quaternion(x: 0, y: 120, z: 0, w: 0);
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor(name: "_Color", value: Color.black);
                        clickSource.Play();
                    }
                    else if (hit.collider.CompareTag(tag: itemTag))
                    {
                        GameObject original = hit.collider.gameObject;
                        ItemClass itemClass = original.GetComponent<ItemClass>();

                        if (!itemClass.isCollected)
                        {
                            inventory.AddInventoryItem(itemObject: original);
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
                                    Transform parent = targetArray[i].transform.parent;
                                    new_item = Instantiate(original: inventory.InventorySlots[inventoryIndex].item, parent: parent);
                                    targetArray[i] = new_item;

                                    new_item.transform.SetLocalPositionAndRotation(localPosition: Vector3.zero, localRotation: Quaternion.identity);
                                    new_item.transform.localScale = Vector3.one;
                                    new_item.GetComponent<ItemClass>().isCollected = true;
                                }
                            }
                            else if (originalParent != null)
                            {
                                new_item = Instantiate(original: original, parent: originalParent);
                                new_item.transform.SetLocalPositionAndRotation(localPosition: Vector3.zero, localRotation: Quaternion.identity);
                                new_item.transform.localScale = Vector3.one;
                                new_item.GetComponent<ItemClass>().isCollected = true;
                            }

                            Destroy(obj: original);
                        }
                        clickSource.Play();
                    }
                    else if (hit.collider.CompareTag(tag: fortuneWheelTag))
                    {
                        hit.collider.gameObject.GetComponent<FortuneWheel>()?.SpinWheel();
                        clickSource.Play();
                    }
                    else
                    {
                        // Case we want to do something
                        //Debug.Log(message: $"Hited {hit.collider.name}");
                    }
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

        }
        else if (callbackContext.canceled)
        {
            moveInput = Vector2.zero;
        }
        movementX = moveInput.x;
        movementY = moveInput.y;
    }

    /// <summary>
    /// Call this method to open and close the SkillTree
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnOpeningSkillTree(InputAction.CallbackContext callbackContext)
    {
        // Always updating on opening
        GameManager.Instance.UpdateSkillPoints();

        if (callbackContext.performed)
        {
            Debug.Log("Opening Inventory");
            //Canvas canvas = SkillTreeManager.Instance.Canvas.GetComponent<Canvas>();

            // Activate and Deactivate SkillTree Canvas
            if (SkillTreeManager.Instance.Canvas.activeSelf)
            {
                SkillTreeManager.Instance.Canvas.SetActive(false);
                GameManager.Instance.TogglePause(false);
            }
            else
            {
                SkillTreeManager.Instance.Canvas.SetActive(true);
                GameManager.Instance.TogglePause(true);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext callbackContext)
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
            statesMachine.ChangeState(attackState);
        }
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

    #region States
    public class PlayerAttackState : State
    {
        private readonly Player player;
        const string punch = "boolPunching";
        const string sword = "boolSword";
        private float attackDuration = 1f; 
        private float timer;

        public PlayerAttackState(StatesMachine fsm, Player player) : base(fsm)
        {
            this.player = player;
        }

        public override void Enter()
        {
            timer = 0f;

            if (player.inventory.InventorySlots[0] != null)
            {
                animator.SetBool(sword, true);
            }
            else
            {
                animator.SetBool(punch, true);
            }

            Debug.Log("Attack animation started");
        }

        public override void Update(float deltaTime)
        {
            timer += deltaTime;

            if (timer >= attackDuration)
            {
                if (player.moveInput.magnitude > 0)
                {
                    fsm.ChangeState(player.runningState);
                }
                else
                {
                    fsm.ChangeState(player.idleState);
                }
            }
        }

        public override void Exit()
        {
            animator.SetBool(punch, false);
            animator.SetBool(sword, false);
        }
    }

    /// <summary>
    /// This class is a state that represents the idle state of the player.
    /// (It will be used to control the player's idle animation and state transitions)
    /// </summary>
    public class PlayerIdleState : State
    {
        private readonly Player player;
        const string animationName = "boolIdle";

        public PlayerIdleState(StatesMachine fsm, Player player) : base(fsm)
        {
            this.player = player;
        }
        public override void Enter()
        {
            //Debug.Log(message: "PlayerIdleState State");
            animator.SetBool(name: animationName, value: true);
        }
        public override void Update(float deltaTime)
        {
            if (player.moveInput.magnitude > 0)
            {
                fsm.ChangeState(newState: player.runningState);
            }
        }
        public override void Exit()
        {
            animator.SetBool(name: animationName, value: false);
        }
    }
    /// <summary>
    /// This class is a state that represents the running state of the player.
    /// (It will be used to control the player's running animation and state transitions)
    /// </summary>
    public class PlayerRunningState : State
    {
        private readonly Player player;
        const string animationName = "boolRunning";
        public PlayerRunningState(StatesMachine fsm, Player player) : base(fsm)
        {
            this.player = player;
        }
        public override void Enter()
        {
            //Debug.Log(message: "PlayerMovementState State");
            animator.SetBool(name: animationName, value: true);
        }
        public override void Update(float deltaTime)
        {
            if (player.velocity == Vector3.zero)
            {
                fsm.ChangeState(newState: player.idleState);
            }
        }
        public override void FixedUpdate(float deltaTime)
        {
            float angleRad = player.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float rotationX = Mathf.Cos(f: angleRad);
            float rotationZ = Mathf.Sin(f: angleRad);

            Vector3 inputDirection = new Vector3(
                x: player.movementX * rotationX + player.movementY * rotationZ,
                y: 0.0f,
                z: player.movementY * rotationX - player.movementX * rotationZ
            ).normalized;

            bool hasInput = (player.movementX != 0 || player.movementY != 0);

            if (hasInput)
            {
                // Accelerate in the input direction
                player.velocity += accelerationSpeed * deltaTime * inputDirection;

                // Clamp velocity to maxSpeed
                if (player.velocity.magnitude > maxSpeed)
                    player.velocity = player.velocity.normalized * maxSpeed;
            }
            else
            {
                // Decelerate naturally
                if (player.velocity.magnitude > 0)
                {
                    Vector3 decel = decelerationSpeed * deltaTime * player.velocity.normalized;
                    if (decel.magnitude > player.velocity.magnitude)
                        player.velocity = Vector3.zero;
                    else
                        player.velocity -= decel;
                }
            }

            player.controller.Move(motion: player.velocity * deltaTime);
        }
        public override void Exit()
        {
            animator.SetBool(name: animationName, value: false);
        }
    }
    #endregion

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
