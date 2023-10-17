using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubjectCard : MonoBehaviour
{
    // a collection of all the observers of this subject
    private List<IObserverCard> observers = new List<IObserverCard>();
    public void AddObserver(IObserverCard observer) 
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IObserverCard observer) 
    {
        observers.Remove(observer);
    }
    protected void NotifyObserver(CardState state)
    {
        observers.ForEach((observer) =>{
            observer.OnNotify(state);
        });
    }
}
