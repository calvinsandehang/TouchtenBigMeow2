using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectPlayer : MonoBehaviour
{
    // a collection of all the observers of this subject
    private List<IObserverPlayerHand> observers = new List<IObserverPlayerHand>();
    public void AddObserver(IObserverPlayerHand observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserverPlayerHand observer)
    {
        observers.Remove(observer);
    }
    protected void NotifyObserver(List<CardModel> cardModels)
    {
        observers.ForEach((observer) => {
            observer.OnNotify(cardModels);
        });
    }
}
