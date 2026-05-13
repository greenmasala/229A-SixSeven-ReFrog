using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public GameObject LevelSelect;
    public GameObject CreditsMenu;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        MainMenu.SetActive(true);
    }
    public void LevelSelectMenu()
    {
        LevelSelect.SetActive(!LevelSelect.activeInHierarchy);
        MainMenu.SetActive(!MainMenu.activeInHierarchy);
    }

    public void Settings()
    {
        SettingsMenu.gameObject.SetActive(!SettingsMenu.activeInHierarchy);
    }

    public void LoadLevel(int levelID)
    {
        LevelManager.Instance.LoadLevel(levelID);
    }

    public void Credits()
    {
        CreditsMenu.gameObject.SetActive(!CreditsMenu.activeInHierarchy);
        MainMenu.SetActive(!MainMenu.activeInHierarchy);
    }
}
