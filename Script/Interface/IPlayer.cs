using Big2Meow.DeckNCard;
using Big2Meow.Gameplay;
using Big2Meow.Player;
using System.Collections.Generic;
using static GlobalDefine;

/// <summary>
/// Represents a player in the Big2 card game.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Gets or sets the type of the player.
    /// </summary>
    PlayerType PlayerType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the player.
    /// </summary>
    int PlayerID { get; set; }

    /// <summary>
    /// Initializes parameters related to the player.
    /// </summary>
    void ParameterInitialization(); 

    /// <summary>
    /// Initializes the unique identifier of the player.
    /// </summary>
    /// <param name="index">The index to assign as the player's identifier.</param>
    void InitializePlayerID(int index);

    /// <summary>
    /// Checks if the player has the Three of Diamonds card.
    /// </summary>
    /// <returns>True if the player has the Three of Diamonds, otherwise false.</returns>
    bool CheckHavingThreeOfDiamonds();

    /// <summary>
    /// Checks if the player has multiple cards of the same rank (a pair, three of a kind, etc.).
    /// </summary>
    /// <returns>True if the player has multiple cards of the same rank, otherwise false.</returns>
    bool CheckHavingQuadrupleTwoCard();

    /// <summary>
    /// Retrieves the cards currently held by the player.
    /// </summary>
    /// <returns>A list of CardModel representing the player's cards.</returns>
    List<CardModel> GetPlayerCards();

    /// <summary>
    /// Adds a card to the player's hand.
    /// </summary>
    /// <param name="card">The CardModel to add to the player's hand.</param>
    void AddCard(CardModel card);

    /// <summary>
    /// Resets the player's hand by clearing all cards.
    /// </summary>
    /// <param name="playerHand">The Big2PlayerHand instance representing the player's hand.</param>
    void ResetPlayerCard(Big2PlayerHand playerHand);

    /// <summary>
    /// Removes specified cards from the player's hand.
    /// </summary>
    /// <param name="removedCards">A list of CardModel to be removed from the player's hand.</param>
    void RemoveCards(List<CardModel> removedCards);

    /// <summary>
    /// Checks if the number of cards in the player's hand is below six.
    /// </summary>
    void CheckCardBelowSix();

    /// <summary>
    /// Checks if the player has won by having no cards left in their hand.
    /// </summary>
    void CheckWinningCondition();

    /// <summary>
    /// Looks up and retrieves the type of the player.
    /// </summary>
    /// <returns>The type of the player (Human, AI, etc.).</returns>
    PlayerType PlayerTypeLookUp();
}
