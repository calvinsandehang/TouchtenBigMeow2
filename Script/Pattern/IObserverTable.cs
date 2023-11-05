using Big2Meow.DeckNCard;
using UnityEngine;
using static GlobalDefine;

/// <summary>
/// Interface for objects that observe table state changes.
/// </summary>
public interface IObserverTable
{
    /// <summary>
    /// Notifies observers of changes in the table state.
    /// </summary>
    /// <param name="cardState">The hand type of the table.</param>
    /// <param name="tableRank">The rank of the table.</param>
    void OnNotifyTableState(HandType cardState, HandRank tableRank);

    /// <summary>
    /// Notifies observers when cards are being assigned.
    /// </summary>
    /// <param name="cardInfo">Information about the assigned cards.</param>
    void OnNotifyAssigningCard(CardInfo cardInfo);

    /// <summary>
    /// Adds the observer to the subject's list.
    /// </summary>
    void AddSelfToSubjectList();

    /// <summary>
    /// Removes the observer from the subject's list.
    /// </summary>
    void RemoveSelfFromSubjectList();
}
