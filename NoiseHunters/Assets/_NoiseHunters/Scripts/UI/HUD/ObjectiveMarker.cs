using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarker : MonoBehaviour
{
    [Header("Marker Components")]
    public Image markerImage;
    private Animator animator;

    [Header("Marker Animator Values")]
    [SerializeField] private string markerAnimator_TriggerRemoveMarker;

    [Header("Debug Stuff")]
    public Objective givenObjective;
    public int objectiveIndex;


    private void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void SetColor(Color colorToSet)
    {
        float alpha = markerImage.color.a;

        markerImage.color = new Color(colorToSet.r, colorToSet.g, colorToSet.b, alpha);
    }

    public void RemoveMarker()
    {
        animator.SetTrigger(markerAnimator_TriggerRemoveMarker);
    }

    public void DestroyMarker()
    {
        Destroy(gameObject);
    }
}
