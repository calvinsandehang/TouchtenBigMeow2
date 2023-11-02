using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    private MenuBase _defaultMenu;

    [SerializeField]
    private MenuBase _chooseAvatarMenu;

    public MenuBase ChooseAvatarMenu { get; private set; }

    private Stack<MenuBase> menuStack = new Stack<MenuBase>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        OpenMenu(_defaultMenu);
    }

    public void OpenMenu(MenuBase menu)
    {
        // If there's a menu already open, close it
        if (menuStack.Count > 0)
        {
            menuStack.Peek().Close();
        }

        // Open the new menu and push it onto the stack
        menu.Open();
        menuStack.Push(menu);
    }

    public void CloseCurrentMenu()
    {
        if (menuStack.Count > 0)
        {
            MenuBase currentMenu = menuStack.Pop();
            currentMenu.Close();

            // If there's a previous menu, reopen it
            if (menuStack.Count > 0)
            {
                menuStack.Peek().Open();
            }
        }
    }

    public void OpenMenu_Default()
    {
        CloseCurrentMenu();
        OpenMenu(_defaultMenu);
    }

    public void OpenMenu_ChooseAvatar() 
    {
        CloseCurrentMenu();
        OpenMenu(_chooseAvatarMenu);
    }

    // This method can be expanded for back functionality
    public void GoBack()
    {
        CloseCurrentMenu();
    }
}
