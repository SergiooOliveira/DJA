public abstract class State
{
    protected StatesMachine fsm;
    public State(StatesMachine fsm)
    {
        this.fsm = fsm;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
