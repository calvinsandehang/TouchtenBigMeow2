using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverCard 
{
    public void OnNotify(CardState cardState);

    public void AddSelfToSubjectList();

    public void RemoveSelfToSubjectList();

}
