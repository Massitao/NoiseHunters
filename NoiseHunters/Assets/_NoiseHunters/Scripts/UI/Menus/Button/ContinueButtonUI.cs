using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonUI : ButtonUI
{
    [HideInInspector] public bool blocked;

    protected void Start()
    {
        if (!LevelList.IsCurrentScenePlayable(SaveInstance._Instance.currentLoadedSave.lastPlayedScene))
        {
            blocked = true;
            button.interactable = false;

            Button continueButton = GetComponent<Button>();
            ColorBlock continueButtonColorBlock = continueButton.colors;
            continueButtonColorBlock.disabledColor = new Color(continueButtonColorBlock.disabledColor.r, continueButtonColorBlock.disabledColor.g, continueButtonColorBlock.disabledColor.b, 1f);
            continueButton.colors = continueButtonColorBlock;
        }
        else
        {
            Button continueButton = GetComponent<Button>();
            ColorBlock continueButtonColorBlock = continueButton.colors;
            continueButtonColorBlock.disabledColor = new Color(continueButtonColorBlock.disabledColor.r, continueButtonColorBlock.disabledColor.g, continueButtonColorBlock.disabledColor.b, 0f);
            continueButton.colors = continueButtonColorBlock;
        }
    }

    public override void OnSubmit()
    {
        if (!blocked)
        {
            base.OnSubmit();
        }
    }

    public override void OnSelect()
    {
        if (!blocked)
        {
            base.OnSelect();
        }
    }

    public override void SetCurrentSelectedGameObject()
    {
        if (!blocked)
        {
            base.SetCurrentSelectedGameObject();
        }
    }
}