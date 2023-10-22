using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big2PlayerUIManager : MonoBehaviour
{
    private Big2SimpleAI simpleAI;
    public  UISkipNotification skipNotification;

    private void Awake()
    {
        simpleAI = GetComponent<Big2SimpleAI>();
    }
    public void SetupSkipNotificationButton(UISkipNotification skipNotification) 
    {
        this.skipNotification = skipNotification;
        skipNotification.InitializeUIElement(simpleAI);
    }

}
