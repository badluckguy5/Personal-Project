using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingMessageSpawner : MonoBehaviour
{
    public GameObject messagePrefab;       // The TextMeshProUGUI prefab
    public Transform messageContainer;     // The Vertical Layout Group

    public float messageDuration = 2f;     // How long before it fades out

    public void ShowMessage(string message)
    {
        GameObject newMessage = Instantiate(messagePrefab, messageContainer);
        TextMeshProUGUI tmp = newMessage.GetComponent<TextMeshProUGUI>();
        tmp.text = message;

        // Start fade-out coroutine
        StartCoroutine(FadeAndDestroy(newMessage, messageDuration));
    }

    private IEnumerator FadeAndDestroy(GameObject messageObj, float duration)
    {
        CanvasGroup canvasGroup = messageObj.GetComponent<CanvasGroup>();
        float t = 0f;

        yield return new WaitForSeconds(0.5f); // Optional delay before fading

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        Destroy(messageObj);
    }
}
