using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Enables specified GameObjects when in Unity Editor's developer mode.
/// </summary>
[DefaultExecutionOrder(-99999)]
public class EnableOnDevMode : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsToEnable;

    private void Awake()
    {
#if UNITY_EDITOR
        if (IsDeveloperModeEnabled())
        {
            // Enable specified GameObjects in developer mode.
            foreach (GameObject obj in gameObjectsToEnable)
            {
                obj.SetActive(true);
            }
        }
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// Checks if Unity Editor's developer mode is enabled.
    /// </summary>
    /// <returns>True if developer mode is enabled; otherwise, false.</returns>
    private bool IsDeveloperModeEnabled()
    {
        return EditorPrefs.GetBool("DeveloperMode", false);
    }
#endif
}
