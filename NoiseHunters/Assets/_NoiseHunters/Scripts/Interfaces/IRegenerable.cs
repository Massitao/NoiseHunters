using System.Collections;
using UnityEngine;

public interface IRegenerable
{
    void RegenerateHealth(int pointToHealPerTick, float tickTime, int amountOfTicks, bool keepTryingToRegenAfterMaxHealth);

    IEnumerator Regenerating(int pointToHealPerTick, float tickTime, int amountOfTicks, bool keepTryingToRegenAfterMaxHealth);

    Coroutine RegenCoroutine { get; set; }
}