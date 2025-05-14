using UnityEngine;

public class StatesMachine
{
    private State currentState;
    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    public void Update() => currentState?.Update(Time.deltaTime);
    public void FixedUpdate() => currentState?.FixedUpdate(Time.fixedDeltaTime);
}
