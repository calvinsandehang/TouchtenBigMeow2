using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

[Serializable]
public class PairCardTemplate
{
    public Image CardImage1;
    public Image CardImage2;
}

[Serializable]
public class ThreeOfAKindCardTemplate
{
    public Image CardImage1;
    public Image CardImage2;
    public Image CardImage3;
}

[Serializable]
public class FiveCardTemplate
{
    public Image CardImage1;
    public Image CardImage2;
    public Image CardImage3;
    public Image CardImage4;
    public Image CardImage5;
}

public class UIBig2TableCards : MonoBehaviour, IObserverTable, ISubscriber
{
    [SerializeField]
    private GameObject _singleTable;

    [SerializeField]
    private GameObject _pairTable;

    [SerializeField]
    private GameObject _threeOfKindTable;

    [SerializeField]
    private GameObject _fiveTable;


    [SerializeField]
    private Image[] _singleTableImages = new Image[4];

    [SerializeField]
    private PairCardTemplate[] _pairCardTemplate = new PairCardTemplate[4];

    [SerializeField]
    private ThreeOfAKindCardTemplate[] _threeOfKindCardTemplate = new ThreeOfAKindCardTemplate[4];

    [SerializeField]
    private FiveCardTemplate[] _fiveCardTemplate = new FiveCardTemplate[4];


    private CardModel[] singleTableCardModel = new CardModel[4];
    private List<CardModel> pairTableCardModel = new List<CardModel>();

    private List<CardModel> submittedTableCardModel = new List<CardModel>();

    Big2TableManager tableManager;
    HandType currentTableState;
    private Big2TableManager big2TableManager;

    private void Start()
    {
        big2TableManager = Big2TableManager.Instance;
        //big2TableManager.OnTableUpdated += OnNotifyAssigningCard;

        AddSelfToSubjectList();
        InitializeImageTemplate();
        SubscribeEvent();

        for (int i = 0; i < 4; i++)
        {
            singleTableCardModel[i] = null; // fill the array with null
        }

        // temporary
        currentTableState = HandType.None;
    }

    private void InitializeImageTemplate()
    {
        _singleTable.SetActive(true);
        _pairTable.SetActive(true);
        _threeOfKindTable.SetActive(true);
        _fiveTable.SetActive(true);

        ResetImageTemplate();
    }

    private void ResetImageTemplate() 
    {
        // Set alpha to 0 for single table images
        for (int i = 0; i < _singleTableImages.Length; i++)
        {
            Color imageColor = _singleTableImages[i].color;
            imageColor.a = 0f;
            _singleTableImages[i].color = imageColor;
        }

        // Set alpha to 0 for pair table images
        for (int i = 0; i < _pairCardTemplate.Length; i++)
        {
            PairCardTemplate pairTemplate = _pairCardTemplate[i];
            if (pairTemplate != null)
            {
                Color image1Color = pairTemplate.CardImage1.color;
                Color image2Color = pairTemplate.CardImage2.color;
                image1Color.a = 0f;
                image2Color.a = 0f;
                pairTemplate.CardImage1.color = image1Color;
                pairTemplate.CardImage2.color = image2Color;
            }
        }

        // Set alpha to 0 for Three of a Kind table images
        for (int i = 0; i < _threeOfKindCardTemplate.Length; i++)
        {
            ThreeOfAKindCardTemplate threeOfKindTemplate = _threeOfKindCardTemplate[i];
            if (threeOfKindTemplate != null)
            {
                Color image1Color = threeOfKindTemplate.CardImage1.color;
                Color image2Color = threeOfKindTemplate.CardImage2.color;
                Color image3Color = threeOfKindTemplate.CardImage3.color;
                image1Color.a = 0f;
                image2Color.a = 0f;
                image3Color.a = 0f;
                threeOfKindTemplate.CardImage1.color = image1Color;
                threeOfKindTemplate.CardImage2.color = image2Color;
                threeOfKindTemplate.CardImage3.color = image3Color;
            }
        }

        // Set alpha to 0 for Five Card table images
        for (int i = 0; i < _fiveCardTemplate.Length; i++)
        {
            FiveCardTemplate fiveCardTemplate = _fiveCardTemplate[i];
            if (fiveCardTemplate != null)
            {
                Color image1Color = fiveCardTemplate.CardImage1.color;
                Color image2Color = fiveCardTemplate.CardImage2.color;
                Color image3Color = fiveCardTemplate.CardImage3.color;
                Color image4Color = fiveCardTemplate.CardImage4.color;
                Color image5Color = fiveCardTemplate.CardImage5.color;
                image1Color.a = 0f;
                image2Color.a = 0f;
                image3Color.a = 0f;
                image4Color.a = 0f;
                image5Color.a = 0f;
                fiveCardTemplate.CardImage1.color = image1Color;
                fiveCardTemplate.CardImage2.color = image2Color;
                fiveCardTemplate.CardImage3.color = image3Color;
                fiveCardTemplate.CardImage4.color = image4Color;
                fiveCardTemplate.CardImage5.color = image5Color;
            }
        }
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
        //Debug.Log("OnNotifyAssigningCard");
        currentTableState = cardInfo.HandType;

        switch (currentTableState)
        {
            case HandType.None:
                EnableCertainTableType(null);
                break;
            case HandType.Single:
                EnableCertainTableType(_singleTable);
                HandleAssigningForSingleCard(cardInfo.CardComposition);
                break;
            case HandType.Pair:
                EnableCertainTableType(_pairTable);
                HandleAssigningCardForPair(cardInfo.CardComposition);
                break;
            case HandType.ThreeOfAKind:
                EnableCertainTableType(_threeOfKindTable);
                HandleAssigningForThreeOfKindCard(cardInfo.CardComposition);
                break;
            case HandType.FiveCards:
                EnableCertainTableType(_fiveTable);
                HandleAssigningForFiveCard(cardInfo.CardComposition);
                break;
        }
    }   

