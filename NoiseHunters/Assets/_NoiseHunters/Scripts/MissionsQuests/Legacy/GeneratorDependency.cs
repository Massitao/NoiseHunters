using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneratorDependency : MonoBehaviour
{
    public List<GeneratorProp> generatorsDepending;

    public UnityEvent OnAllInteractuablesActive;
    public UnityEvent OnNotMatchedRequirements;


    private void OnEnable()
    {
        for (int i = 0; i < generatorsDepending.Count; i++)
        {
            generatorsDepending[i].OnActiveStateChange += CheckIfRequirementsMatched;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < generatorsDepending.Count; i++)
        {
            generatorsDepending[i].OnActiveStateChange -= CheckIfRequirementsMatched;
        }
    }


    private void Start()
    {
        CheckIfRequirementsMatched(null);
    }


    public void CheckIfRequirementsMatched(GeneratorProp gp)
    {
        bool matched = true;

        for (int i = 0; i < generatorsDepending.Count; i++)
        {
            if (!generatorsDepending[i].IsActive)
            {
                matched = false;
            }
        }

        if (matched)
        {
            OnAllInteractuablesActive?.Invoke();
        }
        else
        {
            OnNotMatchedRequirements?.Invoke();
        }
    }
}