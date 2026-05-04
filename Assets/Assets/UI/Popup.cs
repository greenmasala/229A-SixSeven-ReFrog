using UnityEngine;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    RectTransform position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        position = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        position.transform.localPosition = new Vector2(0f, -100f);
        position.DOAnchorPos(new Vector2(0f, 0f), 0.3f).SetUpdate(true);
    }
}
