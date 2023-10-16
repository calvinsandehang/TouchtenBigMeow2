using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "PlayingCard/Deck")]
public class DeckSO : ScriptableObject
{
    [SerializeField]
    private List<CardSO> _cards = new List<CardSO>();

    public List<CardSO> Cards => _cards;
}
