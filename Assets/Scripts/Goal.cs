using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public GameObject LevelComplete;

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
    //    LevelComplete = GameObject.Find("LevelComplete");
    //    LevelComplete.SetActive(false);
    //    GameManager.Instance.Win = false;
    //}

    //private void Awake()
    //{
    //    if (LevelComplete != null)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        LevelComplete = GameObject.Find("LevelComplete");
    //        LevelComplete.SetActive(false);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelComplete.SetActive(true);
            GameManager.Instance.Win = true;
        }
    }
}
