using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentOverlay : MonoBehaviour
{
    public static PersistentOverlay Instance;

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
}
