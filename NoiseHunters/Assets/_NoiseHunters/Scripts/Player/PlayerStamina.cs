using System;
using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float m_maxStamina;
    public float MaxStamina
    {
        get { return m_maxStamina; }
        set { m_maxStamina = value; }
    }

    [SerializeField] private float m_currentStamina;
    public float CurrentStamina
    {
        get { return m_currentStamina; }
        set
        {
            m_currentStamina = value;

            if (m_currentStamina > MaxStamina)
            {
                m_currentStamina = MaxStamina;
            }

            if (m_currentStamina < 0f)
            {
                m_currentStamina = 0f;
            }

            OnStaminaValueChange?.Invoke(m_currentStamina, MaxStamina);
        }
    }


    [SerializeField] private float m_staminaRecoveryPoints;
    [SerializeField] private float m_staminaRecoveryTickTime;

    [SerializeField] private float m_staminaRecoveryCooldownTime;


    private Coroutine m_staminaRecoveryCoroutine;
    private Coroutine m_staminaRecoveryCooldownCoroutine;


    [Header("Stamina Actions")]
    [SerializeField] private float m_staminaActionRetreiveDagger;
    public enum StaminaAction { None, Retreive_Dagger };

    private Action OnStaminaUse;
    public Action<float, float> OnStaminaValueChange;



    // Start is called before the first frame update
    void Start()
    {
        CurrentStamina = MaxStamina;
        OnStaminaUse += StartStaminaRecoveryCooldown;
    }


    [ContextMenu("DRAIN")]
    void Drain()
    {
        StaminaUse(75);
    }

    public bool DoStaminaAction(StaminaAction actionToUse)
    {
        float staminaToDrain = 0f;

        switch (actionToUse)
        {
            case StaminaAction.Retreive_Dagger:
                staminaToDrain = m_staminaActionRetreiveDagger;
                break;
        }

        if (StaminaCheck(staminaToDrain))
        {
            StaminaUse(staminaToDrain);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool StaminaCheck(float staminaToCompare)
    {
        return (staminaToCompare < CurrentStamina);
    }

    public void StaminaUse(float staminaToUse)
    {
        if (staminaToUse != 0f)
        {
            CurrentStamina -=  Mathf.Abs(staminaToUse);

            OnStaminaUse?.Invoke();
        }
    }

    void StartStaminaRecoveryCooldown()
    {
        if (m_staminaRecoveryCoroutine != null)
        {
            StopCoroutine(m_staminaRecoveryCoroutine);
            m_staminaRecoveryCoroutine = null;
        }

        if (m_staminaRecoveryCooldownCoroutine != null)
        {
            StopCoroutine(m_staminaRecoveryCooldownCoroutine);
        }

        m_staminaRecoveryCooldownCoroutine = StartCoroutine(StaminaRecoveryCooldown());
    }

    IEnumerator StaminaRecoveryCooldown()
    {
        yield return new WaitForSeconds(m_staminaRecoveryCooldownTime);

        if (m_staminaRecoveryCoroutine != null)
        {
            StopCoroutine(m_staminaRecoveryCooldownCoroutine);
        }

        m_staminaRecoveryCoroutine = StartCoroutine(StaminaRecovery());

        m_staminaRecoveryCooldownCoroutine = null;

        yield break;
    }

    IEnumerator StaminaRecovery()
    {
        while (CurrentStamina != MaxStamina)
        {
            CurrentStamina += m_staminaRecoveryPoints;

            yield return new WaitForSeconds(m_staminaRecoveryTickTime);
        }

        m_staminaRecoveryCoroutine = null;

        yield break;
    }

}
