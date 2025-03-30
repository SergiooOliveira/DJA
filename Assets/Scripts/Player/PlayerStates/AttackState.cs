using UnityEngine;

public class AttackState : IState
{
    public void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public void Update()
    {
        Debug.Log("AttackState Update");
        
    }

    public void FixedUpdate()
    {
        Debug.Log("AttackState FixedUpdate");
    }

    public void Exit()
    {
        Debug.Log("AttackState Exit");
    }
}
