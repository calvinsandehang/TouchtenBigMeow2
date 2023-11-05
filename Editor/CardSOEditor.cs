

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Big2Meow.DeckNCard;

[CustomEditor(typeof(CardSO))]
public class CardSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        CardSO script = (CardSO)target;

        // If there's a sprite, draw it
        if (script.CardSprite != null)
        {
            GUILayout.Label("Card Preview:");
            var texture = AssetPreview.GetAssetPreview(script.CardSprite);
            if (texture != null)
            {
                GUILayout.Label(texture, GUILayout.Width(100), GUILayout.Height(150));
            }
        }
    }
}

#endif

