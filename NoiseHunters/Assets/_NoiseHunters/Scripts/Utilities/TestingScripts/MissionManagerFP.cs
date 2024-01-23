using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManagerFP : MonoBehaviour
{
    [SerializeField] _LevelManager lm;

    [SerializeField] Text killQuest;
    [SerializeField] Text Countdown;
    [SerializeField] Text escape;

    public bool kq, kqTrue, cd, cdTrue, es, esTrue;

    // Update is called once per frame
    void Update()
    {
        if (kq)
        {
            killQuest.enabled = true;

            if (kqTrue) killQuest.color = Color.green;
        }

        if (cd)
        {
            Countdown.enabled = true;

            if (cdTrue) Countdown.color = Color.green;
        }

        if (es)
        {
            escape.enabled = true;

            if (esTrue) escape.color = Color.green;
        }
    }

    public void KillQuestEnabled()
    {
        kq = true;
    }

    public void KillQuestComplete()
    {
        kqTrue = true;
        cd = true;
    }

    public void CountdownComplete()
    {
        cdTrue = true;
        es = true;
    }

    public void EscapeComplete()
    {
        esTrue = true;
        lm.ResetScene();
    }
}
