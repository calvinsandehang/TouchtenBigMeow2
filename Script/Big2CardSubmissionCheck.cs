using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[InfoBox("Bridge between Player and Table")]
public class Big2CardSubmissionCheck : MonoBehaviour
{
    private Big2TableManager big2TableManager;
    private CardInfo tableInfo;
    public HandType currentTableHandType;
    private HandRank currentTableHandRank;
    private List<CardModel> currentTableCards;

    private List<CardModel> submittedCards = new List<CardModel>();
    private CardInfo submittedCardInfo;
    private bool matchingHandType;
        
    public event Action AllowedToSubmitCard; // Subs : UIPlayerSubmissionButton
    public event Action NotAllowedToSubmitCard; // Subs : UIPlayerSubmissionButton

    private Big2PlayerHand playerHand;
    private PlayerType playerType;

    private Button submitCardButton;

    private Big2PlayerStateMachine playerStateMachine;
    private PlayerState playerState;

    private bool isAllowedToCheck = false;

    public static event Action OnPlayerFinishTurnGlobal; // Subs : Big2GMStateMachine
    public event Action OnPlayerFinishTurnLocal; // subs : Big2PlayerStateMachine
   

    private void Awake()
    {
        
    }

    private void Start()
    {
        InitializeParameters();
        InitializeBig2TableManager();
        //AddSelfToSubjectList();
    }

    private void InitializeBig2TableManager()
    {
        big2TableManager = Big2TableManager.Instance;
        Big2TableLookUp();
    }

    private void Big2TableLookUp()
    {
        tableInfo = big2TableManager.TableLookUp();
        currentTableHandType = tableInfo.HandType;
        currentTableHandRank = tableInfo.HandRank;
        currentTableCards = new List<CardModel>();
    }

    private void InitializeParameters()
    {
        playerStateMachine = GetComponent<Big2PlayerStateMachine>();

        playerHand = GetComponent<Big2PlayerHand>();
        playerType = playerHand.PlayerTypeLookUp();

        if (playerType == PlayerType.Human)
        {
            SetupSubmissionButton();
        }

        SubscribeEvent();

    }
    
    private void OnPlayerIsPlaying() 
    {
        isAllowedToCheck = true;
    }

    private void OnPlayerIsNotPlaying() 
    {
        isAllowedToCheck = false;
    }

    // Receive selected card from PlayerSelectedCardEvaluator
    public void SubmissionCheck(List<CardModel> selectedCard)
    {
        if (selectedCard.Count == 0)
        {
            //Debug.Log("0 card selected, return");
            NotAllowedToSubmitCard?.Invoke();
            return;
        }
        
        
        Big2TableLookUp(); 
        // Check if the hand type of the selected cards is allowed
        if (!CompareHandType(selectedCard) && currentTableHandType != HandType.None)
        {
            NotAllowedToSubmitCard?.Invoke();
            //Debug.Log("HandType mismatch");
            return;
        }

        ClearSubmittedCardList();

        // Evaluate the selected cards to determine their hand type and rank
        submittedCardInfo = EvaluateSelectedCards(selectedCard);

        // Check if the hand rank of the selected cards is allowed
        if (!CompareHandRank(submittedCardInfo.HandRank) || !CheckCardCount(submittedCardInfo))
        {
            NotAllowedToSubmitCard?.Invoke();
            //Debug.Log("Selected card hand rank is lower than the table card / not suitable");
            return;
        }

        // Compare the selected cards with the current table cards
        if (!CompareSelectedCardsWithTableCards(submittedCardInfo.CardComposition))
        {
            NotAllowedToSubmitCard?.Invoke();
            //Debug.Log("Selected cards value is lower than the table cards");
            return;
        }

       

        // If all checks pass, add the selected cards to the submitted cards
        AddNewSubmittedCardToSubmittedCardList();

        AllowedToSubmitCard?.Invoke();
    }

    private bool CheckCardCount(CardInfo cardInfo)
    {
        int cardCount = cardInfo.CardComposition.Count;

        switch (cardInfo.HandType)
        {
            case HandType.None:
                return false;
            case HandType.Single:
                return cardCount == 1;
            case HandType.Pair:
                return cardCount == 2;
            case HandType.ThreeOfAKind:
                return cardCount == 3;
            case HandType.FiveCards:
                return cardCount == 5;
            default:
                return false;
        }
    }


    private void AddNewSubmittedCardToSubmittedCardList()
    {
        submittedCards.AddRange(submittedCardInfo.CardComposition);
    }

    private void ClearSubmittedCardList()
    {
        submittedCards.Clear();
    }

