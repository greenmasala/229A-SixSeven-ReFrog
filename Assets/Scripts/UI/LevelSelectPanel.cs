using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : MonoBehaviour
{
    public Button[] Buttons;
    public GameObject levelButtons;
    public float AnimSpeed;

    private void Awake()
    {
        AddLevelToPanel();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1); 
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        for (int i = 0; i < Mathf.Clamp(unlockedLevel, 1, Buttons.Length); i++)
        {
            Debug.Log(i);
            Buttons[i].interactable = true;
        }
        foreach (var button in Buttons)
        {
            button.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ButtonPopup());
        foreach(var button in Buttons)
        {
            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                text.enabled = text.GetComponentInParent<Button>().interactable;
            }
        }
    }

    void AddLevelToPanel()
    {
        int childCount = levelButtons.transform.childCount;
        Buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    IEnumerator ButtonPopup()
    {
        foreach (var button in Buttons)
        {
            button.GetComponent<CanvasGroup>().alpha = 0;
        }
        foreach (var button in Buttons)
        {
            var position = button.GetComponent<RectTransform>();
            position.transform.localPosition = new Vector2(position.transform.localPosition.x, position.transform.localPosition.y - 100);
            var s = DOTween.Sequence();
            s.Append(position.DOAnchorPos(new Vector2(position.transform.localPosition.x, position.transform.localPosition.y + 100), 0.1f).SetUpdate(true));
            s.Insert(0.05f, button.GetComponent<CanvasGroup>().DOFade(1, 0.01f));
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Popup);
            yield return new WaitForSeconds(AnimSpeed);
        }
    }
}
