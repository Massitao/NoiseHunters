using System;
using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable, IRegenerable
{
    [Header("Health and Regen Values")]
    [SerializeField] protected bool _isAlive;
    public bool IsAlive
    {
        get { return _isAlive; }
        set { _isAlive = value; }
    }

    [SerializeField] protected bool _invincible;
    public bool Invincible
    {
        get { return _invincible; }
        set { _invincible = value; }
    }

    [SerializeField] protected int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField] protected int _currentHealth;
    public int Health
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;

            OnHealthValueChange?.Invoke(_currentHealth);
        }
    }

    protected Coroutine _regenCoroutine;
    public Coroutine RegenCoroutine
    {
        get { return _regenCoroutine; }
        set
        {
            if (_regenCoroutine != value)
            {
                _regenCoroutine = value;
                OnRegenEnd?.Invoke();
            }
        }
    }


    [Header("Entity Events")]
    public Action<int> OnHealthValueChange;
    public Action OnRegenEnd;
    public Action OnEntityHit;
    public Action<LivingEntity> OnEntityDeath;


    protected virtual void Awake()
    {
        RestoreAllHealth();
        CheckEntityLife();
    }


    protected void CheckEntityLife()
    {
        IsAlive = (Health > 0) ? true : false ;

        if (!IsAlive)
        {
            Death();
        }
    }

    public void Heal(int pointsToHeal)
    {
        if (Health + pointsToHeal <= MaxHealth)
        {
            Health += pointsToHeal;
        }
        else
        {
            Health = MaxHealth;
        }
    }
    public void Heal(int pointsToHeal, IDamagable targetToHeal)
    {
        if (targetToHeal.Health + pointsToHeal <= targetToHeal.MaxHealth)
        {
            targetToHeal.Heal(pointsToHeal);
        }
        else
        {
            targetToHeal.Health = targetToHeal.MaxHealth;
        }
    }
    public void RestoreAllHealth()
    {
        Health = MaxHealth;
    }

    public void RegenerateHealth(int pointToHealPerTick, float tickTime, int amountOfTicks, bool keepTryingToRegenAfterMaxHealth)
    {
        if (RegenCoroutine == null)
        {
            if (keepTryingToRegenAfterMaxHealth)
            {
                RegenCoroutine = StartCoroutine(Regenerating(pointToHealPerTick, tickTime, amountOfTicks, keepTryingToRegenAfterMaxHealth));
            }

            else
            {
                if (Health < MaxHealth)
                {
                    RegenCoroutine = StartCoroutine(Regenerating(pointToHealPerTick, tickTime, amountOfTicks, keepTryingToRegenAfterMaxHealth));
                }
            }
        }
    }
    public IEnumerator Regenerating(int pointToHealPerTick, float tickTime, int amountOfTicks, bool keepTryingToRegenAfterMaxHealth)
    {
        for (int i = 0; i < amountOfTicks; i++)
        {
            yield return new WaitForSeconds(tickTime);

            if (Health + pointToHealPerTick <= MaxHealth)
            {
                Heal(pointToHealPerTick);
            }
            else
            {
                Health = MaxHealth;

                if (keepTryingToRegenAfterMaxHealth)
                {
                    continue;
                }

                else
                {
                    RegenCoroutine = null;
                    yield break;
                }
            }
        }

        RegenCoroutine = null;
        yield break;
    }

    public void SetInvincibility(bool isInvincible)
    {
        Invincible = isInvincible;
    }

    public void DealDamage(int damageToDeal, IDamagable targetToHurt)
    {
        targetToHurt.TakeDamage(damageToDeal);
    }
    public void TakeDamage(int damageToTake)
    {
        if (!Invincible && IsAlive)
        {
            Health -= damageToTake;

            if (Health > 0)
            {
                EntityHit();
            }
            else
            {
                Death();
            }
        }
    }

    public void EntityHit()
    {
        OnEntityHit?.Invoke();
    }
    public void Death()
    {
        IsAlive = false;

        OnEntityDeath?.Invoke(this);
    }
}