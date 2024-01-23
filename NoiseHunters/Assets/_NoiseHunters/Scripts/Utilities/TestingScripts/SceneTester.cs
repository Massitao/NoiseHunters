using UnityEngine;
using UnityEngine.InputSystem;

public class SceneTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pageUpKey.isPressed)
        {
            ResetScene();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Application.Quit();
        }
    }

    public void ResetScene()
    {
        _LevelManager._Instance?.ResetScene();
    }
}
