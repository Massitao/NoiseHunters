using UnityEngine;

public class PermaShockEnemy : MonoBehaviour
{
    [SerializeField] private AIBrain brainShocked;

    // Start is called before the first frame update
    void Start()
    {
        if (brainShocked is RangedEnemyBrain)
        {
            RangedEnemyBrain reb = brainShocked as RangedEnemyBrain;
            reb.shock_StunTime = float.MaxValue;

            reb.ShockComponent.Shock_OnBeingElectrified();
        }
        else if (brainShocked is PredatorEnemyBrain)
        {
            PredatorEnemyBrain peb = brainShocked as PredatorEnemyBrain;
            peb.shock_StunTime = float.MaxValue;

            peb.ShockComponent.Shock_OnBeingElectrified();
        }
    }
}
