using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMoveAgent : MonoBehaviour
{
    [Header("Agent Components")]
    protected NavMeshAgent navMeshAgentAI;


    [Header("Agent State")]
    [SerializeField] protected bool _agentActive = true;
    public bool AgentActive
    {
        get { return _agentActive; }
        set
        {
            if (_agentActive != value)
            {
                _agentActive = value;

                Agent_CheckActiveState();
            }
        }
    }


    [Header("Agent Values")]
    protected float _agentVelocity;
    protected float AgentVelocity
    {
        get { return _agentVelocity; }
        set
        {
            if (_agentVelocity != value)
            {
                _agentVelocity = value;

                OnAgentVelocityChanged?.Invoke(_agentVelocity);
            }
        }
    }

    protected Coroutine agentVelocityUpdaterCoroutine;


    [Header("Agent Events")]
    public Action OnAgentActivation;
    public Action OnAgentDeactivation;

    public Action<float> OnAgentVelocityChanged;


    protected void Awake()
    {
        TryGetComponent(out NavMeshAgent thisEntityNavMeshAgent);

        navMeshAgentAI = thisEntityNavMeshAgent;
    }


    protected virtual void OnEnable()
    {
        // Suscribe to Events
        OnAgentActivation += Agent_Activation;
        OnAgentDeactivation += Agent_Deactivation;
    }
    protected virtual void OnDisable()
    {
        // Unsuscribe from Events
        OnAgentActivation -= Agent_Activation;
        OnAgentDeactivation -= Agent_Deactivation;
    }


    protected void Start()
    {
        if (AgentActive)
        {
            OnAgentActivation?.Invoke();
        }
        else
        {
            OnAgentDeactivation?.Invoke();
        }
    }


    public void Agent_SetActive(bool stateToSet)
    {
        AgentActive = stateToSet;
    }

    protected void Agent_CheckActiveState()
    {
        if (_agentActive)
        {
            OnAgentActivation?.Invoke();
        }
        else
        {
            OnAgentDeactivation?.Invoke();
        }
    }
    protected void Agent_Activation()
    {
        if (!navMeshAgentAI.enabled)
        {
            navMeshAgentAI.enabled = true;
        }

        Agent_StartVelocityUpdaterCoroutine();
    }
    protected void Agent_Deactivation()
    {
        Agent_ClearDestination();
        Agent_SetMovementSpeed(0f);
        AgentVelocity = Agent_GetMovementSpeed();

        Agent_StopVelocityUpdaterCoroutine();

        if (navMeshAgentAI.enabled)
        {
            navMeshAgentAI.enabled = false;
        }
    }


    protected void Agent_StartVelocityUpdaterCoroutine()
    {
        // Check if Velocity Updater Coroutine is active. If so, stop the coroutine
        if (agentVelocityUpdaterCoroutine != null)
        {
            StopCoroutine(agentVelocityUpdaterCoroutine);
        }

        // Set and start Velocity Updater Coroutine
        agentVelocityUpdaterCoroutine = StartCoroutine(Agent_VelocityUpdater());
    }
    protected IEnumerator Agent_VelocityUpdater()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            AgentVelocity = Agent_GetMovementVelocity().magnitude;
        }
    }
    protected void Agent_StopVelocityUpdaterCoroutine()
    {
        // Check if Velocity Updater is active. If so, stop the coroutine
        if (agentVelocityUpdaterCoroutine != null)
        {
            StopCoroutine(agentVelocityUpdaterCoroutine);
            agentVelocityUpdaterCoroutine = null;
        }
    }


    #region AGENT TOOLS
    public Vector3 Agent_GetMovementVelocity()
    {
        return navMeshAgentAI.velocity;
    }
    public float Agent_GetMovementSpeed()
    {
        return navMeshAgentAI.speed;
    }
    public void Agent_SetMovementSpeed(float speedToSet)
    {
        navMeshAgentAI.speed = speedToSet;
    }


    public float Agent_GetStoppingDistance()
    {
        return navMeshAgentAI.stoppingDistance;
    }
    public void Agent_SetStoppingDistance(float stoppingDistanceToSet)
    {
        navMeshAgentAI.stoppingDistance = stoppingDistanceToSet;
    }


    public void Agent_SetDestination(Vector3 destinationToSet)
    {
        if (navMeshAgentAI.enabled)
        {
            navMeshAgentAI.SetDestination(destinationToSet);
        }
    }
    public Vector3 Agent_GetDestination()
    {
        return navMeshAgentAI.destination;
    }
    public void Agent_SetStopState(bool doStop)
    {
        navMeshAgentAI.isStopped = doStop;
    }
    public void Agent_ClearDestination()
    {
        if (navMeshAgentAI.enabled)
        {
            navMeshAgentAI.ResetPath();
        }
    }


    public float Agent_GetRemainingDistance()
    {
        return navMeshAgentAI.remainingDistance;
    }
    public bool Agent_HasPathPending()
    {
        return navMeshAgentAI.pathPending;
    }
    public bool Agent_HasArrivedToDestination()
    {
        return (Agent_GetRemainingDistance() <= Agent_GetStoppingDistance());
    }

    public Vector3 Agent_SetRandomPointToInvestigate(float distanceToInvestigate)
    {
        Vector3 investigatePoint = Vector3.zero;

        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);

        investigatePoint.x = transform.position.x + (UnityEngine.Random.Range(-distanceToInvestigate, distanceToInvestigate));
        investigatePoint.y = transform.position.y + (UnityEngine.Random.Range(-distanceToInvestigate, distanceToInvestigate));
        investigatePoint.z = transform.position.z + (UnityEngine.Random.Range(-distanceToInvestigate, distanceToInvestigate));

        return investigatePoint;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (navMeshAgentAI != null && !Agent_HasPathPending())
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Agent_GetDestination(), 0.5f);
        }
    }
}
