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

    TableManager tableManager;
    TableState currentTableState;

    private void Start()
    {
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
        currentTableState = TableState.Single;
    }

    #region Table Observer
    public void AddSelfToSubjectList()
    {
        tableManager = TableManager.Instance;
        tableManager.AddObserver(this);
    }

    public void RemoveSelfToSubjectList()
    {
        tableManager.RemoveObserver(this);
    }

    public void OnNotifyAssigningCard(List<CardModel> cardModels)
    {
        switch (currentTableState)
        {
            case TableState.None:                               
                break;
            case TableState.Single:

                HandleAssigningForSingleCard(cardModels);
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




    public void OnNotifyTableState(TableState cardState)
    {
        switch (cardState) 
        {
            case TableState.None:
                currentTableState = TableState.None;
                _singleTable.SetActive(false);
                break;
            case TableState.Single:
                _singleTable.SetActive(true);
                currentTableState = TableState.Single;
                break;
        }
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();
    }

    public void OnNotifyTableState(TableState cardState, HandRank tableRank)
    {
       
    }
    #endregion


}
