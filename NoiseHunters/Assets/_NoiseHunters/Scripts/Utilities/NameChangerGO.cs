using System.Collections.Generic;
using UnityEngine;

public class NameChangerGO : MonoBehaviour
{
    [SerializeField] private string nameToSet;

    [ContextMenu("Set names for child GameObjects")]
    public void NameChanger()
    {
        if (nameToSet == string.Empty)
        {
            Debug.Log("No name was provided!");
            return;
        }          

        List<Transform> GOToChangeName = new List<Transform>(GetComponentsInChildren<Transform>());
        GOToChangeName.Remove(transform);

        for (int i = 0; i< GOToChangeName.Count; i++)
        {
            GOToChangeName[i].name = nameToSet;
        }
    }
}
