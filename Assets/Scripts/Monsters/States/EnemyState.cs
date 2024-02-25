public abstract class EnemyState
{
    public StateManager stateManager;
    public Enemy enemy;

    public abstract void OnStateEnter();
    public abstract void RunState();
    public abstract void OnStateExit();
}
