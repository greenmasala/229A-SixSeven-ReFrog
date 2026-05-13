using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Refresh : MonoBehaviour
{
    public float RefreshDelay = 0.5f;
    public int RefreshCount;
    public GameObject[] Columns;
    public GameObject[] Columns2;
    public Tilemap Layout1;
    public Tilemap Layout2;
    public TextMeshProUGUI RefreshCountText;
    Coroutine refreshCoroutine;
    public bool HasRefreshed;
    public float InitialRefreshDelay = 0.35f;
    int currentColumn;
    int currentColumn2;

    Coroutine CoFadeInLayout;

    [SerializeField] AudioClip refreshSFX;

    public static Refresh Instance { get; private set; }

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
            if (refreshCoroutine != null)
            {
                StopCoroutine(refreshCoroutine);
                currentColumn = 0;
                currentColumn2 = 0;
                PersistentUI.Instance.RefreshUI.SetBool("HasRefreshed", false);
                PersistentUI.Instance.RefreshUI.ResetControllerState();
            }
            HasRefreshed = false;
            Debug.Log("Acitve");
            gameObject.SetActive(true);
            RefreshCount = FindFirstObjectByType<Player>().RefreshCount;
            Layout1 = GameObject.FindGameObjectWithTag("Layout1").GetComponent<Tilemap>();
            Layout2 = GameObject.FindGameObjectWithTag("Layout2").GetComponent<Tilemap>();

            Columns = GameObject.FindGameObjectsWithTag("Column").OrderByDescending(o =>
            {
                var numberPart = new string(o.name.Where(char.IsDigit).ToArray());
                return int.Parse(numberPart);
            }).ToArray();

            Columns2 = GameObject.FindGameObjectsWithTag("Column2").OrderByDescending(o =>
            {
                var numberPart = new string(o.name.Where(char.IsDigit).ToArray());
                return int.Parse(numberPart);
            }).ToArray();

            if (RefreshCountText != null)
            {
                RefreshCountText.text = RefreshCount.ToString();
            }
           
            foreach (GameObject column in Columns)
            {
                column.SetActive(true);
            }

            foreach (GameObject column2 in Columns2)
            {
                column2.SetActive(false);
            }
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
        if (!GameManager.Instance.Win & !GameManager.Instance.Paused & SceneManager.GetActiveScene().buildIndex != 0 & !GameManager.Instance.Dead)
        {
            if (Input.GetKeyDown(KeyCode.E) & RefreshCount > 0)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Refresh);
                RefreshCount--;
                HasRefreshed = !HasRefreshed;

                Debug.Log("REFRESHING...");
                PersistentUI.Instance.RefreshUI.SetBool("HasRefreshed", HasRefreshed);

                RefreshCountText.text = RefreshCount.ToString();
                Debug.Log("hasRefreshed" + HasRefreshed);

                if (refreshCoroutine != null)
                {
                    StopCoroutine(refreshCoroutine);
                    Debug.Log("Stopped midway");
                }
                refreshCoroutine = StartCoroutine(Refreshing());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (HasRefreshed)
                {
                    if (CoFadeInLayout != null)
                    {
                        StopCoroutine(CoFadeInLayout);
                    }
                    CoFadeInLayout = StartCoroutine(FadeInLayout(0.3f, 0.5f, Layout1));
                }
                else
                {
                    if (CoFadeInLayout != null)
                    {
                        StopCoroutine(CoFadeInLayout);
                    }
                    CoFadeInLayout = StartCoroutine(FadeInLayout(0.3f, 0.5f, Layout2));
                }
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                CoFadeInLayout = StartCoroutine(FadeInLayout(0.3f, 0f, Layout1));
                CoFadeInLayout = StartCoroutine(FadeInLayout(0.3f, 0f, Layout2));
            }
        }
    }

    IEnumerator Refreshing()
    {
        yield return new WaitForSeconds(InitialRefreshDelay);
        while (currentColumn < Columns.Length)
        {
            if (HasRefreshed)
            {
                Columns[currentColumn].SetActive(!Columns[currentColumn].activeInHierarchy);
                Debug.Log("current column: " + currentColumn);
                currentColumn = Mathf.Clamp(currentColumn + 1, 0, Columns.Length);

                Columns2[currentColumn2].SetActive(!Columns2[currentColumn2].activeInHierarchy);
                Debug.Log("current column 2: " + currentColumn2);
                currentColumn2 = Mathf.Clamp(currentColumn2 + 1, 0, Columns.Length);
            }
            else 
            {
                Debug.Log("current column returning: " + currentColumn);
                currentColumn = Mathf.Clamp(currentColumn - 1, 0, Columns.Length);
                Columns[currentColumn].SetActive(!Columns[currentColumn].activeInHierarchy);

                Debug.Log("current column 2 returning: " + currentColumn);
                currentColumn2 = Mathf.Clamp(currentColumn2 - 1, 0, Columns.Length);
                Columns2[currentColumn2].SetActive(!Columns2[currentColumn2].activeInHierarchy);

                if (currentColumn == 0)
                {
                    Debug.Log("done returning stopped at: " + currentColumn);
                    yield break;
                }
            }
            yield return new WaitForSeconds(RefreshDelay);
        }

        while (currentColumn > -1 & !HasRefreshed)
        {
            Debug.Log("current column returning: " + currentColumn);
            currentColumn = Mathf.Clamp(currentColumn - 1, 0, Columns.Length);
            Columns[currentColumn].SetActive(!Columns[currentColumn].activeInHierarchy);

            Debug.Log("current column 2 returning: " + currentColumn);
            currentColumn2 = Mathf.Clamp(currentColumn2 - 1, 0, Columns.Length);
            Columns2[currentColumn2].SetActive(!Columns2[currentColumn2].activeInHierarchy);

            if (currentColumn == 0)
            {
                Debug.Log("done returning stopped at: " + currentColumn);
                yield break;
            }

            yield return new WaitForSeconds(RefreshDelay);
            Debug.Log("run");
        }

        Debug.Log("done stopped at: " + currentColumn);
        yield break;
    }

    public void ReplenishRefresh()
    {
        RefreshCount++;
        RefreshCountText.text = RefreshCount.ToString();
    }

    IEnumerator FadeInLayout(float duration, float endAlpha, Tilemap layout)
    {
        Color startingColor = layout.color;
        Color c = layout.color;
        c.a = endAlpha;
        float elapsed = 0;
 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            layout.color = Color.Lerp(startingColor, c, elapsed / duration);
            yield return null;
        }
        layout.color = c;
    }
}
