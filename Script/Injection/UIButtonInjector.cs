using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.Injection
{
    /// <summary>
    /// Responsible for injecting buttons into a central repository for easy access throughout the game.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class UIButtonInjector : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of UIButtonInjector.
        /// </summary>
        public static UIButtonInjector Instance;

        /// <summary>
        /// Gets the dictionary containing all the injected buttons.
        /// </summary>
        [ShowInInspector]
        public Dictionary<ButtonType, Button> InjectedButtons { get; private set; } = new Dictionary<ButtonType, Button>();

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Here it initializes the singleton instance.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Injects a button into the repository if it doesn't exist, or replaces it if it does.
        /// </summary>
        /// <param name="buttonType">The type of the button to inject.</param>
        /// <param name="button">The button to inject.</param>
        public void InjectButton(ButtonType buttonType, Button button)
        {
            if (!InjectedButtons.ContainsKey(buttonType))
            {
                InjectedButtons.Add(buttonType, button);
            }
            else
            {
                Debug.LogWarning($"A button of type {buttonType} already exists. Overwriting.");
                InjectedButtons[buttonType] = button;
            }
        }

        /// <summary>
        /// Retrieves a button of a specified type from the repository.
        /// </summary>
        /// <param name="buttonType">The type of button to retrieve.</param>
        /// <returns>The Button if found; otherwise, null.</returns>
        public Button GetButton(ButtonType buttonType)
        {
            if (InjectedButtons.TryGetValue(buttonType, out Button button))
            {
                return button;
            }
            else
            {
                Debug.LogWarning($"Button of type {buttonType} does not exist.");
                return null;
            }
        }
    }
}
