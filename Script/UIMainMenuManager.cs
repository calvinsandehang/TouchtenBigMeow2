using UnityEngine;
using static GlobalDefine;

public class UIMainMenuManager : MonoBehaviour, ISubscriber
{
    [SerializeField]
    private CanvasGroup _chooseAvatarMenu;

    private void Start()
    {
        HideChooseAvatarMenu();
    }

    /// <summary>
    /// Sets the visibility and interactivity of a CanvasGroup.
    /// </summary>
    /// <param name="canvasGroup">The CanvasGroup to set.</param>
    /// <param name="isVisible">Whether the CanvasGroup should be visible and interactable.</param>
    public void SetCanvasGroupVisibility(CanvasGroup canvasGroup, bool isVisible)
    {
        if (isVisible)
        {
            canvasGroup.alpha = 1f;          // Fully visible
            canvasGroup.interactable = true; // Can interact
            canvasGroup.blocksRaycasts = true; // Can receive input events
        }
        else
        {
            canvasGroup.alpha = 0f;          // Fully transparent
            canvasGroup.interactable = false; // Cannot interact
            canvasGroup.blocksRaycasts = false; // Cannot receive input events
        }
    }

    public void HideChooseAvatarMenu()
    {
        SetCanvasGroupVisibility(_chooseAvatarMenu, false);
    }

    public void HideChooseAvatarMenu(PlayerType playerType) 
    {
        SetCanvasGroupVisibility(_chooseAvatarMenu, false);
    }

    public void SubscribeEvent()
    {
        //Big2GlobalEvent.SubscribeAvatarIsSet(HideChooseAvatarMenu);
    }

    public void UnsubscribeEvent()
    {
        //Big2GlobalEvent.UnsubscribeAvatarIsSet(HideChooseAvatarMenu);
    }
}
