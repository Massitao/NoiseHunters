using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaManager : MonoBehaviour
{
    [Tooltip("Put here all active enemies")]
    [SerializeField] private List<EnemyEncounter> enemiesInArea;
 

    public void EnemyDeployment()
    {
        bool skipEncounter = false;

        for (int i = 0; i < enemiesInArea.Count; i++)
        {
            for (int j = 0; j < enemiesInArea[i].enemies.Count; j++)
            {
                if (enemiesInArea[i].enemies[j].IsAlive || enemiesInArea[i].enemiesToDeployIfKilled.Count == 0)
                {
                    skipEncounter = true;
                    break;
                }
            }

            if (!skipEncounter)
            {
                for (int k = 0; k < enemiesInArea[i].enemiesToDeployIfKilled.Count; k++)
                {
                    enemiesInArea[i].enemiesToDeployIfKilled[k].gameObject.SetActive(true);
                }
            }
            else
            {
                skipEncounter = false;
            }
        }

        Destroy(this);
    }
}

[System.Serializable]
public class EnemyEncounter
{
    [Tooltip("This name sets the List element name. Use this if you want to have a reference of the Enemy Encounter.")]
    public string name;

    public List<LivingEntity> enemies;

    [Tooltip("Put here all inactive enemies in the area you want to reactivate them after you kill this entity.")]
    public List<LivingEntity> enemiesToDeployIfKilled;
}