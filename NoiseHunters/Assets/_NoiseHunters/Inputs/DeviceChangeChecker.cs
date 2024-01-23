using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;


public class DeviceChangeChecker : MonoBehaviour
{
    public CurrentDevice currentDevice;
    public enum CurrentDevice { KM, XBOX, PS }

    public Action OnKMChange;
    public Action OnXBOXChange;
    public Action OnPSChange;

    private void Awake()
    {
        switch (InputUser.all[0].controlScheme.Value.name)
        {
            case "Keyboard & Mouse":
                currentDevice = CurrentDevice.KM;
                break;

            case "Xbox":
                currentDevice = CurrentDevice.XBOX;
                break;

            case "PlayStation":
                currentDevice = CurrentDevice.PS;
                break;
        }      
    }

    private void OnEnable()
    {
        InputUser.onChange += ControlSchemeChange;
    }
    private void OnDisable()
    {
        InputUser.onChange += ControlSchemeChange;
    }

    void ControlSchemeChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            switch(user.controlScheme.Value.name)
            {
                case "Keyboard & Mouse":
                    currentDevice = CurrentDevice.KM;
                    OnKMChange?.Invoke();
                    break;

                case "Xbox":
                    currentDevice = CurrentDevice.XBOX;
                    OnXBOXChange?.Invoke();
                    break;

                case "PlayStation":
                    currentDevice = CurrentDevice.PS;
                    OnPSChange?.Invoke();
                    break;
            }
        }
    }

}
