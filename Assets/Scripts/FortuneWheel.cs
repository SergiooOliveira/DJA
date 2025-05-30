using UnityEngine;

public class FortuneWheel : MonoBehaviour
{
    [Header("Wheel Settings")]

    // Speed of the wheel
    [Range(0.1f, 500f)]
    [SerializeField] private float speed = 100f;

    // Deceleration factor
    [Range(0.01f, 1f)]
    [SerializeField] private float deceleration = 0.98f;

    // Current speed of the wheel
    [SerializeField] private float currentSpeed = 0f;
    // Current deceleration factor
    private float currentDeceleration = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Start spinning the wheel
            currentDeceleration += Mathf.Min(1f, deceleration);
            currentDeceleration = Mathf.Clamp(currentDeceleration, 0.01f, 1f);
            currentSpeed += speed;
            currentSpeed = Mathf.Clamp(currentSpeed, 0.1f, 5000f);
        }
        if (currentDeceleration > 0)
        {
            // Apply deceleration
            currentSpeed *= currentDeceleration;
            // Rotate the wheel
            transform.Rotate(0, currentSpeed *Time.deltaTime, 0);
            // Reduce current deceleration
            currentDeceleration -= Time.deltaTime;
            // Stop the wheel if speed is low enough
            if (currentSpeed < 0.01f)
            {
                currentSpeed = 0f;
                currentDeceleration = 0f;
            }
        }
    }
}
