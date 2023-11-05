using Big2Meow.DeckNCard;
using Big2Meow.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab; 

    [SerializeField]
    private DeckSO deckSO;

    [SerializeField]
    private Transform cardParent; 

    // Start is called before the first frame update
    void Start()
    {
        DisplayDeck();
    }

    // This method instantiates a cardPrefab for each card in the DeckSO
    // and uses the CardDisplay's DisplayCard method to set the card visuals
    private void DisplayDeck()
    {
        List<CardSO> cards = deckSO.Cards;
        for (int i = 0; i < cards.Count; i++)
        {
            // Instantiate a new card prefab and set its parent
            GameObject cardGO = Instantiate(cardPrefab, cardParent);

            // Get the CardDisplay component and call its DisplayCard method
            UISelectableCard cardDisplay = cardGO.GetComponent<UISelectableCard>();
            cardDisplay.DisplayCard(cards[i]);
        }
    }
}
