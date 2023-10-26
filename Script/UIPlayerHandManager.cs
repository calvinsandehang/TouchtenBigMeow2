using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

public class PlayerCard 
{
    public int PlayerID;
    public List<GameObject> CardsObjectsInPlayerHand = new List<GameObject>();
    public List<CardModel> CardModelsInPlayerHand = new List<CardModel>();
}

[DefaultExecutionOrder(-9999)]
public class UIPlayerHandManager : MonoBehaviour
{
    public static UIPlayerHandManager Instance;

    [SerializeField]
    private DeckSO deckSO;

    [SerializeField]
    private Transform cardParent;

    [SerializeField]
    private List<GameObject> _playerCardsParent;

    [SerializeField]
    private CardPool cardPool;

   private List<PlayerCard> PlayerCards = new List<PlayerCard>();
   
    private SortCriteria currentSortCriteria;

    #region MonoBehaviour
    private void OnDisable()
    {

    }
    private void Awake()
    {
        if (Instance == null)   
            Instance = this;
        else
            Destroy(Instance);

        PlayerCardInitialization();
        ParameterInitialization();
    }

    private void PlayerCardInitialization()
    {
        for (int i = 0; i < 4; i++)        
            PlayerCards.Add(new PlayerCard());        
    }

    private void Start()
    {
    }
    #endregion
   
    public void DisplayCards(List<CardModel> cards, int playerID, PlayerType playerType)
    {
        Debug.Log($"Display Card, Player {playerID}");

        PlayerCards[playerID].CardModelsInPlayerHand.Clear();
        PlayerCards[playerID].CardModelsInPlayerHand = cards.ToList();

        // Clear out the old cards.
        while (PlayerCards[playerID].CardsObjectsInPlayerHand.Count > 0)
        {
            var card = PlayerCards[playerID].CardsObjectsInPlayerHand[0];
            PlayerCards[playerID].CardsObjectsInPlayerHand.RemoveAt(0);
            cardPool.ReturnCard(card);
        }

        // Generate the new cards from the pool
        foreach (var cardModel in PlayerCards[playerID].CardModelsInPlayerHand)
        {
            GameObject cardGO = cardPool.GetCard();
            cardGO.transform.SetParent(_playerCardsParent[playerID].transform, false);
            cardGO.transform.localRotation = Quaternion.identity;  // Reset rotation to 0,0,0
            UISelectableCard selectableCard = cardGO.GetComponent<UISelectableCard>();
            selectableCard.Initialize(cardModel, playerType);  // Adjust this if necessary to match CardModel structure.
            PlayerCards[playerID].CardsObjectsInPlayerHand.Add(cardGO);
        }

        Debug.Log(_playerCardsParent[playerID].transform);

        SortPlayerHand(currentSortCriteria, playerID, playerType);
    }    

    public void SortPlayerHand(SortCriteria criteria, int playerID, PlayerType playerType)
    {
        Big2CardSorter cardSorter = new Big2CardSorter();

        switch (criteria)
        {
            case SortCriteria.Rank:
                currentSortCriteria = SortCriteria.Rank;
                cardSorter.SortPlayerHandByRank(PlayerCards[playerID].CardsObjectsInPlayerHand, playerType);
                break;
            case SortCriteria.Suit:
                currentSortCriteria = SortCriteria.Suit;
                cardSorter.SortPlayerHandBySuit(PlayerCards[playerID].CardsObjectsInPlayerHand, playerType);
                break;
            case SortCriteria.BestHand:
                currentSortCriteria = SortCriteria.BestHand;
                cardSorter.SortPlayerHandByBestHand(PlayerCards[playerID].CardsObjectsInPlayerHand, cardPool, _playerCardsParent[playerID].transform, playerType);
                break;
        }
    }

