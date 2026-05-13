using System.Collections;
using TMPro;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (text.text != null)
        {
            text.enabled = false;
            StartCoroutine(FlickerText());
        }
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
    public void TextAppear()
    {
        StartCoroutine(FlickerText());
    }

    public void TextDisappear()
    {
        StartCoroutine(UnflickerText());
    }
    IEnumerator UnflickerText()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        text.enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        text.enabled = true;
        yield return new WaitForSecondsRealtime(0.1f);
        text.enabled = false;
    }
}
