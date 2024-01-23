using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFP : MonoBehaviour
{
    public MissionManagerFP missionManager;

    public enum States { kq, cd, es };
    public States toUse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            switch (toUse)
            {
                case States.kq:
                    missionManager.KillQuestEnabled();
                    break;

                case States.es:
                    missionManager.EscapeComplete();
                    break;
            }
        }
    }
}
