using UnityEngine;

public class PistonAnimator : MonoBehaviour
{
    [Header("Pistons")]
    public Transform piston_1;
    public Transform piston_2;
    public Transform piston_3;
    public Transform piston_4;

    [Header("Animation Parameters")]
    public float downSpeed;
    public float upSpeed;
    public float restTime;
    public float timeBetween;
    float timeBetween_Back;
    float restBack;

    [Header("Behaviour Parameters")]
    public bool activeOnPlay;
    public float startDelay;
    private float startDelayTimer;
    private bool onStartDelayEnded;

    SoundSpeaker pistonSpeaker;
    [SerializeField] SoundInfo hitSound;
    [SerializeField] SoundInfo airReleaseSound;

    bool inActive, goingDown, isResting, goingUp; 

    // Start is called before the first frame update
    void Start()
    {
        pistonSpeaker = GetComponent<SoundSpeaker>();

        restBack = restTime;
        timeBetween_Back = timeBetween;
        inActive = activeOnPlay;

        startDelayTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            ActivatePiston();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            DeactivatePiston();
        }*/

        if (Time.time >= startDelayTimer + startDelay)
        {
            PistonAnimation();
        }
    }

    public void PistonAnimation()
    {
        if (inActive)
        {
            timeBetween_Back -= 1 * Time.deltaTime;

            if (timeBetween_Back <= 0)
            {
                goingDown = true;
                inActive = false;
                timeBetween_Back = timeBetween;
                PlayAirSound(); //Sonido de aire que se suelta en el comienzo de la bajada
            }
        }


        if (goingDown)
        {
            inActive = false;
            
            if (piston_1.localPosition.z > -1f)
            {
                piston_1.position -= transform.forward * downSpeed * Time.deltaTime;
            }else if(piston_2.localPosition.z > -1f)
            {
                piston_1.localPosition = new Vector3(0, 0, -1f);
                piston_2.position -= transform.forward * downSpeed * Time.deltaTime;
            }
            else if (piston_3.localPosition.z > -0.96f)
            {
                piston_2.localPosition = new Vector3(0, 0, -1f);
                piston_3.position -= transform.forward * downSpeed * Time.deltaTime;
            }
            else if (piston_4.localPosition.z > -1f)
            {
                piston_3.localPosition = new Vector3(0, 0, -0.96f);
                piston_4.position -= transform.forward * downSpeed * Time.deltaTime;
            }
            else
            {
                //Aqui es cuando el pistón toca el suelo. Todo comportamiento de generación de sonidos se deberia hacer aquí

                piston_4.localPosition = new Vector3(0, 0, -1f);
                goingDown = false;
                isResting = true;
                PlayHitSound();
            }
        }
        if (isResting)
        {
            restBack -= 1 * Time.deltaTime;
        }
        if(restBack <= 0 && isResting)
        {
            restBack = restTime;
            isResting = false;
            goingUp = true;
            PlayAirSound();   // Sonido de aire que se suelta en el comienzo de la subida
        }

        if(goingUp)
        {
            
            if(piston_4.localPosition.z < 0)
            {
                piston_4.position += transform.forward * upSpeed * Time.deltaTime;
            }else if(piston_3.localPosition.z < 0)
            {
                piston_4.localPosition = new Vector3(0, 0, 0);
                piston_3.position += transform.forward * upSpeed * Time.deltaTime;
            }
            else if (piston_2.localPosition.z < 0)
            {
                piston_3.localPosition = new Vector3(0, 0, 0);
                piston_2.position += transform.forward * upSpeed * Time.deltaTime;
            }
            else if (piston_1.localPosition.z < 0)
            {
                piston_2.localPosition = new Vector3(0, 0, 0);
                piston_1.position += transform.forward * upSpeed * Time.deltaTime;
            }
            else
            {
                piston_1.localPosition = new Vector3(0, 0, 0f);
                goingUp = false;
                inActive = true;
            }
        }
    }

    public void ActivatePiston()
    {
        goingDown = true;
    }
    public void DeactivatePiston()
    {
        goingDown = false;
        inActive = false;
        isResting = false;
        goingUp = false;
    }

    public void PlayHitSound()
    {
        pistonSpeaker.CreateSoundBubble(hitSound, null, gameObject, false);
    }

    public void PlayAirSound()
    {
        pistonSpeaker.CreateSoundBubble(airReleaseSound, null, gameObject, true);
    }
}
