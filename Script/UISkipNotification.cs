using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISkipNotification : MonoBehaviour, ISubscriber
{
    public float fadeInDuration = 1.0f;  // Duration for fading in
    public float fadeOutDuration = 1.0f; // Duration for fading out
    public float displayDuration = 1.0f; // Duration for displaying

    private Image image;

    private Big2SimpleAI simpleAI; 

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        // Hide the image initially
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void InitializeUIElement(Big2SimpleAI simpleAI)
    {
        this.simpleAI = simpleAI;
        SubscribeEvent();
    }

    // Method to instantly show the image and then fade out after a delay
    public void ShowAndFadeOut()
    {
        StartCoroutine(ShowAndFadeOutCoroutine());
    }

    private IEnumerator ShowAndFadeOutCoroutine()
    {
        // Fade in
        float elapsedTime = 0f;        
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        // Display for a specified duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void SubscribeEvent()
    {
        simpleAI.UIOnAISkipTurn += ShowAndFadeOut;
    }

    public void UnsubscribeEvent()
    {
        simpleAI.UIOnAISkipTurn -= ShowAndFadeOut;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
}
