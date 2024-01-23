using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexaDoorScript : MonoBehaviour
{
    [SerializeField] GameObject lockedLight1, lockedLight2;
    [SerializeField] Material red, green;

    public bool isLocked;
    public bool isActive;
    [SerializeField] UnityEvent OnTrue;
    [SerializeField] UnityEvent OnFalse;
    Animator c_door_animator;
    [SerializeField] SoundInfo soundOpenDoor;

    private void Awake()
    {
        c_door_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (isActive) OnTrue.Invoke();
        else OnFalse.Invoke();

        CloseDoor();
    }

    private void Update()
    {
        if (!isLocked)
        {
            lockedLight1.GetComponent<MeshRenderer>().material = green;   
            lockedLight2.GetComponent<MeshRenderer>().material = green;   
        }
        else
        {
            lockedLight1.GetComponent<MeshRenderer>().material = red;
            lockedLight2.GetComponent<MeshRenderer>().material = red;
        }
    }


    public void SelectDoorBehaviour()
    {
        if (!isLocked)
        {
            GetComponent<SoundSpeaker>().CreateSoundBubble(soundOpenDoor, null, gameObject, false);
            isActive = (isActive) ? false : true;
            if (isActive) OpenDoor();
            else CloseDoor();
        }
    }

    public void OpenDoor()
    {
        OnTrue.Invoke();
        c_door_animator.SetBool("Active", isActive);
    }
    public void CloseDoor()
    {
        OnFalse.Invoke();
        c_door_animator.SetBool("Active", isActive);
    }

    public void UnlockDoor()
    {
        isLocked = false;
    }
}
