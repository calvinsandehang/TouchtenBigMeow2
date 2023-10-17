using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public abstract class SubjectCardEvaluator : MonoBehaviour
{
    // a collection of all the observers of this subject
    private List<IObserverCardEvaluator> observers = new List<IObserverCardEvaluator>();
    public void AddObserver(IObserverCardEvaluator observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserverCardEvaluator observer)
    {
        observers.Remove(observer);
    }
    protected void NotifyObserverAboutEvaluatedCard(List<CardModel> selectedCard)
    {
        observers.ForEach((observer) => {
            observer.OnNotifySelectedCards(selectedCard);
        });
    }   
}
