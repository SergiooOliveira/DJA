using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    #region Variables
    public CharacterController controller;

    private string doorTag = "Door";

    private Vector2 moveInput;
    public float speed = 5f;
    #endregion

    #region MonoBehaviour
    private void Start()
    {        
        Initialize(100, 10, 25);
    }

    private void Awake()
    {
        SnapToGround();
    }

    private void Update()
    {

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
    /// 
    /// </summary>
    /// <param name="callbackContext"></param>
    public void OnInteract (InputAction.CallbackContext callbackContext)
    {
        /*
         * if (callbackContext.performed)
         */


        print("E click");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            print("Sending raycast");
            if (hit.collider.CompareTag(doorTag))
            {
                print("Hit door");
                print("Transforming now");
                //hit.transform.Rotate(0, 90, 0);

                /*
                 * Code for animator and moving the door
                 * For now just destroing collider
                 */
                hit.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                Destroy(hit.collider);
            }
            else
            {
                print("Missed");
                Debug.Log("Hited name: " + hit.collider.name);
                Debug.Log("Hited tag: " + hit.collider.tag);
            }
        }
    }

    /// <summary>
    /// 
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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callbackContext"></param>
    public void Attack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
            Debug.Log("<color=red>Attacking</color>");

    }
    #endregion

}
