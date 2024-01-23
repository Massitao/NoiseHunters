using UnityEngine;

public class PauseUser : MonoBehaviour
{
    [Header("Pause GameObject")]
    [SerializeField] private GameObject pauseMenuGO;
    private PauseMenuManager pauseManager;


    private void Awake()
    {
        if (pauseMenuGO != null)
        {
            pauseManager = Instantiate(pauseMenuGO, null).GetComponentInChildren<PauseMenuManager>();
        }
    }

    public void DoPause()
    {
        pauseManager.PauseGame();
    }
}