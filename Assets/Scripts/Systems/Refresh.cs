using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    public GameObject[] Layout1;
    public GameObject[] Layout2;
    public bool Layout1Active;
    public bool Layout2Active; 
    public GameObject[] UniversalLayout;
    public TextMeshProUGUI RefreshCountText;
    Coroutine refreshCoroutine;
    public bool HasRefreshed;
    public float InitialRefreshDelay = 0.35f;
    int currentColumn;
    int currentColumn2;

    Coroutine CoFadeInLayout;
    Coroutine CoFadeOutLayout;

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
        if (!GameManager.Instance.StopDeath & !GameManager.Instance.Paused & SceneManager.GetActiveScene().buildIndex != 0 & !GameManager.Instance.Dead)
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
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Flicker);
                StartCoroutine(FadeInUniLayout());
                PersistentUI.Instance.PreviewActive();
                if (CoFadeInLayout != null)
                {
                    StopCoroutine(CoFadeInLayout);
                }
                if (HasRefreshed)
                {
                    CoFadeInLayout = StartCoroutine(FadeInLayout(Layout1));
                    Layout1Active = true;
                }
                else
                {
                    CoFadeInLayout = StartCoroutine(FadeInLayout(Layout2));
                    Layout2Active = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.Flicker);
                StartCoroutine(FadeOutUniLayout());
                PersistentUI.Instance.PreviewDisable();
                if (CoFadeOutLayout != null)
                {
                    StopCoroutine(CoFadeOutLayout);
                }
                if (Layout1Active)
                {
                    CoFadeOutLayout = StartCoroutine(FadeOutLayout(Layout1));
                    Layout1Active = false;
                }
                else if (Layout2Active)
                {
                    CoFadeOutLayout = StartCoroutine(FadeOutLayout(Layout2));
                    Layout2Active = false;
                }
                //if (HasRefreshed)
                //{
                //    if (CoFadeOutLayout != null)
                //    {
                //        StopCoroutine(CoFadeOutLayout);
                //    }
                //    if (Layout1.activeInHierarchy)
                //        CoFadeOutLayout = StartCoroutine(FadeOutLayout(0.3f, 0f, Layout1));
                //}
                //else if (HasRefreshed & !Layout2.activeInHierarchy)
                //{
                //    if (CoFadeOutLayout != null)
                //    {
                //        StopCoroutine(CoFadeOutLayout);
                //    }
                //    if (Layout2.activeInHierarchy)
                //    {
                //        CoFadeOutLayout = StartCoroutine(FadeOutLayout(0.3f, 0f, Layout2));
                //    }
                //}
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

    IEnumerator FadeInLayout(GameObject[] layout)
    {
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        //foreach (var uni in UniversalLayout)
        //{
        //    Color uniStartingColor = uni.GetComponent<SpriteRenderer>().color;
        //    Color c2 = uni.GetComponent<SpriteRenderer>().color;
        //    c2.a = endAlpha;
        //}

        //Color startingColor = layout.color;
        //Color c = layout.color;
        //c.a = endAlpha;
        //float elapsed = 0;

        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;
        //    layout.color = Color.Lerp(startingColor, c, elapsed / duration);

        //    foreach (var uni in UniversalLayout)
        //    {

        //    }
        //    yield return null;
        //}
        //layout.color = c;
    }
    IEnumerator FadeOutLayout(GameObject[] layout)
    {
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject obj in layout)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    IEnumerator FadeInUniLayout()
    {
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
    }

    IEnumerator FadeOutUniLayout()
    {
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var uni in UniversalLayout)
        {
            if (uni != null)
            {
                uni.SetActive(false);
            }
        }
    }
}
