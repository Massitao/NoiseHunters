using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundLooper : MonoBehaviour
{
    AudioSource myAudio;
    public float playeDelay;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!myAudio.isPlaying)
        {
            myAudio.PlayDelayed(playeDelay);
        }
    }
}
