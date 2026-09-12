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
    void HandleDamage(int damage, int maxHealth);
    void HandleHeal(int heal, int maxHealth);
    void HandleDeath();
}
