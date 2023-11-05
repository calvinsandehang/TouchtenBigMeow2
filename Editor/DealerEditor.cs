

#if UNITY_EDITOR
using Big2Meow.FSM;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Big2GMStateMachine))]
public class DealerEditor : Editor
{
    /*
    private const int CardsPerRow = 5; // Number of card images per row
    private const float CardImageWidth = 70; // Width of the card image
    private const float CardImageHeight = 100; // Height of the card image

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        Big2GMStateMachine dealer = (Big2GMStateMachine)target;

        // Add a button to deal the cards
        if (dealer.IsInitialized && GUILayout.Button("Deal Cards")) // Check if dealer is initialized
        {
            dealer.DealCards();
        }

        // Check if the hands have been dealt and the dealer is initialized
        if (dealer.IsInitialized && dealer.PlayerHands != null) // Check if dealer is initialized
        {
            for (int i = 0; i < dealer.PlayerHands.Count; i++)
            {
                EditorGUILayout.LabelField($"Player {i + 1}'s Hand:", EditorStyles.boldLabel);

                List<CardModel> cards = dealer.PlayerHands[i].GetPlayerCards();
                DisplayCardGrid(cards);
            }
        }

        // Display the dealer's remaining deck if the dealer is initialized
        if (dealer.IsInitialized) // Check if dealer is initialized
        {
            EditorGUILayout.LabelField("Dealer's Remaining Deck:", EditorStyles.boldLabel);
            List<CardModel> remainingDeck = dealer.GetRemainingDeck(); // Retrieve the remaining deck from the dealer
            if (remainingDeck != null)
            {
                DisplayCardGrid(remainingDeck); // Display the remaining cards
            }
            else
            {
                EditorGUILayout.LabelField("No remaining cards in the dealer's deck.");
            }
        }
        else
        {
            // If the dealer isn't initialized, display a warning message
            EditorGUILayout.HelpBox("The dealer is not initialized. Please enter Play Mode to initialize the dealer.", MessageType.Warning);
        }

        // Add some spacing after
        EditorGUILayout.Space();
    }


    private void DisplayCardGrid(List<CardModel> cards)
    {
        int cardsInRow = 0;
        GUILayout.BeginHorizontal();
        foreach (CardModel card in cards)
        {
            if (card.CardSprite != null)
            {
                // Show the card sprite
                GUILayout.Box(card.CardSprite.texture, GUILayout.Width(CardImageWidth), GUILayout.Height(CardImageHeight));

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
                EditorGUILayout.LabelField("Card sprite is missing!");
            }
        }
        GUILayout.EndHorizontal();
    }
    */
}

#endif