    #region Observer Pattern
    /*
    public void InitialializedPlayerHand(Big2PlayerHand playerHand) 
    {
        this.playerHand = playerHand;
        InitializeObserverPattern();
    }

    public void OnNotify(List<CardModel> cardModels, int index)
    {
        //DisplayCards(cardModels, index);
    }

    public void AddSelfToSubjectList()
    {
       
        playerHand.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        if (playerHand == null)
            return;

        playerHand.RemoveObserver(this);
    }
    #endregion

    #region Helper
    private void InitializeObserverPattern()
    {
        AddSelfToSubjectList();
    }
    */
    private void ParameterInitialization()
    {
        currentSortCriteria = SortCriteria.Rank;
    }
    #endregion






    /*
public void SortPlayerHandByRank()
{
  cardsObjectsInPlayerHand.Sort((card1, card2) =>
  {
      UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
      UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

      CardModel cardModel1 = selectableCard1.GetCardModel();
      CardModel cardModel2 = selectableCard2.GetCardModel();

      return cardModel1.CardRank.CompareTo(cardModel2.CardRank);
  });
  UpdateCardPositions();
}

public void SortPlayerHandBySuit()
{
  cardsObjectsInPlayerHand.Sort((card1, card2) =>
  {
      UISelectableCard selectableCard1 = card1.GetComponent<UISelectableCard>();
      UISelectableCard selectableCard2 = card2.GetComponent<UISelectableCard>();

      CardModel cardModel1 = selectableCard1.GetCardModel();
      CardModel cardModel2 = selectableCard2.GetCardModel();

      return cardModel1.CardSuit.CompareTo(cardModel2.CardSuit);
  });
  UpdateCardPositions();
}


public void SortPlayerHandByBestHand()
{
  Big2PokerHands pokerHandsChecker = new Big2PokerHands();
  List<CardModel> tempCardModels = new List<CardModel>(cardModelsInPlayerHand);
  List<CardModel> sortedCardModels = new List<CardModel>();

  while (tempCardModels.Count > 0)
  {
      // Get the best hand from the remaining cards
      Tuple<HandRank, List<CardModel>, int> bestHand = pokerHandsChecker.GetBestHand(tempCardModels);
      List<CardModel> bestHandCards = bestHand.Item2;

      // Log for debugging
      Debug.Log("Best hand rank: " + bestHand.Item1.ToString());
      foreach (var card in bestHandCards)
      {
          Debug.Log("Card in best hand: " + card.CardRank.ToString() + " of " + card.CardSuit.ToString());
      }

      // Add the best hand cards to the sorted list
      sortedCardModels.AddRange(bestHandCards);

      // Remove the best hand cards from tempCardModels for the next iteration
      foreach (var card in bestHandCards)
      {
          tempCardModels.Remove(card);
      }
  }

  // Update the player's hand with the sorted cards and reflect this in the UI
  cardModelsInPlayerHand = new List<CardModel>(sortedCardModels); // Replace the list, don't clear and re-add
  cardModelsInPlayerHand.Reverse();

  // Clear out the old cards.
  while (cardsObjectsInPlayerHand.Count > 0)
  {
      var card = cardsObjectsInPlayerHand[0];
      cardsObjectsInPlayerHand.RemoveAt(0);
      cardPool.ReturnCard(card);
  }

  for (int i = 0; i < cardModelsInPlayerHand.Count; i++)
  {
      GameObject cardGO = cardPool.GetCard();
      cardGO.transform.SetParent(cardParent, false);
      cardGO.transform.SetSiblingIndex(i);
      UISelectableCard selectableCard = cardGO.GetComponent<UISelectableCard>();
      selectableCard.Initialize(cardModelsInPlayerHand[i]); // Adjust this if necessary to match CardModel structure.
      cardsObjectsInPlayerHand.Add(cardGO);
  }
}   


private void UpdateCardPositions()
{
   for (int i = 0; i < cardsObjectsInPlayerHand.Count; i++)
   {
       // This is a placeholder for whatever logic you use to position your cards in the scene.
       // You may set their position, change their order in the hierarchy, etc.
       cardsObjectsInPlayerHand[i].transform.SetSiblingIndex(i);
   }
}
*/

}
