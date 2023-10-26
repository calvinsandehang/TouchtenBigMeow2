using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

public abstract class SubjectTable : MonoBehaviour
{
    // a collection of all the observers of this subject
    private List<IObserverTable> observers = new List<IObserverTable>();
    public void AddObserver(IObserverTable observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserverTable observer)
    {
        observers.Remove(observer);
    } 
    protected void NotifyObserverAssigningCard(CardInfo cardInfo) 
    {
        observers.ForEach((observer) => {
            observer.OnNotifyAssigningCard(cardInfo);
        });
    }

    protected void NotifyTableState(HandType tableHandType, HandRank tableRank)
    {
        observers.ForEach((observer) => {
            observer.OnNotifyTableState(tableHandType, tableRank);
        });
    }
}
