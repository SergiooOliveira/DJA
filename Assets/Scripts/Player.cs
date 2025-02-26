using UnityEngine;

public class Player : Character
{
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
    }

}
