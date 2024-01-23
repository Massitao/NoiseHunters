using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{
    [Header("Manager Components")]
    public EventSystem eventSystem;

    [Header("Menu Stack")]
    public Menu primaryMenu;
    public Stack<Menu> menuStack = new Stack<Menu>();

    [Header("Current Button")]
    [SerializeField] protected GameObject _currentSelectedGO;
    public GameObject currentSelectedGO
    {
        get { return _currentSelectedGO; }
        set
        {
            if (value != _currentSelectedGO && value != null)
            {
                _currentSelectedGO = value;
                SetSelectedGameObject(_currentSelectedGO);
            }
        }
    }


    protected virtual void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void AccessMenu(Menu menuToAccess)
    {
        if (menuStack.Count != 0)
        {
            Menu currentMenu = menuStack.Peek();
            currentMenu.ButtonActive(false);
        }

        menuStack.Push(menuToAccess);
        menuStack.Peek().ButtonActive(true);

        DeselectGameObject();
        SetSelectedGameObject(menuToAccess.defaultElement);
    }
    public void ExitCurrentMenu()
    {
        Menu currentMenu = menuStack.Peek();

        if (currentMenu.canLeaveMenu)
        {
            currentMenu.ButtonActive(false);
            menuStack.Pop();
            DeselectGameObject();

            if (menuStack.Count != 0)
            {
                currentMenu = menuStack.Peek();
                currentMenu.ButtonActive(true);
                SetSelectedGameObject(currentMenu.lastSelectedElement);
            }
        }
    }

    public void SetSelectedGameObject(GameObject objectToSet)
    {
        eventSystem.SetSelectedGameObject(objectToSet);
    }
    public void DeselectGameObject()
    {
        eventSystem.SetSelectedGameObject(null);
    }
}