using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.MainMenu
{
    /// <summary>
    /// Manages the game's menu system, including opening, closing, and navigating menus.
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the singleton instance of the MenuManager.
        /// </summary>
        public static MenuManager Instance { get; private set; }

        [Header("Menus")]
        [SerializeField] private MenuBase defaultMenu;
        [SerializeField] private MenuBase chooseAvatarMenu;

        private Stack<MenuBase> menuStack = new Stack<MenuBase>();

        private void Awake()
        {
            // Singleton pattern: Ensure there is only one instance of MenuManager.
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                // If an instance already exists, destroy this GameObject.
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Open the default menu when the game starts.
            OpenMenu(defaultMenu);
        }

        /// <summary>
        /// Opens a menu and pushes it onto the menu stack.
        /// </summary>
        /// <param name="menu">The menu to open.</param>
        public void OpenMenu(MenuBase menu)
        {
            // If there's a menu already open, close it.
            if (menuStack.Count > 0)
            {
                menuStack.Peek()?.Close();
            }

            // Open the new menu and push it onto the stack.
            menu?.Open();
            menuStack.Push(menu);
        }

        /// <summary>
        /// Closes the current menu and, if applicable, reopens the previous menu.
        /// </summary>
        public void CloseCurrentMenu()
        {
            if (menuStack.Count > 0)
            {
                MenuBase currentMenu = menuStack.Pop();
                currentMenu?.Close();

                // If there's a previous menu, reopen it.
                if (menuStack.Count > 0)
                {
                    menuStack.Peek()?.Open();
                }
            }
        }

        /// <summary>
        /// Opens the default menu.
        /// </summary>
        public void OpenDefaultMenu()
        {
            CloseCurrentMenu();
            OpenMenu(defaultMenu);
        }

        /// <summary>
        /// Opens the "Choose Avatar" menu.
        /// </summary>
        public void OpenChooseAvatarMenu()
        {
            CloseCurrentMenu();
            OpenMenu(chooseAvatarMenu);
        }

        // This method can be expanded for additional functionality
        /// <summary>
        /// Handles the "Go Back" functionality.
        /// </summary>
        public void GoBack()
        {
            CloseCurrentMenu();
        }
    }
}


