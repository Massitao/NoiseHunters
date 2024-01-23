using UnityEngine;

public class LMLink : MonoBehaviour
{
    [SerializeField] private SceneToProgress sceneToChange;
    [SerializeField] private string customSceneToLoad;


    public void ChangeLevel()
    {
        if (sceneToChange != SceneToProgress.Custom)
        {
            string sceneToLoad = LevelList.ReturnSceneString(sceneToChange.ToString());

            _LevelManager._Instance.LevelChange(sceneToLoad, false);
        }
        else
        {
            _LevelManager._Instance.LevelChange(customSceneToLoad, false);
        }
    }
}