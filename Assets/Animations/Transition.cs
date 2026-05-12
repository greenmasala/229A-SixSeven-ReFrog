using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transition : MonoBehaviour
{
    public RectTransform TransitionImage;
    public RectTransform Text;
    public TextMeshProUGUI LevelText;
    Sequence sequence;
    [SerializeField] AudioClip typeSFX;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChanged;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChanged;
    }
    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            return;
        }
        else
        {
            Debug.Log("running");
            TransitionImage.localScale = new Vector3(1, 1, 1);
            LevelText.rectTransform.localScale = new Vector3(1, 1, 1);
            LevelText.text = $"LEVEL_{SceneManager.GetActiveScene().buildIndex}";
            LevelText.maxVisibleCharacters = 0;
            RunTransition();
        }
    }

    private void Update()
    {
        Debug.Log(LevelText.text);
        Debug.Log(LevelText.maxVisibleCharacters);
    }

    void RunTransition()
    {
        //sequence.Restart();
        sequence = DOTween.Sequence();
        sequence.Append(TransitionImage.DOScaleY(0.15f, 0.6f));
        sequence.InsertCallback(0.4f, () => StartCoroutine(TypeText()));
        sequence.AppendInterval(0.6f);
        sequence.Append(TransitionImage.DOScaleY(0f, 0.6f));
        sequence.Join(Text.DOScaleY(0, 0.2f));
    }

    IEnumerator TypeText()
    {
        int currentCharacterCount = LevelText.textInfo.characterCount + 1;

        for (int i = 0; i < currentCharacterCount; i++)
        {
            LevelText.maxVisibleCharacters = i;
            SFXManager.Instance.PlaySound(typeSFX, transform, 1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
