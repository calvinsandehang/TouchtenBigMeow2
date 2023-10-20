using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[RequireComponent(typeof(Button))]
public class InjectableButton : MonoBehaviour
{
    [SerializeField] ButtonType _buttonType;

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
