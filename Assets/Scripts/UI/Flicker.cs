using System.Collections;
using TMPro;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    TextMeshProUGUI text;
    CanvasGroup canvas;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        canvas = GetComponent<CanvasGroup>();
        if (text != null)
        {
            if (text.text != null)
            {
                text.enabled = false;
                StartCoroutine(FlickerText());
            }
        }
        if (canvas != null)
        {
            canvas.alpha = 0;
            StartCoroutine(FlickerCanvas());
        }
    }

    public void TextAppear()
    {
        StartCoroutine(FlickerText());
    }

    public void TextDisappear()
    {
        StartCoroutine(UnflickerText());
    }

    IEnumerator FlickerText()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        text.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        text.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        text.enabled = true;
    }
    IEnumerator UnflickerText()
    {
        if (text.enabled)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            text.enabled = false;
            yield return new WaitForSecondsRealtime(0.1f);
            text.enabled = true;
            yield return new WaitForSecondsRealtime(0.1f);
            text.enabled = false;
        }
    }

    IEnumerator FlickerCanvas()
    {
        Debug.Log("flicker");
        yield return new WaitForSecondsRealtime(0.2f);
        canvas.alpha = 1;
        yield return new WaitForSecondsRealtime(0.1f);
        canvas.alpha = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        canvas.alpha = 1;
    }
}
