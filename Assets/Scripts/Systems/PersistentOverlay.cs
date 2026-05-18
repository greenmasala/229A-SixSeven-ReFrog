using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentOverlay : MonoBehaviour
{
    public static PersistentOverlay Instance;
    public GameObject TransitionRef;

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
        Instance.RunTransition(false);
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void RunTransition(bool activate)
    {
        if (Refresh.Instance != null)
        {
            Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        }
        TransitionRef.SetActive(activate);
    }
}
