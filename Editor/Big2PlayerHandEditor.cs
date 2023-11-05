

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Big2Meow.DeckNCard; // Assuming CardModel is in this namespace
using Big2Meow.Gameplay;
using Big2Meow.Player;

[CustomEditor(typeof(Big2PlayerHand))]
public class Big2PlayerHandEditor : Editor
{
    private const int CardsPerRow = 5; // Number of card images per row
    private const float CardImageWidth = 70; // Width of the card image
    private const float CardImageHeight = 100; // Height of the card image

    public override void OnInspectorGUI()
    {
        Big2PlayerHand playerHand = (Big2PlayerHand)target;

        // Draw the default inspector options
        DrawDefaultInspector();

        // Now draw the player cards in a custom layout
        List<CardModel> playerCards = playerHand.GetPlayerCardsForEditor(); // Ensure this method exists

        // Ensure the list is not null
        if (playerCards == null)
            return;

        // Start custom layout
        GUILayout.Label("Player Cards:", EditorStyles.boldLabel);

        // Start a new vertical group for this player hand
        GUILayout.BeginVertical("box");

        DisplayCardGrid(playerCards);

        // End the vertical group for this player hand
        GUILayout.EndVertical();

        // Apply changes and repaint the inspector if needed
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void DisplayCardGrid(List<CardModel> cards)
    {
        int cardsInRow = 0;
        GUILayout.BeginHorizontal();
        foreach (CardModel card in cards)
        {
            if (card.CardSprite != null)
            {
                Rect spriteRect = card.CardSprite.rect;
                Texture2D spriteTexture = card.CardSprite.texture;
                Rect texCoords = new Rect(spriteRect.x / spriteTexture.width, spriteRect.y / spriteTexture.height, spriteRect.width / spriteTexture.width, spriteRect.height / spriteTexture.height);
                Vector2 fullSize = new Vector2(spriteRect.width, spriteRect.height);
                Vector2 size = new Vector2(CardImageWidth, CardImageHeight);

                // Draw the sprite with the correct aspect ratio
                GUILayout.Box(GUIContent.none, GUILayout.Width(size.x), GUILayout.Height(size.y));

                Rect lastRect = GUILayoutUtility.GetLastRect();
                GUI.DrawTextureWithTexCoords(lastRect, spriteTexture, texCoords, true);

                cardsInRow++;
                if (cardsInRow >= CardsPerRow)
                {
                    // End the current row and start a new one if we've reached the limit
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    cardsInRow = 0;
                }
            }
            else
            {
                // Display a placeholder or a message if the card sprite is missing
                GUILayout.Box("No Image", GUILayout.Width(CardImageWidth), GUILayout.Height(CardImageHeight));
            }
        }
        if (cardsInRow != 0) // Ensure the horizontal group is closed if it's not a full row
        {
            GUILayout.EndHorizontal();
        }
    }

}
#endif

