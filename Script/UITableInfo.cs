using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GlobalDefine;

public class UITableInfo : MonoBehaviour, IObserverTable
{
    [SerializeField]
    private TextMeshProUGUI _tableText;

    private Big2TableManager tableManager;

    void Start()
    {
        tableManager = Big2TableManager.Instance;
        AddSelfToSubjectList();
    }

    public void OnNotifyAssigningCard(CardInfo cardInfo)
    {
       // do nothing
    }

    public void OnNotifyTableState(HandType tableHandType, HandRank tableRank)
    {
        if (tableHandType == HandType.None)
        {
            _tableText.text = "";
        }
        else
        {
            _tableText.text = tableHandType.ToString();
        }       
    }

    public void RemoveSelfToSubjectList()
    {
       tableManager.RemoveObserver(this);
    }
    public void AddSelfToSubjectList()
    {
        tableManager.AddObserver(this);
    }

    private void OnDisable()
    {
        RemoveSelfToSubjectList();
    }
}
