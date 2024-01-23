public interface IDamagable
{
    bool IsAlive { get; set; }
    bool Invincible { get; set; }

    int MaxHealth { get; set; }
    int Health { get; set; }

    void SetInvincibility(bool isInvincible);

    void Heal(int pointsToHeal);
    void Heal(int pointsToHeal, IDamagable targetToHeal);

    void RestoreAllHealth();

    void DealDamage(int damageToDeal, IDamagable targetToHurt);
    void TakeDamage(int damageToTake);

    void EntityHit();
    void Death();
}