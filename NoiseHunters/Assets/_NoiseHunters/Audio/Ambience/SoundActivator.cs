using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActivator : MonoBehaviour
{

    public GameObject[] newSound;
    public GameObject[] previousSound;

    public void ChangeAmbienceSound()
    {
        for(int i = 0; i < previousSound.Length; i++)
        {
            previousSound[i].SetActive(false);
        }
        for (int i = 0; i < newSound.Length; i++)
        {
            newSound[i].SetActive(true);
        }
    }
}
