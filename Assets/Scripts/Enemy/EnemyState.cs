public abstract class EnemyState
{
    protected EnemyAI _enemy;

    public EnemyState(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public abstract void UpdateState();
}
