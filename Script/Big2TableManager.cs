using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableManager : SubjectTable
{
    public static Big2TableManager Instance;
    [ShowInInspector]
    public HandType TableHandType { get; private set; }
    [ShowInInspector]
    public HandRank TableHandRank { get; private set; }
    [ShowInInspector]
    public List<CardModel> TableCards { get; private set; }

    public event Action<CardInfo> OnTableUpdated;

    private void Awake()
    {
        if (Instance == null)
        {  
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        ParameterInitialization();

    }

    private void ParameterInitialization()
    {
        this.TableHandType = HandType.None;
        TableHandRank = HandRank.None;
        TableCards = new List<CardModel>();
    }

    public CardInfo TableLookUp()
    {
        CardInfo tableInfo = new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        tableInfo = new CardInfo(TableHandType, TableHandRank, TableCards);
        return tableInfo;
    }

    public void UpdateTableCards(CardInfo cardInfo)
    {
        TableHandType = cardInfo.HandType;
        TableHandRank = cardInfo.HandRank;
        TableCards.Clear();
        TableCards.AddRange(cardInfo.CardComposition);
        OnTableUpdated?.Invoke(cardInfo);
    }

}
