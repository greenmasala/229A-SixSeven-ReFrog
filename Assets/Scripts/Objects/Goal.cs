using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public ParticleSystem WinFX;
    Animator WinAnim;
    void Start()
    {
        WinAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WinAnim.SetBool("IsWinning", true);

            var winFXPrefab = Instantiate(WinFX, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(winFXPrefab, 0.6f);
            UnlockLevel();
            PersistentUI.Instance.LevelCompleteActive();
            GameManager.Instance.GameOver = true;
        }
    }

    void UnlockLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
