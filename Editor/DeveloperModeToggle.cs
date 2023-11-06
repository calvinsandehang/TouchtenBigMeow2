#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

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
        ShowDeveloperModeWarning(!currentSetting);
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

    /// <summary>
    /// Displays a warning in the console if the developer mode is enabled.
    /// </summary>
    [DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (IsDeveloperModeEnabled())
        {
            ShowDeveloperModeWarning(true);
        }
    }

    /// <summary>
    /// Shows a warning in the console that developer mode is active.
    /// </summary>
    /// <param name="isActive">If set to true, show the developer mode warning.</param>
    private static void ShowDeveloperModeWarning(bool isActive)
    {
        if (isActive)
        {
            Debug.LogWarning("Developer Mode is active. Builds are disabled.");
        }
    }
}

/// <summary>
/// Prevents building when developer mode is enabled.
/// </summary>
public class DeveloperModeBuildPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (DeveloperModeToggle.IsDeveloperModeEnabled())
        {
            throw new BuildFailedException("Build failed because Developer Mode is active. Disable Developer Mode to build the project.");
        }
    }
}
#endif
