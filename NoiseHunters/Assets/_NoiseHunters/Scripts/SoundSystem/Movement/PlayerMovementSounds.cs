using UnityEngine;

public class PlayerMovementSounds : MonoBehaviour
{
    SoundSpeaker mySoundSpeaker;
    ThirdPersonController c_CharacterController;

    [SerializeField] private SoundInfo stepWalking, stepRunning, stepCrouching, stepLanding;
    

    void Start()
    {
        c_CharacterController = GetComponent<ThirdPersonController>();
    }

    public SoundInfo SelectingMovementSound()
    {
        SoundInfo soundToChoose = null;

        switch (c_CharacterController.MovementCurrentState)
        {
            case ThirdPersonController.MovementState.Walking:
                soundToChoose = stepWalking;
                break;


            case ThirdPersonController.MovementState.Crouching:
                switch (c_CharacterController.CrouchingCurrentState)
                {
                    case ThirdPersonController.CrouchingState.Normal:

                        soundToChoose = stepCrouching;
                        break;
                }
                break;


            case ThirdPersonController.MovementState.Running:
                soundToChoose = stepRunning;
                break;
        }

        return soundToChoose;
    }

    public SoundInfo LandSound()
    {
        return stepLanding;
    }
}