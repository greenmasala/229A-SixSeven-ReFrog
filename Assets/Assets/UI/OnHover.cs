using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI buttonName;
    Color ogButtonColor;
    Color hoverColor;
    public float TypeSpeed = 0.01f;
    [SerializeField] List<Image> changeColor = new List<Image>();
    private void Awake()
    {
        buttonName = GetComponentInChildren<TextMeshProUGUI>();
        ogButtonColor = buttonName.color;
        hoverColor = new Color(0.03529412f, 0.07450981f, 0.003921569f);
        var changeColor = GetComponentsInChildren<Image>().ToList();
        changeColor.RemoveAt(0);
        this.changeColor = changeColor;
    }

    private void OnEnable()
    {
        buttonName.maxVisibleCharacters = 0;
        StartCoroutine(TypeText());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TypeText());
        Color c = changeColor[0].color;
        c.a = 1;
        changeColor[0].color = c;

        buttonName.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color c = changeColor[0].color;
        c.a = 0;
        changeColor[0].color = c;

        buttonName.color = ogButtonColor;
    }
    IEnumerator TypeText()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        int currentCharacterCount = buttonName.textInfo.characterCount + 1;

        for (int i = 0; i < currentCharacterCount; i++)
        {
            buttonName.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(TypeSpeed);
        }
    }
}