    private void EnableCertainTableType(GameObject enabledGameObject)
    {
        _singleTable.SetActive(false);
        _pairTable.SetActive(false);
        _threeOfKindTable.SetActive(false);
        _fiveTable.SetActive(false);

        if (enabledGameObject!=null)        
            enabledGameObject.SetActive(true);
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();
    }

    public void OnNotifyTableState(HandType cardState, HandRank tableRank)
    {
       
    }
    #endregion

    #region Assigning Card
    private void HandleAssigningForSingleCard(List<CardModel> cardModels)
    {
        submittedTableCardModel.AddRange(cardModels);

        // Trim the cardModels list to a maximum of 4 elements
        if (submittedTableCardModel.Count > 4)
        {
            submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 4);
        }

        for (int i = 0; i < _singleTableImages.Length; i++)
        {
            if (i < submittedTableCardModel.Count)
            {
                singleTableCardModel[i] = submittedTableCardModel[i];
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

    private void HandleAssigningCardForPair(List<CardModel> cardModels)
    {
        // list that store the submitted cards
        submittedTableCardModel.AddRange(cardModels);

        // limit the list, remove the first component when the count is more than 8
        if (submittedTableCardModel.Count > 8)
        {
            submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 8);
        }

        int j = 0;

        for (int i = 0; i < submittedTableCardModel.Count; i+=2)
        {
            Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
            Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;

            //Debug.Log("j : " + j);

            _pairCardTemplate[j].CardImage1.sprite = cardSprite1;
            _pairCardTemplate[j].CardImage2.sprite = cardSprite2;

            Color imageColor1 = _pairCardTemplate[j].CardImage1.color;
            imageColor1.a = 1f;
            _pairCardTemplate[j].CardImage1.color = imageColor1;

            Color imageColor2 = _pairCardTemplate[j].CardImage2.color;
            imageColor2.a = 1f;
            _pairCardTemplate[j].CardImage2.color = imageColor2;

            j++;
        }
    }

    private void HandleAssigningForThreeOfKindCard(List<CardModel> cardModels)
    {
        // Store the submitted cards for Three of a Kind
        submittedTableCardModel.AddRange(cardModels);

        // Limit the list to a maximum of 12 elements
        if (submittedTableCardModel.Count > 12)
        {
            submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 12);
        }

        int j = 0;

        for (int i = 0; i < submittedTableCardModel.Count; i += 3)
        {
            // Get the card sprites for Three of a Kind
            Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
            Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;
            Sprite cardSprite3 = submittedTableCardModel[i + 2].CardSprite;

            // Assign the card sprites to the Three of a Kind template
            _threeOfKindCardTemplate[j].CardImage1.sprite = cardSprite1;
            _threeOfKindCardTemplate[j].CardImage2.sprite = cardSprite2;
            _threeOfKindCardTemplate[j].CardImage3.sprite = cardSprite3;

            // Set the alpha values for the card images to make them visible
            Color imageColor1 = _threeOfKindCardTemplate[j].CardImage1.color;
            Color imageColor2 = _threeOfKindCardTemplate[j].CardImage2.color;
            Color imageColor3 = _threeOfKindCardTemplate[j].CardImage3.color;
            imageColor1.a = 1f;
            imageColor2.a = 1f;
            imageColor3.a = 1f;
            _threeOfKindCardTemplate[j].CardImage1.color = imageColor1;
            _threeOfKindCardTemplate[j].CardImage2.color = imageColor2;
            _threeOfKindCardTemplate[j].CardImage3.color = imageColor3;

            j++;
        }
    }

