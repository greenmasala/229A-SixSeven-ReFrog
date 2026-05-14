using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioClip typeSFX;
    public bool PlaySFX;
    public float TypeSpeed = 0.01f;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        text.maxVisibleCharacters = 0;
        StartCoroutine(TypeText());
    }
    IEnumerator TypeText()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        int currentCharacterCount = text.textInfo.characterCount + 1;

        for (int i = 0; i < currentCharacterCount; i++)
        {
            text.maxVisibleCharacters = i;
            if (typeSFX != null || PlaySFX)
            {
                AudioManager.Instance.PlaySFX(typeSFX);
            }
            else
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Typing);
            }
            yield return new WaitForSecondsRealtime(TypeSpeed);
        }
    }
}
