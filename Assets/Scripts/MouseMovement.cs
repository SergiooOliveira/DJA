using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private readonly float mouseSensitiviy = 3f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitiviy;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitiviy;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
