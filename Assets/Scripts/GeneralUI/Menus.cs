using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject consentMenu;
    [SerializeField] private AudioSource popUpOpenSound;

    private void Start()
    {
        if (AnalyticsManager.Instance != null && !AnalyticsManager.Instance.HasSeenMenu)
        {
            StartCoroutine(ShowConsentMenu());
        }
    }

    private IEnumerator ShowConsentMenu()
    {
        yield return new WaitForSeconds(0.5f);

        if (!AnalyticsManager.Instance.HasSeenMenu)
        {
            AnalyticsManager.Instance.MarkAsSeen();
            popUpOpenSound.Play();
            consentMenu.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void TriggerResetProgress()
    {
        StatsManager.Instance.ResetAllProgress();
    }
}