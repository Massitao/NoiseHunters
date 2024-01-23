using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Menu Manager")]
    [HideInInspector] public MenuManager manager;

    [Header("Menu Components")]
    protected Canvas canvas;
    [HideInInspector] public CanvasGroup canvasGroup;

    [Header("Default Button")]
    public GameObject defaultElement;
    public GameObject lastSelectedElement;
    protected List<ButtonUI> allButtons;

    [Header("Cancel Sound")]
    public AudioClip cancelSound;

    [Header("Can Leave Menu")]
    [HideInInspector] public bool canLeaveMenu = true;


    protected virtual void Awake()
    {
        GetMenuComponents();
    }
    protected virtual void GetMenuComponents()
    {
        manager = FindObjectOfType<MenuManager>();

        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        allButtons = new List<ButtonUI>(GetComponentsInChildren<ButtonUI>());

        lastSelectedElement = defaultElement;
    }

    protected virtual void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
    }



    protected virtual void SetManager(MenuManager managerToSet)
    {
        manager = managerToSet;
    }

    public virtual void AccessMenu()
    {
        manager.AccessMenu(this);
    }
    public virtual void ExitMenu()
    {
        manager.ExitCurrentMenu();
    }

    public virtual void InputExitMenu()
    {
        if (cancelSound != null)
        {
            SoundDictionary.CreateOnlyAudioSource(this, cancelSound, transform.position);
        }

        ExitMenu();
    }

    public virtual void ButtonActive(bool active)
    {
        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].GetThisButton().interactable = active;
        }
    }
}