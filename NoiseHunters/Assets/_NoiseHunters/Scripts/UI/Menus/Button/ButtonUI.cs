using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [Header("Button Sounds")]
    [SerializeField] protected AudioClip selectSound;
    [SerializeField] protected AudioClip submitSound;

    [Header("Inside Menu")]
    protected Menu insideMenu;

    [Header("Button")]
    protected Selectable button;


    protected virtual void Awake()
    {
        insideMenu = GetComponentInParent<Menu>();
        button = GetComponent<Selectable>();
    }

    public virtual Selectable GetThisButton()
    {
        return button;
    }


    public virtual void SetCurrentSelectedGameObject()
    {
        if (button.interactable)
        {
            insideMenu.manager.SetSelectedGameObject(button.gameObject);
        }
    }

    public virtual void OnSelect()
    {
        if (selectSound != null)
        {
            insideMenu.lastSelectedElement = gameObject;
            SoundDictionary.CreateOnlyAudioSource(this, selectSound, transform.position);
        }
    }

    public virtual void OnSubmit()
    {
        if (submitSound != null)
        {
            SoundDictionary.CreateOnlyAudioSource(this, submitSound, transform.position);
        }
    }
}