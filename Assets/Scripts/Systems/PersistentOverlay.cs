using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentOverlay : MonoBehaviour
{
    public static PersistentOverlay Instance;
    public GameObject TransitionRef;

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
        TransitionRef.SetActive(activate);
    }
}
