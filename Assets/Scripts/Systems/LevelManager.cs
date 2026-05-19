using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadLevel(int levelID)
    {
        StartCoroutine(LoadLevelCoroutine(levelID));
    }

    public void LoadNextLevel()
    {
        int levelID = SceneManager.GetActiveScene().buildIndex;
        int nextLevelID = levelID + 1;
        StartCoroutine(LoadLevelCoroutine(nextLevelID));
    }

    IEnumerator LoadLevelCoroutine(int levelID)
    {
        PersistentOverlay.Instance.RunTransition(true);
        yield return new WaitForSecondsRealtime(1.15f);
        if (DDOL.Instance != null)
        {
            Destroy(DDOL.Instance.gameObject);
            DDOL.Instance = null;
        }
        yield return null;
        SceneManager.LoadScene("Bootloader");
        yield return null;
        SceneManager.LoadSceneAsync(levelID);
    }
}
