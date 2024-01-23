using UnityEngine;

public class LogHandle : MonoBehaviour
{    
    public void Log (string msgToLog)
    {
        Debug.Log(msgToLog);
    }

    public static void StaticLog (string msgToLog)
    {
        Debug.Log(msgToLog);
    }
}
