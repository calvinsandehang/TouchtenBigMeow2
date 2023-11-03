using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public class Big2TableManager : SubjectTable, ISubscriber
{
    public static Big2TableManager Instance;
    [ShowInInspector]
    public HandType TableHandType { get; private set; }
    [ShowInInspector]
    public HandRank TableHandRank { get; private set; }
    [ShowInInspector]
    public List<CardModel> TableCards { get; private set; }

    //public event Action<CardInfo> OnTableUpdated;

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
        TableHandType = HandType.None;
        TableHandRank = HandRank.None;
        TableCards = new List<CardModel>();

        SubscribeEvent();
    }

    public CardInfo TableLookUp()
    {
        CardInfo tableInfo = new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        tableInfo = new CardInfo(TableHandType, TableHandRank, TableCards);
        return tableInfo;
    }

    public void UpdateTableCards(CardInfo cardInfo)
    {
        Debug.Log("UpdateTableCards");
        TableHandType = cardInfo.HandType;
        TableHandRank = cardInfo.HandRank;
        TableCards.Clear();
        TableCards.AddRange(cardInfo.CardComposition);
        NotifyObserverAssigningCard(cardInfo);
        NotifyTableState(TableHandType, TableHandRank);
        //OnTableUpdated?.Invoke(cardInfo);
    }

    private void CleanTable() 
    {
        Debug.Log("CleanTable");
        TableHandType = HandType.None;
        TableHandRank = HandRank.None;
        TableCards = new List<CardModel>();

        CardInfo tableInfo = new CardInfo(HandType.None, HandRank.None, new List<CardModel>());
        NotifyObserverAssigningCard(tableInfo);
        NotifyTableState(TableHandType, TableHandRank);
    }

    public void SubscribeEvent()
    {
        Big2GlobalEvent.SubscribeRoundHasEnded(CleanTable);
        Big2GlobalEvent.SubscribeGameHasEnded(CleanTable);       
    }

    public void UnsubscribeEvent()
    {
        Big2GlobalEvent.UnsubscribeRoundHasEnded(CleanTable);
        Big2GlobalEvent.UnsubscribeGameHasEnded(CleanTable);
    }
}
