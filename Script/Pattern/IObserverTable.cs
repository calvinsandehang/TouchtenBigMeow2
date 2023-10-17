using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public interface IObserverTable
{
    public void OnNotifyTableState(TableState cardState, HandRank tableRank);

    public void OnNotifyAssigningCard(List<CardModel> cardModels);

    public void AddSelfToSubjectList();

    public void RemoveSelfToSubjectList();
}
