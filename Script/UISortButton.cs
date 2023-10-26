using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalDefine;

public class UISortButton : MonoBehaviour
{
    private const string bestHand = "Sort by Best Hand";
    private const string rank = "Sort by Rank";
    private const string suit = "Sort by Suit";

    private Button sortButton;

    [SerializeField]
    private TextMeshProUGUI _buttonText;

    private List<Action> methods = new List<Action>();

    private int currentIndex = -1;
    private int maxIndex = 3;

    // Start is called before the first frame update
    void Start()
    {
        sortButton = GetComponent<Button>();
        sortButton.onClick.AddListener(OnSortButtonPressed);

        methods.Add(SortByBestHand);
        methods.Add(SortByRank);
        methods.Add(SortBySuit);

        _buttonText.text = bestHand;
    }

    public void SortByBestHand() 
    {
        Debug.Log("SortByBestHand() ");
        // sort by best hand
        UIPlayerHandManager.Instance.SortPlayerHand(SortCriteria.BestHand, 0, PlayerType.Human);
        // change text
        _buttonText.text = rank;
    }

    public void SortByRank() 
    {
        Debug.Log("SortByRank() ");
        // sort by best rank
        UIPlayerHandManager.Instance.SortPlayerHand(SortCriteria.Rank, 0, PlayerType.Human);
        // change text
        _buttonText.text = suit;
    }

    public void SortBySuit() 
    {
        Debug.Log("SortBySuit()");
        // sort by suit
        UIPlayerHandManager.Instance.SortPlayerHand(SortCriteria.Suit, 0, PlayerType.Human);
        // change text
        _buttonText.text = bestHand;
    }
    
    public void OnSortButtonPressed() 
    {
        currentIndex = IncrementValue(currentIndex);
        methods[currentIndex].Invoke();
    }

    public int IncrementValue(int currentIndex)
    {
        return currentIndex = (currentIndex + 1) % (maxIndex);
    }
}
