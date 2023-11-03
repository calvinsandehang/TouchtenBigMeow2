using UnityEngine;

/// <summary>
/// Represents the state of a Big2 player when they are losing the game.
/// </summary>
public class Big2PlayerStateLosing : BaseState<PlayerState>
{
    private Big2PlayerStateMachine PSM;

    /// <summary>
    /// Initializes a new instance of the Big2PlayerStateLosing class with the specified state key and state machine.
    /// </summary>
    /// <param name="key">The key that represents the state.</param>
    /// <param name="stateMachine">The state machine managing the player's states.</param>
    public Big2PlayerStateLosing(PlayerState key, Big2PlayerStateMachine stateMachine) : base(key)
    {
        PSM = stateMachine;
    }

    /// <summary>
    /// Called when entering the losing state.
    /// </summary>
    public override void EnterState()
    {
        int playerID = PSM.PlayerHand.PlayerID;
        Debug.Log("Player " + playerID + " is in Losing state");

        // Broadcast the event to indicate that the player is losing
        PSM.BroadcastPlayerIsLosing();
    }

    /// <summary>
    /// Called when exiting the losing state.
    /// </summary>
    public override void ExitState()
    {
        // No specific actions needed when exiting the losing state
    }

    /// <summary>
    /// Gets the currently active state.
    /// </summary>
    /// <returns>The currently active state key (Losing).</returns>
    public override PlayerState GetActiveState()
    {
        return PlayerState.Losing;
    }

    /// <summary>
    /// Called to update the losing state logic (not used in this state).
    /// </summary>
    public override void UpdateState()
    {
        // No specific logic needed for the losing state
    }
}
