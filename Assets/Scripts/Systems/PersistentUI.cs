using System.Collections;
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance; 
    public Animator RefreshUI;
    public GameObject LevelComplete;
    public GameObject Credits;

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

    IEnumerator CreditsCoroutine()
    {
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "LOADING...";
        PersistentOverlay.Instance.RunTransition(true);
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        yield return new WaitForSeconds(1.15f);
        PersistentOverlay.Instance.RunTransition(false);
        Credits.SetActive(true);
    }
}
