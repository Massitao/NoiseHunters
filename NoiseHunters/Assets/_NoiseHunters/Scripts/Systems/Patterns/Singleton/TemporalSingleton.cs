using UnityEngine;

public class TemporalSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();

            if (Instance == null)
            {
                GameObject newInstance = new GameObject();
                newInstance.AddComponent<T>();
                newInstance.name = typeof(T).ToString();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