    private CardInfo EvaluateSelectedCards(List<CardModel> selectedCard)
    {
        Big2PokerHands checkSelectedCard = new Big2PokerHands();
        var bestHand = checkSelectedCard.GetBestHand(selectedCard);
        var selectedCardHandType = bestHand.HandType;
        var selectedCardHandRank = bestHand.HandRank;
        var bestHandCards = bestHand.CardComposition;
        return new CardInfo (selectedCardHandType, selectedCardHandRank, bestHandCards);
    }

    private bool CompareSelectedCardsWithTableCards(List<CardModel> bestHandCards)
    {
        Big2CardComparer big2CardComparer = new Big2CardComparer();
        tableInfo = big2TableManager.TableLookUp();
        currentTableCards = tableInfo.CardComposition;

        return big2CardComparer.CompareHands(bestHandCards, currentTableCards);
    }  

    private bool CompareHandRank(HandRank selectedCardHandRank)
    {
        switch (currentTableHandRank)
        {
            case HandRank.None:
                return true;
            case HandRank.HighCard:
                // Allow any hand rank that is equal or higher
                if (selectedCardHandRank == HandRank.HighCard)
                    return true;
                else
                    return false;                
            case HandRank.Pair:
                // Allow any hand rank that is equal or higher, except HighCard
                return selectedCardHandRank == HandRank.Pair;
            case HandRank.ThreeOfAKind:
                // Allow any hand rank that is equal or higher, except HighCard and Pair
                return selectedCardHandRank == HandRank.ThreeOfAKind;
            case HandRank.Straight:
            case HandRank.Flush:
            case HandRank.FullHouse:
            case HandRank.FourOfAKind:
            case HandRank.StraightFlush:
                // Allow only the same or higher hand rank
                return selectedCardHandRank >= currentTableHandRank;
        }

        return false;
    }
    private bool CompareHandType(List<CardModel> submittedCardModels)
    {
        int cardCount = submittedCardModels.Count;

        switch (cardCount)
        {
            case 0:
                return currentTableHandType == HandType.None;
            case 1:
                return currentTableHandType == HandType.Single;
            case 2:
                return currentTableHandType == HandType.Pair;
            case 3:
                return currentTableHandType == HandType.ThreeOfAKind;
            case 5:
                return currentTableHandType == HandType.FiveCards;
            default:
                return false;
        }
    }

    
    #region Submission Button
    private void SetupSubmissionButton()
    {
        submitCardButton = UIButtonInjector.Instance.GetButton(ButtonType.SubmitCard);
        submitCardButton.onClick.AddListener(OnSubmitCard);

        UIPlayerSubmissionButton submitButtonBehaviour = submitCardButton.GetComponent<UIPlayerSubmissionButton>();
        submitButtonBehaviour.InitializeButton(this);
    }

    public void OnSubmitCard()
    {
        Debug.Log("OnSubmitCard");
        Big2TableManager.Instance.UpdateTableCards(submittedCardInfo);
        playerHand.RemoveCards(submittedCards);

        NotAllowedToSubmitCard?.Invoke();
        StartCoroutine(DelayedAction(EndTurn, 2f));        
    }

    private void EndTurn()
    {
        Debug.Log("player end turn");
        OnPlayerFinishTurnGlobal?.Invoke();
        OnPlayerFinishTurnLocal?.Invoke();
    }
 
    #endregion

    #region Event
    private void SubscribeEvent()
    {
        playerStateMachine.OnPlayerIsPlaying += OnPlayerIsPlaying;
        playerStateMachine.OnPlayerIsWaiting += OnPlayerIsNotPlaying;
    }

    private void UnsubscribeEvent()
    {
        playerStateMachine.OnPlayerIsPlaying -= OnPlayerIsPlaying;
        playerStateMachine.OnPlayerIsWaiting -= OnPlayerIsNotPlaying;
    }
    #endregion

    private void OnDestroy()
    {
        //RemoveSelfToSubjectList(); //testing
    }

    private void OnEnable()
    {
        //AddSelfToSubjectList(); //testing
    }

    private void OnDisable()
    {
        //RemoveSelfToSubjectList(); //testing
        UnsubscribeEvent();
    }

    private IEnumerator DelayedAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    #region Testing and debugging
    // Card Evaluator
    public void AddSelfToSubjectList()
    {
        // Assuming both TableManager and CardEvaluator have lists of observers
        //CardEvaluator.Instance?.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        //CardEvaluator.Instance?.RemoveObserver(this);
    }
    #endregion
}
