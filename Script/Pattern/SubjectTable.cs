using Big2Meow.DeckNCard;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

/// <summary>
/// An abstract base class for subjects in the observer pattern related to tables.
/// </summary>
public abstract class SubjectTable : MonoBehaviour
{
    // A collection of all the observers of this subject.
    private List<IObserverTable> observers = new List<IObserverTable>();

    /// <summary>
    /// Adds an observer to the list of observers.
    /// </summary>
    /// <param name="observer">The observer to add.</param>
    public void AddObserver(IObserverTable observer)
    {
        observers.Add(observer);
    }

    /// <summary>
    /// Removes an observer from the list of observers.
    /// </summary>
    /// <param name="observer">The observer to remove.</param>
    public void RemoveObserver(IObserverTable observer)
    {
        observers.Remove(observer);
    }

    /// <summary>
    /// Notifies all observers about assigning a card.
    /// </summary>
    /// <param name="cardInfo">The information about the assigned card.</param>
    protected void NotifyObserverAssigningCard(CardInfo cardInfo)
    {
        observers.ForEach((observer) =>
        {
            observer.OnNotifyAssigningCard(cardInfo);
        });
    }

    /// <summary>
    /// Notifies all observers about the table state.
    /// </summary>
    /// <param name="tableHandType">The hand type of the table.</param>
    /// <param name="tableRank">The rank of the table.</param>
    protected void NotifyTableState(HandType tableHandType, HandRank tableRank)
    {
        observers.ForEach((observer) =>
        {
            observer.OnNotifyTableState(tableHandType, tableRank);
        });
    }
}
