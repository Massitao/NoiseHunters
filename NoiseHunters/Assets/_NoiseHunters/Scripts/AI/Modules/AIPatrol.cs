using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [Header("Patrol State")]
    [HideInInspector] public bool pathDetected;


    [Header("Patrol Holder")]
    public Transform pathHolder;
    protected PatrolPointInfo[] patrolWayPoints;
    protected Vector3[] patrolPointPositions;
    protected float[] patrolPointWaitTimes;


    [Header("Patrol Current Point")]
    protected int currentIndexPatrolPoint;


    protected void Start()
    {
        Patrol_SetPatrolPoints();
    }

    protected void Patrol_SetPatrolPoints()
    {
        if (pathHolder != null && pathHolder.GetComponentsInChildren<PatrolPointInfo>().Length >= 2)
        {
            pathDetected = true;

            patrolWayPoints = pathHolder.GetComponentsInChildren<PatrolPointInfo>();
            patrolPointPositions = new Vector3[patrolWayPoints.Length];

            for (int i = 0; i < patrolWayPoints.Length; i++)
            {
                patrolPointPositions[i] = new Vector3(patrolWayPoints[i].transform.position.x, patrolWayPoints[i].transform.position.y, patrolWayPoints[i].transform.position.z);
            }

            patrolPointWaitTimes = new float[pathHolder.childCount];

            for (int i = 0; i < patrolPointWaitTimes.Length; i++)
            {
                patrolPointWaitTimes[i] = pathHolder.GetChild(i).GetComponent<PatrolPointInfo>().timeToWait;
            }
        }
        else
        {
            pathDetected = false;
        }
    }


    #region PATROL TOOLS
    public bool Patrol_CheckPatrolPoint()
    {
        return patrolWayPoints[currentIndexPatrolPoint].checkThisPoint;
    }
    public bool Patrol_LookAtPatrolPointRotation()
    {
        return patrolWayPoints[currentIndexPatrolPoint].lookAtPointRotation;
    }


    public void Patrol_PointChange()
    {
        currentIndexPatrolPoint = (currentIndexPatrolPoint + 1) % patrolWayPoints.Length;
    }
    public Transform Patrol_GetCurrentPointPosition()
    {
        return patrolWayPoints[currentIndexPatrolPoint].transform;
    }
    public int Patrol_GetCurrentPointIndex()
    {
        return currentIndexPatrolPoint;
    }
    public float Patrol_GetPointWaitTime()
    {
        return patrolPointWaitTimes[currentIndexPatrolPoint];
    }


    public void Patrol_CheckNearestPatrolPoint()
    {
        float distanceBetweenAIandPoint = float.MaxValue;
        int currentPatrolPoint = currentIndexPatrolPoint;

        for (int i = 0; i < patrolPointPositions.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, patrolPointPositions[i]);

            if (distance < distanceBetweenAIandPoint)
            {
                currentPatrolPoint = i;
                distanceBetweenAIandPoint = distance;
            }
        }

        currentIndexPatrolPoint = currentPatrolPoint;
    }
    #endregion


    private void OnDrawGizmosSelected()
    {
        if (pathHolder != null)
        {
            Vector3 startPos = pathHolder.GetChild(0).position;
            Vector3 previousPos = startPos;
            foreach (Transform waypoint in pathHolder)
            {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPos, waypoint.position);
                previousPos = waypoint.position;
            }
            Gizmos.DrawLine(previousPos, startPos);
            Gizmos.color = Color.red;
        }
    }
}
