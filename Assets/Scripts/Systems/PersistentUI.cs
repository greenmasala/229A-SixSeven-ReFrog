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

    public void HideLayout(GameObject[] layout, GameObject[] layout2)
    {
        foreach (GameObject go in layout)
        {
            go.gameObject.SetActive(false);
        }
        PreviewOverlay.SetActive(false);
        foreach (GameObject go in layout2)
        {
            go.gameObject.SetActive(false);
        }
        PreviewOverlay2.SetActive(false);
    }

    public void LayoutPreview(GameObject[] layout, GameObject[] layout2, GameObject[] uniLayout)
    {
        if (previewRoutine != null)
        {
            StopCoroutine(previewRoutine);
        }
        if (Refresh.Instance.HasRefreshed)
        {
            previewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay, layout, uniLayout));
            Refresh.Instance.Layout1Active = true;
            foreach (GameObject go in layout2)
            {
                go.gameObject.SetActive(false);
            }
            PreviewOverlay2.SetActive(false);
        }
        else
        {
            previewRoutine = StartCoroutine(PreviewCoroutine(PreviewOverlay2, layout2, uniLayout));
            Refresh.Instance.Layout2Active = true;
            foreach (GameObject go in layout)
            {
                go.gameObject.SetActive(false);
            }
            PreviewOverlay.SetActive(false);
        }
    }

    public void PreviewDisable(GameObject[] layout, GameObject[] layout2, GameObject[] uniLayout)
    {
        if (disablePreviewRoutine != null)
        {
            StopCoroutine(disablePreviewRoutine);
        }
        if (Refresh.Instance.Layout1Active)
        {
            disablePreviewRoutine = StartCoroutine(DisablePreviewCoroutine(PreviewOverlay, layout, uniLayout));
            Refresh.Instance.Layout1Active = false;
            foreach (GameObject go in layout2)
            {
                go.gameObject.SetActive(false);
            }
            PreviewOverlay2.SetActive(false);
        }
        else if (Refresh.Instance.Layout2Active)
        {
            disablePreviewRoutine = StartCoroutine(DisablePreviewCoroutine(PreviewOverlay2, layout2, uniLayout));
            Refresh.Instance.Layout2Active = false;
            foreach (GameObject go in layout)
            {
                go.gameObject.SetActive(false);
            }
            PreviewOverlay.SetActive(false);
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

    IEnumerator PreviewCoroutine(GameObject PreviewOverlay, GameObject[] layout, GameObject[] uniLayout) 
    {
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
        PreviewOverlay.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
        PreviewOverlay.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
        PreviewOverlay.SetActive(true);
    }

    IEnumerator DisablePreviewCoroutine(GameObject PreviewOverlay, GameObject[] layout, GameObject[] uniLayout)
    {
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
        PreviewOverlay.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
        PreviewOverlay.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        foreach (var uni in uniLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
        PreviewOverlay.SetActive(false);
    }
}
