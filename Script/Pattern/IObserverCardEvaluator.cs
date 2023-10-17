using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public interface IObserverCardEvaluator 
{
    public void OnNotifySelectedCards(List<CardModel> selectedCard);

    public void AddSelfToSubjectList();

    public void RemoveSelfToSubjectList();
}
