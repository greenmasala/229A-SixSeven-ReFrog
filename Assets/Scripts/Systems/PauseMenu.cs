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
        Destroy(DDOL.Instance.gameObject);
        DDOL.Instance = null;
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void Settings()
    {
        SettingsMenu.gameObject.SetActive(!SettingsMenu.activeInHierarchy);
    }
}
