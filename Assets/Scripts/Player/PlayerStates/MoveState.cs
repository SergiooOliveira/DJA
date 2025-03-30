using UnityEngine;

public class MoveState : IState
{
    public void Enter()
    {
        Debug.Log("MoveState Enter");
    }

    public void Update()
    {
        Debug.Log("MoveState Update");
    }

    public void FixedUpdate()
    {
        Debug.Log("MoveState FixedUpdate");
    }

    public void Exit()
    {
        Debug.Log("MoveState Exit");
    }
}
