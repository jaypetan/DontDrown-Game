using UnityEngine;

public abstract class BaseStateManager : MonoBehaviour
{
    protected StateMachine currentState;

    public void SwitchState(StateMachine newState)
    {
        currentState?.OnStateExit();
        currentState = newState;
        currentState.OnStateEnter();
    }

    // Other shared functionalities...
}

public class AggressiveStateManager : BaseStateManager
{
    // Implementation of aggressive-specific states and behaviors
}

public class PassiveStateManager : BaseStateManager
{
    // Implementation of passive-specific states and behaviors
}
