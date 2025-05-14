public abstract class State
{
    protected StatesMachine fsm;
    public State(StatesMachine fsm)
    {
        this.fsm = fsm;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update(float deltaTime) { }
    public virtual void FixedUpdate(float deltaTime) { }
}
