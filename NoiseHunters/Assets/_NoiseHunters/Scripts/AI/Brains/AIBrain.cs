using UnityEngine;

// AI Brain is a base for every variant brain that allows for high customization and offers generic tools for the AIs. Recommended methods will be left as comments.
public class AIBrain : MonoBehaviour
{
    #region Entity Detection Values
    [HideInInspector] public ThirdPersonController PlayerPosition;
    [HideInInspector] public Vector3 detectedTarget;
    #endregion


    /* Awake call is best used to get ONLY local components from this very same GameObject.
    protected virtual void Awake() {}
    */

    /* AI_GetComponents method will be used to get the neccesary components of the AI. Should and Must be called in Awake().
    protected virtual void AI_GetComponents() {}
    */


    /* Start call is usually used to set local gotten components variables, get and set external GameObject or Components references, or run code once.
    protected virtual void Start() {}
    */

    /* OnEnable is called when this Component is first loaded or enabled. It's in between Awake() and Start(). Mostly used to suscribe to Events.
    protected virtual void OnEnable() {}
    */


    /* OnDisable is called when this component is disabled. It's the latest call in the Unity Game Loop before OnDestroy(). Mostly used to unsuscribe from Events.
    protected virtual void OnDisable() {}
    */


    #region AI TOOLS: Inherited methods for every AI
    // Looks at Target in every Rotation
    public Quaternion AI_LookAt_SameRotation(Vector3 forwardPos)
    {
        Quaternion posRotation = Quaternion.LookRotation(forwardPos, Vector3.up);
        return posRotation;
    }
    // Only looks at Target in the XZ axis
    public Quaternion AI_LookAt_TargetXZ(Vector3 targetPos, Vector3 originPos)
    {
        Vector3 direction = (targetPos - originPos).normalized;
        Quaternion targetLook = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z), Vector3.up);
        return targetLook;
    }

    // Chooses a random number from provided values (either int, float, or Vector2)
    public int      AI_ChooseRandomNumber(int minNumber, int maxNumber)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        return Random.Range(minNumber, maxNumber);
    }
    public float    AI_ChooseRandomNumber(float minNumber, float maxNumber)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        return Random.Range(minNumber, maxNumber);
    }
    public float    AI_ChooseRandomNumber(Vector2 numbers)
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        return Random.Range(numbers.x, numbers.y);
    }
    #endregion
}
