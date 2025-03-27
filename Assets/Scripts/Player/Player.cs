using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : Character
{
    public static Player Instance;

    #region Variables
    public CharacterController controller;
    public Animator animator;

    private string doorTag = "Door";
    private string slotTag = "SlotMachine";

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
        //Initialize(100, 10, 25);
        SnapToGround();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.fixedDeltaTime;
        controller.Move(movement);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Call the method to place the player in the ground
    /// </summary>
    private void SnapToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = hit.point; // Move player to the ground
        }
    }

    #endregion

    #region Unity Events
    /// <summary>
    /// Call this method to interact with an object
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        //print("E outside");
        if (callbackContext.started)
        {
            //print("E inside");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.collider.CompareTag(doorTag))
                {
                    hit.transform.Rotate(0, 90, 0);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                }
                else if (hit.collider.CompareTag(slotTag))
                {
                    // Activate gambling mechanics
                    GamblingManager.Instance.StartRolling();
                    GameManager.Instance.UpdateUpgradesUI();
                }
                else
                {
                    // Case we want to do something
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
    /// Call this method to attack
    /// </summary>
    /// <param name="callbackContext"></param>
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            // Debug.Log("<color=red>Attacking</color>");
        }


    }
    #endregion

}
