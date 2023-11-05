

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerHandEvaluator))]
public class PlayerHandEvaluatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerHandEvaluator evaluator = (PlayerHandEvaluator)target;

        GUILayout.Space(20);

        GUILayout.Label("Best Hand:", EditorStyles.boldLabel);

        foreach (var bestHand in evaluator.GetRankedHands())
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Hand Rank: " + bestHand.Item1.ToString(), GUILayout.Width(150));

            foreach (var card in bestHand.Item2)
            {
                if (card.CardSprite != null)
                {
                    GUILayout.Box(card.CardSprite.texture, GUILayout.Width(60), GUILayout.Height(80));
                }
                else
                {
                    GUILayout.Label(card.ToString());
                }
            }

            GUILayout.Label("Points: " + bestHand.Item3.ToString(), GUILayout.Width(80));
            GUILayout.EndHorizontal();
        }
    }
}
#endif


