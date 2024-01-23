using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CountDownQuest : MonoBehaviour
{
    public bool ended;

    [SerializeField] Text countdownText;

    public bool isCountingDown;
    public float countdownTime;
    [SerializeField] float countdownTimer;

    [SerializeField] bool succeded;

    [SerializeField] UnityEvent onSucceed;
    [SerializeField] UnityEvent onFail;

    [SerializeField] UnityEvent onActivate;

    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = countdownTime;
    }

    // Update is called once per frame
    void Update()
    {    
        if (isCountingDown)
        {
            if (countdownTimer <= 0)
            {
                TimerEnd();
            }
            else
            {
                countdownText.text = countdownTimer.ToString("000.00") + " S";
                countdownTimer -= Time.deltaTime;
            }
        }
    }

    public void CountDownSelector()
    {
        if (!ended)
        {
            if (!isCountingDown)
            {
                CountDownStarted();
            }
            else
            {
                CountDownStopped();
            }
        }
    }

    void CountDownStarted()
    {
        isCountingDown = true;
        onActivate.Invoke();
    }

    void CountDownStopped()
    {
        isCountingDown = false;
        succeded = true;

        TimerEnd();
    }

    public void TimerEnd()
    {
        if (succeded)
        {
            onSucceed.Invoke();
        }
        else
        {
            onFail.Invoke();
        }

        ended = true;
    }
}
