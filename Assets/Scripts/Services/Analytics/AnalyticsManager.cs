using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private const string ConsentKey = "Analytics_UserConsent";
    private const string SeenKey = "Analytics_HasSeenMenu";

    private bool userGaveConsent = false;
    public bool UserGaveConsent => userGaveConsent;

    public bool HasSeenMenu => PlayerPrefs.GetInt(SeenKey, 0) == 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            userGaveConsent = PlayerPrefs.GetInt(ConsentKey, 0) == 1;
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (HasSeenMenu)
        {
            SetUserConsent(userGaveConsent);
        }
    }

    public void MarkAsSeen()
    {
        PlayerPrefs.SetInt(SeenKey, 1);
        PlayerPrefs.Save();
    }

    public void SetUserConsent(bool consent)
    {
        userGaveConsent = consent;

        PlayerPrefs.SetInt(ConsentKey, consent ? 1 : 0);
        PlayerPrefs.SetInt(SeenKey, 1);
        PlayerPrefs.Save();

        var consentState = new ConsentState
        {
            AnalyticsIntent = consent ? ConsentStatus.Granted : ConsentStatus.Denied
        };

        EndUserConsent.SetConsentState(consentState);
    }
}