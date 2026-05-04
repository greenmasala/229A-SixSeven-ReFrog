using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
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
        gameObject.SetActive(false);
    }
}
