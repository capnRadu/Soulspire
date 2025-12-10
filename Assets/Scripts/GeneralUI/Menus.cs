using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject consentMenu;
    [SerializeField] private AudioSource popUpOpenSound;

    private void Start()
    {
        if (AnalyticsManager.Instance != null && !AnalyticsManager.Instance.isInitialized)
        {
            StartCoroutine(ShowConsentMenu());
        }
    }

    private IEnumerator ShowConsentMenu()
    {
        yield return new WaitForSeconds(0.5f);
        popUpOpenSound.Play();
        consentMenu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}