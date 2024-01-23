using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    static T instance;
    public static T _Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (_Instance == null)
        {
            instance = FindObjectOfType<T>();

            if (_Instance == null)
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

        DontDestroyOnLoad(gameObject);
    }
}
