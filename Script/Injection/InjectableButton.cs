using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

namespace Big2Meow.Injection
{
    /// <summary>
    /// Represents a button that can be injected into the UIButtonInjector for centralized access.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class InjectableButton : MonoBehaviour
    {
        /// <summary>
        /// The type of this button, used for identification in the injector.
        /// </summary>
        [SerializeField] private ButtonType _buttonType;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// Here it attempts to inject this button into the UIButtonInjector.
        /// </summary>
        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                UIButtonInjector.Instance.InjectButton(_buttonType, button);
            }
            else
            {
                Debug.LogError("No Button component found on this GameObject.");
            }
        }
    }
}
