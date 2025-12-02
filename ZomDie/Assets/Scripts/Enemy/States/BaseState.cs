public abstract class BaseState
{
    public StateMachine m_stateMachine;

    public Enemy m_Enemy;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

}