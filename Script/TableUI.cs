using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

public class TableUI : MonoBehaviour, IObserverTable
{
    [SerializeField]
    private GameObject _singleTable;


    [SerializeField]
    private Image[] _singleTableImages = new Image[4];

    private CardModel[] singleTableCardModel = new CardModel[4];

    Big2TableManager tableManager;
    HandType currentTableState;
    private Big2TableManager big2TableManager;

    private void Start()
    {
        big2TableManager = Big2TableManager.Instance;
        big2TableManager.OnTableUpdated += OnNotifyAssigningCard;

        AddSelfToSubjectList();

        for (int i = 0; i < _singleTableImages.Length; i++)
        {
            Color imageColor = _singleTableImages[i].color;
            imageColor.a = 0f; 
            _singleTableImages[i].color = imageColor;
        }

        for (int i = 0; i < 4; i++)
        {
            singleTableCardModel[i] = null; // fill the array with null
        }

        // temporary
        currentTableState = HandType.Single;
    }

    #region Table Observer
    public void AddSelfToSubjectList()
    {
        tableManager = Big2TableManager.Instance;
        tableManager.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        tableManager.RemoveObserver(this);
    }


    public void OnNotifyAssigningCard(CardInfo cardInfo)
    {
        switch (currentTableState)
        {
            case HandType.None:
                break;
            case HandType.Single:

                HandleAssigningForSingleCard(cardInfo.CardComposition);
                break;
        }
    }   

    private void HandleAssigningForSingleCard(List<CardModel> cardModels)
    {
        // Trim the cardModels list to a maximum of 4 elements
        if (cardModels.Count > 4)
        {
            cardModels.RemoveRange(0, cardModels.Count - 4);
        }

        CardModel[] tempCardModelArray = cardModels.ToArray();

        for (int i = 0; i < _singleTableImages.Length; i++)
        {
            if (i < tempCardModelArray.Length)
            {
                singleTableCardModel[i] = tempCardModelArray[i];
            }
            else
            {
                singleTableCardModel[i] = null;
            }

            // Update the sprites for the table images
            if (singleTableCardModel[i] == null) continue; // skip assigning sprite if still null

            Sprite cardSprite = singleTableCardModel[i].CardSprite;
            _singleTableImages[i].sprite = cardSprite;

            Color imageColor = _singleTableImages[i].color;
            imageColor.a = 1f;
            _singleTableImages[i].color = imageColor;
        }
    }




    public void OnNotifyTableState(HandType cardState)
    {
        switch (cardState) 
        {
            case HandType.None:
                currentTableState = HandType.None;
                _singleTable.SetActive(false);
                break;
            case HandType.Single:
                _singleTable.SetActive(true);
                currentTableState = HandType.Single;
                break;
        }
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();
    }

    public void OnNotifyTableState(HandType cardState, HandRank tableRank)
    {
       
    }
    #endregion


}
