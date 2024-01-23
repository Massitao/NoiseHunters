using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateButtonsQuest : MonoBehaviour
{
    public List<ButtonProp> buttonsToActivate;

    public bool activationComplete;

    [SerializeField] UnityEvent onQuestComplete;


    // Update is called once per frame
    void Update()
    {
        if (AllActivated() && !activationComplete)
        {
            activationComplete = true;
            onQuestComplete.Invoke();
        }
    }

    bool AllActivated()
    {
        for (int i = 0; i < buttonsToActivate.Count; i++)
        {
            if (buttonsToActivate[i].IsActive && !buttonsToActivate[i].IsOn)
            {
                return false;
            }
        }
        return true;
    }
}