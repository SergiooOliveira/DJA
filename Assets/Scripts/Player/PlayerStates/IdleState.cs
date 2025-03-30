using System;
using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleState Enter");
    }

    public void Update()
    {
        Debug.Log("IdleState Update");
    }

    public void FixedUpdate()
    {
        Debug.Log("IdleState FixedUpdate");
    }

    public void Exit()
    {
        Debug.Log("IdleState Exit");
    }
}
