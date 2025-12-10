using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private bool userGaveConsent = false;
    public bool UserGaveConsent
    {
        get => userGaveConsent;
    }

    public bool isInitialized = false;

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
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (userGaveConsent)
        {
            SetUserConsent(true);
        }
    }

    public void SetUserConsent(bool consent)
    {
        userGaveConsent = consent;

        var consentState = new ConsentState
        {
            AnalyticsIntent = consent ? ConsentStatus.Granted : ConsentStatus.Denied
        };

        EndUserConsent.SetConsentState(consentState);
        isInitialized = true;
    }
}