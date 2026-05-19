using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Refresh : MonoBehaviour
{
    public float RefreshDelay = 0.5f;
    public int RefreshCount;
    public GameObject[] Columns;
    public GameObject[] Columns2;
    public GameObject[] Layout1;
    public GameObject[] Layout2;
    public GameObject[] UniversalLayout;
    public bool Layout1Active;
    public bool Layout2Active; 
    public TextMeshProUGUI RefreshCountText;
    Coroutine refreshCoroutine;
    public bool HasRefreshed;
    public float InitialRefreshDelay = 0.35f;
    int currentColumn;
    int currentColumn2;

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
            
            if (scene.buildIndex != SceneManager.sceneCountInBuildSettings - 1)
            {
                RefreshCount = FindFirstObjectByType<Player>().RefreshCount;
            }
            Layout1 = GameObject.FindGameObjectsWithTag("Layout1");
            Layout2 = GameObject.FindGameObjectsWithTag("Layout2");
            UniversalLayout = GameObject.FindGameObjectsWithTag("UniLayout");
            foreach (var item in Layout1)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in Layout2)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var uni in UniversalLayout)
            {
                uni.gameObject.SetActive(false);
            }

            if (Layout1Active || Layout2Active)
            {
                PersistentUI.Instance.HideLayout(Layout1, Layout2);
                Layout1Active = false;
                Layout2Active = false;
            }
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

            foreach (GameObject column in Columns)
            {
                column.SetActive(true);
            }

            foreach (GameObject column2 in Columns2)
            {
                column2.SetActive(false);
            }
        }
        if (RefreshCountText != null)
        {
            RefreshCountText.text = RefreshCount.ToString();
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
        if (!GameManager.Instance.Paused & SceneManager.GetActiveScene().buildIndex != 0 & !GameManager.Instance.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.E) & RefreshCount > 0)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Refresh);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Refresh2);
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
                ShowLayout();
            }

            if (Input.GetKeyUp(KeyCode.Q) & Layout1Active || Input.GetKeyUp(KeyCode.Q) & Layout2Active) 
            {
                HideLayout();
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

    public void ShowLayout()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Flicker);
        PersistentUI.Instance.LayoutPreview(Layout1, Layout2, UniversalLayout);
    }

    public void HideLayout()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Flicker);
        PersistentUI.Instance.PreviewDisable(Layout1, Layout2, UniversalLayout);
    }

    //public void ReplenishRefresh()
    //{
    //    RefreshCount++;
    //    RefreshCountText.text = RefreshCount.ToString();
    //}
}
