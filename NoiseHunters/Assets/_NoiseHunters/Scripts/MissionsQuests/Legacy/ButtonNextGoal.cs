using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNextGoal : MonoBehaviour
{
    public MissionManager mManager;

    public void NextIntermediateGoal()
    {
        mManager.NextIntermediateGoal();
        Destroy(this);
    }
}
