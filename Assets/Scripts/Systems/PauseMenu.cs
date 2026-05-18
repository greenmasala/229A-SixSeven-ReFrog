using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject SettingsMenu;
    public void Resume()
    {
        GameManager.Instance.Unpause();
    }
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
    public void ReturnToMenu()
    {
        StartCoroutine(BackToMenu());
    }
    public void Settings()
    {
        SettingsMenu.gameObject.SetActive(!SettingsMenu.activeInHierarchy);
    }

    IEnumerator BackToMenu()
    {
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "LOADING...";
        PersistentOverlay.Instance.RunTransition(true);
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        yield return new WaitForSecondsRealtime(1.15f);
        Time.timeScale = 1f;
        Destroy(DDOL.Instance.gameObject);
        DDOL.Instance = null;
        GameManager.Instance.LoadLevel(0);
    }
}
