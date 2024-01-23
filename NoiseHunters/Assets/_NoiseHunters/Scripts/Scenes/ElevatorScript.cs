using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    [Header("Mesh Renderer Values")]
    [SerializeField] private MeshRenderer rend;
    [SerializeField] private Material inactiveMat;
    [SerializeField] private Material activeMat;

    [Header("Thing To Move")]
    [SerializeField] GameObject thingToMove;

    [Header("Times")]
    [SerializeField] private float timeToTravel, activeHeight;

    [Header("Sounds")]
    [SerializeField] private SoundInfo activationSound;
    [SerializeField] private SoundInfo deactivationSound;

    // OTHER
    private bool _isActive;
    float inactiveHeight;
    SoundGenerator mySoundGenerator;
    SoundSpeaker soundSpeaker;

    Coroutine magnetMoveCoroutine;

    private bool IsActive
    {
        get { return _isActive; }
        set
        {
            if (_isActive != value)
            {
                _isActive = value;

                MagnetMoveOperation();
            }
        }
    }

    private bool _isMoving;
    private bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            if (_isMoving != value)
            {
                _isMoving = value;

                MagnetDetectMovement();
            }
        }
    }


    void Awake()
    {
        inactiveHeight = transform.position.y;
        rend.sharedMaterial = (_isActive) ? activeMat: inactiveMat;
        mySoundGenerator = GetComponent<SoundGenerator>();
        soundSpeaker = gameObject.AddComponent<SoundSpeaker>();
    }


    void MagnetMoveOperation()
    {
        if (magnetMoveCoroutine != null)
        {
            StopCoroutine(magnetMoveCoroutine);
        }

        magnetMoveCoroutine = StartCoroutine(MagnetMove(IsActive));
    }

    IEnumerator MagnetMove(bool up)
    {
        float yPosToMove = (up) ? activeHeight : inactiveHeight ;
        Material matToUse = (up) ? activeMat : inactiveMat;

        float initialLerpPosition = (up) ? Mathf.InverseLerp(inactiveHeight, activeHeight, transform.position.y) : Mathf.InverseLerp(activeHeight, inactiveHeight, transform.position.y);
        float initialPosition = thingToMove.transform.position.y;
        Material initialMat = rend.sharedMaterial;

        float timer = Time.time;
        float timeToArrive = timeToTravel * (1 - initialLerpPosition);

        float lerpTime = 0f;      

        while (lerpTime < 1f || thingToMove.transform.position.y != yPosToMove)
        {
            // Get Lerp percentage
            lerpTime = Mathf.InverseLerp(timer, timer + timeToArrive, Time.time);

            thingToMove.transform.position = new Vector3(thingToMove.transform.position.x, Mathf.Lerp(initialPosition, yPosToMove, lerpTime), thingToMove.transform.position.z);
            rend.material.Lerp(initialMat, matToUse, lerpTime);


            IsMoving = true;

            yield return null;
        }

        thingToMove.transform.position = new Vector3(thingToMove.transform.position.x, yPosToMove, thingToMove.transform.position.z);

        IsMoving = false;
        magnetMoveCoroutine = null;

        yield break;
    }

    void MagnetDetectMovement()
    {
        if (IsMoving)
        {
            mySoundGenerator.ActivateGenerator();
        }
        else
        {
            mySoundGenerator.DeactivateGenerator();
        }
    }

    public void MagnedEnabled()
    {
        IsActive = true;
    }
    public void MagnedDisabled()
    {
        IsActive = false;
    }


    void PlayActivationSound()
    {
        if (activationSound == null)
        {
            return;
        }
        soundSpeaker.CreateSoundBubble(activationSound, null, gameObject, false);
    }
    void PlayDeactivationSound()
    {
        if (deactivationSound == null)
        {
            return;
        }
        soundSpeaker.CreateSoundBubble(deactivationSound, null, gameObject, false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SwordProjectile>())
        {
            MagnedEnabled();
        }
    }
}
