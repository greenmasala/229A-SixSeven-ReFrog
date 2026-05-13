using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance; 
    public Animator RefreshUI;
    public GameObject LevelComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LevelCompleteActive()
    {
        LevelComplete.SetActive(!LevelComplete.activeInHierarchy);
    }
}
