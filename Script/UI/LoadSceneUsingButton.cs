using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneUsingButton : MonoBehaviour
{
    [SerializeField]
    private Button loadSceneButton;

    [SerializeField]
    private string sceneNameToLoad;

    private void Start()
    {
        // Add the listener to the button
        loadSceneButton.onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadNextScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogError("SceneLoader instance not found!");
        }
    }
}
