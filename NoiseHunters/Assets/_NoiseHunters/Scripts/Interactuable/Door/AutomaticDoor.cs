using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticDoor : MonoBehaviour
{
    [Header("Logic Components")]
    private Animator doorAnimator;
    private SoundSpeaker soundSpeaker;

    #region Door Animator Values
    [Header("Animator Values")]
    [SerializeField] private string doorOpenStateBool;
    #endregion

    #region Door Components
    [Header("Door Components")]
    [SerializeField] private GameObject doorStateLight;
    private MeshRenderer doorStateLightMeshRenderer;

    [SerializeField] private GameObject frontDoorButton;
    private AutomaticDoorButton frontDoorButtonScript;
    private MeshRenderer frontDoorButtonMeshRenderer;

    [SerializeField] private GameObject backDoorButton;
    private AutomaticDoorButton backDoorButtonScript;
    private MeshRenderer backDoorButtonMeshRenderer;
    #endregion

    #region Door States
    [Header("Door State")]
    [Tooltip("Should the door receive any player input?")]
    [SerializeField] private bool isActive = true;
    private bool IsActive
    {
        get { return isActive; }
        set
        {
            if (value != isActive)
            {
                isActive = value;

                if (isActive)
                {
                    OnDoorActivate?.Invoke();
                }
                else
                {
                    OnDoorDeactivate?.Invoke();
                }
            }
        }
    }

    [Tooltip("Is the door locked? If so, receive player input but return error event")]
    [SerializeField] private bool isLocked = false;
    public bool IsLocked
    {
        get { return isLocked; }
        set
        {
            if (value != isLocked)
            {
                isLocked = value;

                if (isLocked)
                {
                    OnDoorLocked?.Invoke();
                }
                else
                {
                    OnDoorUnlocked?.Invoke();
                }
            }
        }
    }

    [Tooltip("Is the door opened?")]
    [SerializeField] private bool isOpen = false;
    private bool IsOpen
    {
        get { return isOpen; }
        set
        {
            if (value != isOpen)
            {
                isOpen = value;

                if (isOpen)
                {
                    OnDoorOpen?.Invoke();
                }
                else
                {
                    OnDoorClose?.Invoke();
                }
            }
        }
    }
    #endregion

    #region Door State Materials
    [Header("Door State Materials")]
    [SerializeField] private Material doorOpenedMaterial;
    [SerializeField] private Material doorClosedMaterial;
    #endregion

    #region Door Automatic Close
    [Header("Door Automatic Close")]
    [SerializeField] private float automaticCloseTime;
    private Coroutine automaticCloseCoroutine;
    #endregion

    #region Door Sounds
    [Header("Door Sounds")]
    [SerializeField] private SoundInfo activateDoorSound;
    [SerializeField] private SoundInfo deactivateDoorSound;

    [Space(5)]

    [SerializeField] private SoundInfo openingDoorSound;
    [SerializeField] private SoundInfo closingDoorSound;

    [Space(5)]

    [SerializeField] private SoundInfo automaticCloseDoorSound;

    [Space(5)]

    [SerializeField] private SoundInfo unlockingDoorSound;
    [SerializeField] private SoundInfo lockingDoorSound;
    #endregion

    #region Door Events
    [Header("Door Events - Scene related")]
    [SerializeField] private UnityEvent OnDoorActivate;
    [SerializeField] private UnityEvent OnDoorDeactivate;

    [Space(5)]

    [SerializeField] private UnityEvent OnDoorOpen;
    [SerializeField] private UnityEvent OnDoorClose;

    [Space(5)]

    [SerializeField] private UnityEvent OnDoorLocked;
    [SerializeField] private UnityEvent OnDoorUnlocked;
    #endregion



    private void Awake()
    {
        doorAnimator = GetComponent<Animator>();
        
        if (TryGetComponent(out SoundSpeaker ss))
        {
            soundSpeaker = ss;
        }
        else
        {
            soundSpeaker = gameObject.AddComponent<SoundSpeaker>();
        }

        doorStateLightMeshRenderer = doorStateLight.GetComponent<MeshRenderer>();

        frontDoorButtonMeshRenderer = frontDoorButton.GetComponent<MeshRenderer>();
        frontDoorButtonScript = frontDoorButton.GetComponentInParent<AutomaticDoorButton>();

        backDoorButtonMeshRenderer = backDoorButton.GetComponent<MeshRenderer>();
        backDoorButtonScript = backDoorButton.GetComponentInParent<AutomaticDoorButton>();
    }

    private void Start()
    {
        IsActive = true;

        doorStateLightMeshRenderer.material = (IsLocked) ? doorClosedMaterial : doorOpenedMaterial;

        frontDoorButtonMeshRenderer.material = (IsOpen) ? doorOpenedMaterial : doorClosedMaterial;
        frontDoorButtonScript.IsActive = IsActive;

        backDoorButtonMeshRenderer.material = (IsOpen) ? doorOpenedMaterial : doorClosedMaterial;
        backDoorButtonScript.IsActive = IsActive;

        doorAnimator.SetBool(doorOpenStateBool, isOpen);
    }

    public void SelectDoorBehaviour()
    {
        if (IsActive)
        {
            if (!IsLocked)
            {
                if (IsOpen)
                {
                    CloseDoor();
                }
                else
                {
                    OpenDoor();                
                }
            }
        }
    }


    public void ActivateDoor()
    {
        IsActive = true;

        frontDoorButtonScript.IsActive = IsActive;
        backDoorButtonScript.IsActive = IsActive;

        PlayActivateDoorSound();
    }
    public void DeactivateDoor()
    {
        IsActive = false;

        frontDoorButtonScript.IsActive = IsActive;
        backDoorButtonScript.IsActive = IsActive;

        PlayDeactivateDoorSound();
    }

    public void UnlockDoor()
    {
        IsLocked = false;

        doorStateLightMeshRenderer.material = doorOpenedMaterial;

        PlayUnlockSound();
    }
    public void LockDoor()
    {
        IsLocked = true;

        doorStateLightMeshRenderer.material = doorClosedMaterial;

        PlayLockDoorSound();
    }

    public void OpenDoor()
    {
        IsOpen = true;

        frontDoorButtonMeshRenderer.material = doorOpenedMaterial;
        backDoorButtonMeshRenderer.material = doorOpenedMaterial;

        PlayOpeningDoorSound();

        doorAnimator.SetBool(doorOpenStateBool, IsOpen);
    }
    public void CloseDoor()
    {
        IsOpen = false;

        frontDoorButtonMeshRenderer.material = doorClosedMaterial;
        backDoorButtonMeshRenderer.material = doorClosedMaterial;

        PlayClosingDoorSound();

        doorAnimator.SetBool(doorOpenStateBool, IsOpen);
    }
    public void AutomaticCloseDoor()
    {
        if (IsOpen)
        {
            CloseDoor();
            PlayAutomaticCloseDoorTriggeredSound();
        }
    }


    private IEnumerator AutomaticCloseDoorTimer()
    {
        yield return new WaitForSeconds(automaticCloseTime);

        CloseDoor();
        PlayAutomaticCloseDoorTriggeredSound();

        automaticCloseCoroutine = null;
        yield break;
    }


    #region Sounds Methods
    public void PlayActivateDoorSound()
    {
        PlaySound(activateDoorSound, doorStateLight.transform.position, false);
    }
    public void PlayDeactivateDoorSound()
    {
        PlaySound(deactivateDoorSound, doorStateLight.transform.position, false);
    }

    public void PlayUnlockSound()
    {
        PlaySound(unlockingDoorSound, doorStateLight.transform.position, false);
    }
    public void PlayLockDoorSound()
    {
        PlaySound(lockingDoorSound, doorStateLight.transform.position, false);
    }

    public void PlayOpeningDoorSound()
    {
        PlaySound(openingDoorSound, transform.position, false);
    }
    public void PlayClosingDoorSound()
    {
        PlaySound(closingDoorSound, transform.position, false);
    }

    public void PlayAutomaticCloseDoorTriggeredSound()
    {
        PlaySound(automaticCloseDoorSound, doorStateLight.transform.position, false);
    }

    private void PlaySound(SoundInfo soundToPlay, Vector3? posToPlaySound, bool onlySound)
    {
        if (soundToPlay != null)
        {
            soundSpeaker.CreateSoundBubble(soundToPlay, posToPlaySound, gameObject, onlySound);
        }
    }
    #endregion
}