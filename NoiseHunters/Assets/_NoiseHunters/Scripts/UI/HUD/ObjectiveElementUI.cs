using UnityEngine;
using UnityEngine.UI;

public class ObjectiveElementUI : MonoBehaviour
{
    [Header("Objective Element Components")]
    [SerializeField] private Image objectiveImage;
    public Text objectiveText;

    private Animator animator;


    [Header("Objective Element Components")]
    [SerializeField] private string animatorProgressTrigger;
    [SerializeField] private string animatorExitTrigger;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        objectiveText.text = "";
    }


    public void SetImageColor(Color colorToSet)
    {
        objectiveImage.color = new Color(colorToSet.r, colorToSet.g, colorToSet.b, 1f);
    }

    public void SetTextContent(string textToSet)
    {
        objectiveText.text = textToSet; 
    }


    public void ObjectiveProgress()
    {
        animator.SetTrigger(animatorProgressTrigger);
    }

    public void ObjectiveCompleted()
    {
        animator.SetTrigger(animatorExitTrigger);
    }

    public void DestroyElement()
    {
        Destroy(gameObject);
    }
}