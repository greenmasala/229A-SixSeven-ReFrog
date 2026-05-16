using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject CreditsMenu;
    public GameObject LevelCompleteMenu;
    public GameObject RefreshCount;

    int levelID;
    int nextLevelID;
    public bool Paused;
    public bool StopDeath;
    public bool Dead;
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
            Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextAppear();
            PersistentOverlay.Instance.RunTransition(false);
            StopDeath = false;
            Dead = false;
            Debug.Log(PauseMenu);
            levelID = SceneManager.GetActiveScene().buildIndex;
            nextLevelID = levelID + 1;
            Debug.Log("LevelID" + levelID);
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

    private void Start()
    {
        levelID = SceneManager.GetActiveScene().buildIndex;
        nextLevelID = levelID + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) & !StopDeath & !Dead)
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

            //if (Input.GetKeyDown(KeyCode.R) & SceneManager.GetActiveScene().buildIndex != 0)
            //{
            //    Restart();
            //    //NextLevel();
            //}
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        PauseMenu.gameObject.SetActive(true);
        Paused = true;
        Debug.Log("paused");
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        PauseMenu.gameObject.SetActive(false);
        Paused = false;
        if (Refresh.Instance.Layout1Active || Refresh.Instance.Layout2Active)
        {
            Refresh.Instance.HideLayout();
        }
    }
    public void Restart()
    {
        if (restartRoutine != null || StopDeath)
        {
            return;
        }
        Time.timeScale = 1f;
        restartRoutine = StartCoroutine(RestartRoutine());
       
        //Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        //Destroy(DDOL.Instance.gameObject);
        //DDOL.Instance = null;
        //PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "RESTARTING...";
        //LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
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

    IEnumerator DeathRoutine(Transform transform)
    {
        Dead = true;
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        Instantiate(DeathFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(TransitionRoutine("RESTARTING...", true));
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
            StopDeath = true;
            Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
            PersistentOverlay.Instance.RunTransition(true);
            yield return new WaitForSeconds(1.15f);
            LoadLevel(nextLevelID);
        }
    }

    IEnumerator RestartRoutine()
    {
        Refresh.Instance.RefreshCountText.GetComponent<Flicker>().TextDisappear();
        PersistentOverlay.Instance.TransitionRef.GetComponent<LevelTransition>().TitleText = "RESTARTING...";
        LevelManager.Instance.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(0.7f);
        Destroy(DDOL.Instance.gameObject);
        DDOL.Instance = null;
    }
    //public void Credits()
    //{
    //    CreditsMenu.gameObject.SetActive(!CreditsMenu.activeInHierarchy);
    //    LevelCompleteMenu.SetActive(!LevelCompleteMenu.activeInHierarchy);
    //    RefreshCount.SetActive(!RefreshCount.activeInHierarchy);
    //}
}
