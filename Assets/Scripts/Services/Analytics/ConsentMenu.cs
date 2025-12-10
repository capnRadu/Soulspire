using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class ConsentMenu : MonoBehaviour
{
    [SerializeField] private Button optInButton;
    [SerializeField] private Button deleteDataButton;
    [SerializeField] private Button optOutButton;

    private void OnEnable()
    {
        switch (AnalyticsManager.Instance.UserGaveConsent)
        {
            case true:
                optInButton.interactable = false;
                optOutButton.interactable = true;
                deleteDataButton.interactable = true;
                break;
            case false:
                optInButton.interactable = true;
                optOutButton.interactable = false;
                deleteDataButton.interactable = false;
                break;
        }
    }

    public void OptIn()
    {
        AnalyticsManager.Instance.SetUserConsent(true);

        optInButton.interactable = false;
        optOutButton.interactable = true;
        deleteDataButton.interactable = true;

        Debug.Log("Analytics data collection started");
    }

    public void OptOut()
    {
        AnalyticsManager.Instance.SetUserConsent(false);

        optInButton.interactable = true;
        optOutButton.interactable = false;
        deleteDataButton.interactable = true;

        Debug.Log("Analytics data collection stopped");
    }

    public void DeleteData()
    {
        AnalyticsManager.Instance.SetUserConsent(false);
        AnalyticsService.Instance.RequestDataDeletion();

        optInButton.interactable = true;
        optOutButton.interactable = false;
        deleteDataButton.interactable = false;

        Debug.Log("Analytics data deleted and data collection stopped");
    }
}