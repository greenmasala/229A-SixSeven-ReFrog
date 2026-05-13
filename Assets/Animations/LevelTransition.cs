using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public RectTransform TransitionImage;
    public TextMeshProUGUI Title;
    public string TitleText;
    private void OnEnable()
    {
        Debug.Log("running");
        TransitionImage.sizeDelta = new Vector2(TransitionImage.sizeDelta.x, 0);
        Title.text = TitleText;
        RunTransition();
    }

    public void RunTransition()
    {
        TransitionImage.DOSizeDelta(new Vector2(TransitionImage.sizeDelta.x, 1150), 0.3f);
    }
}
