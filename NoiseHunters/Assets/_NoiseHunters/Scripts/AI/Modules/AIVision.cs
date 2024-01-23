using System;
using System.Collections;
using UnityEngine;

public class AIVision : MonoBehaviour
{
    [Header("Vision States")]
    [SerializeField] protected bool _visionActive;
    [HideInInspector] public bool VisionActive
    {
        get { return _visionActive; }
        set
        {
            if (_visionActive != value)
            {
                _visionActive = value;

                if (_visionActive)
                {
                    OnVisionActivation?.Invoke();
                }
                else
                {
                    OnVisionDeactivation?.Invoke();
                }
            }
        }
    }

    [SerializeField] protected bool _visionTargetOnSight;
    [HideInInspector] public bool VisionTargetOnSight
    {
        get { return _visionTargetOnSight; }
        set
        {
            if (_visionTargetOnSight != value)
            {
                _visionTargetOnSight = value;

                if (_visionTargetOnSight)
                {
                    OnTargetSpotted?.Invoke();
                }
                else
                {
                    OnTargetLost?.Invoke();
                }
            }
        }
    }

    [HideInInspector] public bool VisionTargetCheat;


    [Header("Vision Target")]
    [SerializeField] protected Transform viewTransform;

    [SerializeField] protected Transform playerTransform;
    [HideInInspector] public Vector3 lastTargetSeenPosition;


    [Header("Vision Distance and Angle")]
    [SerializeField] protected float _viewDistance;
    [SerializeField] public float ViewDistance
    {
        get { return _viewDistance; }
        set
        {
            if (_viewDistance != value)
            {
                _viewDistance = value;

                Vision_CalculateViewMultiplier();
            }
        }
    }

    [SerializeField] protected float viewAngle;

    protected float viewDistanceBetween;
    [HideInInspector] public float viewMultiplier;


    [Header("Vision Raycasts")]
    [SerializeField] protected float viewSphereCastRadius;
    [SerializeField] protected LayerMask viewMask;


    [Header("Vision Events")]
    public Action OnVisionActivation;
    public Action OnVisionDeactivation;

    public Action OnTargetSpotted;
    public Action OnTargetLost;



    protected virtual void OnEnable()
    {
        // Suscribe to Events
        OnVisionDeactivation += Vision_DisableVision;
    }
    protected virtual void OnDisable()
    {
        // If Vision is active, invoke Deactivation Event.
        if (VisionActive)
        {
            OnVisionDeactivation?.Invoke();
        }

        // Unsuscribe from Events
        OnVisionDeactivation -= Vision_DisableVision;
    }

    protected virtual void Start()
    {
        if (VisionActive)
        {
            OnVisionActivation?.Invoke();
        }
        else
        {
            OnVisionDeactivation?.Invoke();
        }
    }


    void VisionCheck()
    {
        VisionTargetOnSight = CanSeePlayer();

        if (VisionTargetOnSight || VisionTargetCheat)
        {
            Vision_Update_LastSeenPosition();
        }
    }
    public bool CanSeePlayer()
    {
        if (playerTransform.TryGetComponent(out ThirdPersonController playerController))
        {
            float distanceBetweenEnemyAndTarget = Vector3.Distance(viewTransform.position, playerController.c_CharacterController.bounds.center);

            if (distanceBetweenEnemyAndTarget < ViewDistance && playerTransform.GetComponent<PlayerHealth>().IsAlive)
            {
                viewDistanceBetween = distanceBetweenEnemyAndTarget;

                Vector3 dirToPlayer = (playerController.c_CharacterController.bounds.center - viewTransform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

                if (angleBetweenGuardAndPlayer < viewAngle)
                {
                    if (Physics.SphereCast(viewTransform.position, viewSphereCastRadius, dirToPlayer, out RaycastHit sphereHit, ViewDistance, viewMask, QueryTriggerInteraction.Ignore) && sphereHit.transform == playerTransform)
                    {
                        return true;
                    }

                    else if (Physics.Linecast(viewTransform.position, playerController.c_CharacterController.bounds.center, out RaycastHit hit, viewMask, QueryTriggerInteraction.Ignore) && hit.transform == playerTransform)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }
        return false;
    }


    #region VISION TOOLS
    public void Vision_SetTarget(Transform target)
    {
        playerTransform = target;
    }

    public void Vision_Cheat(bool cheat)
    {
        VisionTargetCheat = cheat;
    }

    public void Vision_Update_LastSeenPosition()
    {
        lastTargetSeenPosition = playerTransform.position;
    }

    public void Vision_CalculateViewMultiplier()
    {
        viewMultiplier = Mathf.InverseLerp(ViewDistance, 0f, viewDistanceBetween);
    }
    #endregion


    #region Event Methods
    // OnVisionDeactivation
    public void Vision_DisableVision()
    {
        VisionTargetOnSight = false;
        viewDistanceBetween = 0f;
    }
    #endregion


    private void OnDrawGizmosSelected()
    {
        if (viewTransform != null && playerTransform != null)
        {
            Gizmos.DrawLine(viewTransform.position, playerTransform.GetComponent<ThirdPersonController>().c_CharacterController.bounds.center);
        }
    }
}
