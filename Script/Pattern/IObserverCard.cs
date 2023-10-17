using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserverCard 
{
    // classes that inherit from the IObserver interface
    // must implement the OnNotify method
    public void OnNotify(CardState cardState) 
    {
        // do something when the event happens
    }

    public void AddSelfToSubjectList() 
    {
        // reference the subject and call AddObserver(this)
    }

}
