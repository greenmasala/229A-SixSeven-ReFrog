using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int levelID;
    int nextLevelID;
    public bool Paused;
    public bool GameOver;

    public ParticleSystem DeathFX;
    Coroutine restartRoutine;
    public static GameManager Instance { get; private set; }

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
        if (scene.buildIndex == 0)
        {
            return;
        }
        else
        {
            Time.timeScale = 1f;
            Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextAppear();
            PersistentOverlay.Instance.RunTransition(false);
            GameOver = false;
            levelID = SceneManager.GetActiveScene().buildIndex;
            nextLevelID = levelID + 1;
        }
    }
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

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) & !GameOver)
            {
                if (Paused == true)
                {
                    Unpause();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Pause()
    {
        PersistentUI.Instance.Pause();
        Time.timeScale = 0f;
        Paused = true;
        Debug.Log("paused");
    }

    public void Unpause()
    {
        PersistentUI.Instance.Pause();
        Time.timeScale = 1f;
        Paused = false;
        if (Refresh.Instance.Layout1Active || Refresh.Instance.Layout2Active)
        {
            Refresh.Instance.HideLayout();
        }
    }
    public void Restart()
    {
        if (restartRoutine != null)
        {
            return;
        }
        restartRoutine = StartCoroutine(RestartRoutine());
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
        {
            PersistentUI.Instance.CreditsActive();
        }
        else
        {
            StartCoroutine(TransitionRoutine("LOADING...", false));
        }
    }

    public void LoadLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void Death(Transform transform)
    {
        StartCoroutine(DeathRoutine(transform));
    }
    IEnumerator TransitionRoutine(string text, bool restart)
    {
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = text;
        if (restart)
        {
            Restart();
        }
        else
        {
            Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
            PersistentOverlay.Instance.RunTransition(true);
            yield return new WaitForSeconds(1.15f);
            LoadLevel(nextLevelID);
        }
    }

    IEnumerator DeathRoutine(Transform transform)
    {
        GameOver = true;
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        Instantiate(DeathFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(TransitionRoutine("RESTARTING...", true));
    }

    IEnumerator RestartRoutine()
    {
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "RESTARTING...";
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        return null;
    }
}
