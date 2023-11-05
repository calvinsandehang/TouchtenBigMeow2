using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Big2Meow.Utility
{
    /// <summary>
    /// A GameObject that persists across scene loads.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class PersistentGameObject : MonoBehaviour
    {
        private void Awake()
        {
            // Prevent this GameObject from being destroyed when loading new scenes.
            DontDestroyOnLoad(gameObject);
        }
    }
}


