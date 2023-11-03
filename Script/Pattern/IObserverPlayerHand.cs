using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverPlayerHand 
{
    //public void OnNotify(List<CardModel> cardModels, int index);
    public void AddSelfToSubjectList();
    public void RemoveSelfToSubjectList();
}
