using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Character
{
    private static Player Instance;

    #region Variables
    public CharacterController controller;
    public Animator animator;

    private string doorTag = "Door";
    private string slotTag = "SlotMachine";

    private Vector2 moveInput;
    public float speed = 5f;

    [Header("Icons UI")]
    public Transform icons;
    public GameObject upgradeObject;
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

    private void ShowUpgradesUI()
    {
        //print("Showing upgrades");
        foreach (Upgrade upgrade in Upgrades.Instance.playerUpgrades)
        {
            Upgrades.Instance.ToString(upgrade);

            GameObject newIcon = Instantiate(upgradeObject, icons);

            //TMP_Text textComponent = newIcon.GetComponentInChildren<TMP_Text>();
            //if (textComponent != null) textComponent.text = upgrade.UpgradeName;

            Image spriteComponent = newIcon.GetComponentInChildren<Image>();
            if (spriteComponent != null) spriteComponent.sprite = upgrade.Icon;
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

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.CompareTag(doorTag))
                {
                    //print("Hit door");
                    //print("Transforming now");
                    hit.transform.Rotate(0, 90, 0);
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                }
                else if (hit.collider.CompareTag(slotTag))
                {
                    // Activate gambling mechanics
                    // For now it only rolls 1 Upgrade
                    GamblingManager.Instance.StartRolling();
                    ShowUpgradesUI();
                }
                else
                {
                    //print("Missed");
                    //Debug.Log("Hited name: " + hit.collider.name);
                    //Debug.Log("Hited tag: " + hit.collider.tag);
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
