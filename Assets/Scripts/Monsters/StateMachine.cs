public abstract class StateMachine
{
    protected StateManager stateManager;

    public StateMachine(StateManager stateManager)
    {
        this.stateManager = stateManager;
    }

    public abstract void OnStateEnter();
    public abstract void RunState();
    public abstract void OnStateExit();
}
