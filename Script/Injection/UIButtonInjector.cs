using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[DefaultExecutionOrder(-9999)]
public class UIButtonInjector : MonoBehaviour
{
    public static UIButtonInjector Instance;


    [ShowInInspector]
    public Dictionary<ButtonType, Button> InjectedButtons { get; private set; } = new Dictionary<ButtonType, Button>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public Button GetButton(ButtonType buttonType)
    {
        if (InjectedButtons.ContainsKey(buttonType))
        {
            return InjectedButtons[buttonType];
        }
        else
        {
            Debug.LogWarning($"Button of type {buttonType} does not exist.");
            return null;
        }
    }
}
