public struct EnemyInitializeSettings
{
    public readonly int? level;

    public EnemyInitializeSettings(int? level)
    {
        this.level = level;
    }
}

public interface IDamageable
{
    void TakeDamage(float damage);
}