    private void HandleAssigningForFiveCard(List<CardModel> cardModels)
    {
        // Store the submitted cards for Five Card
        submittedTableCardModel.AddRange(cardModels);

        // Limit the list to a maximum of 20 elements
        if (submittedTableCardModel.Count > 20)
        {
            submittedTableCardModel.RemoveRange(0, submittedTableCardModel.Count - 20);
        }

        int j = 0;

        for (int i = 0; i < submittedTableCardModel.Count; i += 5)
        {
            // Get the card sprites for Five Card
            Sprite cardSprite1 = submittedTableCardModel[i].CardSprite;
            Sprite cardSprite2 = submittedTableCardModel[i + 1].CardSprite;
            Sprite cardSprite3 = submittedTableCardModel[i + 2].CardSprite;
            Sprite cardSprite4 = submittedTableCardModel[i + 3].CardSprite;
            Sprite cardSprite5 = submittedTableCardModel[i + 4].CardSprite;

            // Assign the card sprites to the Five Card template
            _fiveCardTemplate[j].CardImage1.sprite = cardSprite1;
            _fiveCardTemplate[j].CardImage2.sprite = cardSprite2;
            _fiveCardTemplate[j].CardImage3.sprite = cardSprite3;
            _fiveCardTemplate[j].CardImage4.sprite = cardSprite4;
            _fiveCardTemplate[j].CardImage5.sprite = cardSprite5;

            // Set the alpha values for the card images to make them visible
            Color imageColor1 = _fiveCardTemplate[j].CardImage1.color;
            Color imageColor2 = _fiveCardTemplate[j].CardImage2.color;
            Color imageColor3 = _fiveCardTemplate[j].CardImage3.color;
            Color imageColor4 = _fiveCardTemplate[j].CardImage4.color;
            Color imageColor5 = _fiveCardTemplate[j].CardImage5.color;
            imageColor1.a = 1f;
            imageColor2.a = 1f;
            imageColor3.a = 1f;
            imageColor4.a = 1f;
            imageColor5.a = 1f;
            _fiveCardTemplate[j].CardImage1.color = imageColor1;
            _fiveCardTemplate[j].CardImage2.color = imageColor2;
            _fiveCardTemplate[j].CardImage3.color = imageColor3;
            _fiveCardTemplate[j].CardImage4.color = imageColor4;
            _fiveCardTemplate[j].CardImage5.color = imageColor5;

            j++;
        }
    }

    private PairCardTemplate FindAvailablePairTemplate(List<PairCardTemplate> assignedTemplates)
    {
        // Iterate through the pair templates and find the first available template
        foreach (var template in _pairCardTemplate)
        {
            if (!assignedTemplates.Contains(template))
            {
                return template;
            }
        }

        // If all templates are assigned, return null
        return null;
    }


    private void HidePairImages(PairCardTemplate pairTemplate)
    {
        if (pairTemplate != null)
        {
            pairTemplate.CardImage1.sprite = null;
            pairTemplate.CardImage2.sprite = null;
            Color image1Color = pairTemplate.CardImage1.color;
            Color image2Color = pairTemplate.CardImage2.color;
            image1Color.a = 0f;
            image2Color.a = 0f;
            pairTemplate.CardImage1.color = image1Color;
            pairTemplate.CardImage2.color = image2Color;
        }
    }



    #endregion

    public void SubscribeEvent()
    {
        Big2GlobalEvent.SubscribeGameHasEnded(ClearTableUI);
        Big2GlobalEvent.SubscribeRoundHasEnded(ClearTableUI);
    }

    private void ClearTableUI()
    {
        submittedTableCardModel.Clear();
        ResetImageTemplate();
    }

    public void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeGameHasEnded(ClearTableUI);
        Big2GlobalEvent.UnsubscribeRoundHasEnded(ClearTableUI);
    }
}
