using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentOverlay : MonoBehaviour
{
    public static PersistentOverlay Instance;

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += SceneChanged;
    //}
    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= SceneChanged;
    //}
    //private void SceneChanged(Scene scene, LoadSceneMode mode)
    //{
    //    var refreshCount = Instance.GetComponentInChildren<TextMeshProUGUI>();
    //    if (scene.buildIndex == 0)
    //    {
    //        refreshCount.enabled = false;
    //    }
    //    else
    //    {
    //        refreshCount.enabled = true;
    //    }
    //}
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
