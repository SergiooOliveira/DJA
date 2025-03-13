using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    private string doorTag = "Door";

    private void Start()
    {        
        Initialize(100, 10, 25);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
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
                    hit.transform.Rotate(0, 90, 0);
                }
                else
                {
                    print("Missed");
                    Debug.Log("Hited name: " + hit.collider.name);
                    Debug.Log("Hited tag: " + hit.collider.tag);
                }
            }
        }

        // movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
