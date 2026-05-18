using System.Collections;
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance; 
    public Animator RefreshUI;
    public GameObject PauseMenu;
    public GameObject LevelComplete;
    public GameObject PreviewOverlay;
    public GameObject PreviewOverlay2;
    public GameObject Credits;
    Coroutine previewRoutine;
    Coroutine disablePreviewRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LevelCompleteActive()
    {
        LevelComplete.SetActive(!LevelComplete.activeInHierarchy);
    }
    public void CreditsActive()
    {
        StartCoroutine(CreditsCoroutine());
    }

    public void LayoutPreview()
    {
        if (previewRoutine != null)
        {
            StopCoroutine(previewRoutine);
        }
        if (Refresh.Instance.HasRefreshed)
        {
            previewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay));
        }
        else
        {
            previewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay2));
        }
    }

    public void PreviewDisable()
    {
        if (disablePreviewRoutine != null)
        {
            StopCoroutine(disablePreviewRoutine);
        }
        if (Refresh.Instance.Layout1Active)
        {
            disablePreviewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay));
        }
        else if (Refresh.Instance.Layout2Active)
        {
            disablePreviewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay2));
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
    }

    IEnumerator CreditsCoroutine()
    {
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "LOADING...";
        PersistentOverlay.Instance.RunTransition(true);
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        yield return new WaitForSeconds(1.15f);
        PersistentOverlay.Instance.RunTransition(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ThankYou);
        Credits.SetActive(true);
    }

    IEnumerator PreviewCoroutine(GameObject PreviewOverlay) //spamming causes inconsistencies will have to come check in this and refresh
    {
        PreviewOverlay.SetActive(!PreviewOverlay.activeInHierarchy);
        yield return new WaitForSeconds(0.1f);
        PreviewOverlay.SetActive(!PreviewOverlay.activeInHierarchy);
        yield return new WaitForSeconds(0.1f);
        PreviewOverlay.SetActive(!PreviewOverlay.activeInHierarchy);
    }
}
