using UnityEngine;
using System.Collections;

public class CanvasTrigger : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f; //ความเร็วของ fade

    private Coroutine fadeRoutine;

    private void Start()
    {
        SetCanvasVisible(false, true); // ปิดทันทีตอนเริ่ม
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetCanvasVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetCanvasVisible(false);
        }
    }

    //--------------------------------
    private void SetCanvasVisible(bool show, bool instant = false)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        if (instant)
        {
            canvasGroup.alpha = show ? 1 : 0;
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;
        }
        else
        {
            fadeRoutine = StartCoroutine(FadeCanvas(show));
        }
    }

    private IEnumerator FadeCanvas(bool show)
    {
        float startAlpha = canvasGroup.alpha;
        float endAlpha = show ? 1f : 0f;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, blend);
            yield return null;
        }

        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
