using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public interface IObserverTable
{
    public void OnNotifyTableState(HandType cardState, HandRank tableRank);

    public void OnNotifyAssigningCard(CardInfo cardInfo);

    public void AddSelfToSubjectList();

    public void RemoveSelfToSubjectList();
}
