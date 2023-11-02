#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// A utility class for toggling developer mode in the Unity Editor.
/// </summary>
public static class DeveloperModeToggle
{
    private const string MENU_PATH = "Developer/DevMode";

    /// <summary>
    /// Toggles the developer mode on or off and saves the setting.
    /// </summary>
    [MenuItem(MENU_PATH)]
    public static void ToggleDeveloperMode()
    {
        bool currentSetting = IsDeveloperModeEnabled();
        Menu.SetChecked(MENU_PATH, !currentSetting);
        EditorPrefs.SetBool("DeveloperMode", !currentSetting);
    }

    /// <summary>
    /// Validates the toggle developer mode menu item based on its current state.
    /// </summary>
    /// <returns>True to validate the menu item.</returns>
    [MenuItem(MENU_PATH, true)]
    public static bool ValidateToggleDeveloperMode()
    {
        Menu.SetChecked(MENU_PATH, IsDeveloperModeEnabled());
        return true;
    }

    /// <summary>
    /// Checks if developer mode is currently enabled.
    /// </summary>
    /// <returns>True if developer mode is enabled, otherwise false.</returns>
    public static bool IsDeveloperModeEnabled()
    {
        return EditorPrefs.GetBool("DeveloperMode", false);
    }
}
#endif
