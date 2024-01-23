using UnityEngine;
using UnityEngine.UI;

public class UsernameGetter : MonoBehaviour
{
    private Text usernameText;

    private void Awake()
    {
        usernameText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SteamManager._Instance != null)
        {
            usernameText.text = SteamManager._Instance.GetClientUsername();
        }   
    }
}
