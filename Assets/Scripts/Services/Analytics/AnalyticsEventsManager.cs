using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsEventsManager : MonoBehaviour
{
    public static AnalyticsEventsManager Instance { get; private set; }

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

    public void RecordPlayerDeathEvent(int _waveReached, int _soulsGained)
    {
        AnalyticsService.Instance.RecordEvent(new PlayerDeathEvent
        {
            WaveReached = _waveReached,
            SoulsGained = _soulsGained,
        });

        Debug.LogWarning($"PlayerDeathEvent Recorded: WaveReached={_waveReached}, SoulsGained={_soulsGained}");
    }

    public void RecordRunStartEvent(int _currentSouls, int _playerLevel)
    {
        AnalyticsService.Instance.RecordEvent(new RunStartEvent
        {
            CurrentSouls = _currentSouls,
            PlayerLevel = _playerLevel
        });

        Debug.LogWarning($"RunStartEvent Recorded: CurrentSouls={_currentSouls}, PlayerLevel={_playerLevel}");
    }

    public void RecordWaveCompleteEvent(float _playerCurrentHealth, int _waveReached)
    {
        AnalyticsService.Instance.RecordEvent(new WaveCompleteEvent
        {
            PlayerCurrentHealth = _playerCurrentHealth,
            WaveReached = _waveReached
        });

        Debug.LogWarning($"WaveCompleteEvent Recorded: PlayerCurrentHealth={_playerCurrentHealth}, WaveReached={_waveReached}");
    }

    public void RecordStatUpgradeEvent(string _statName, string _statType, int _statLevelReached)
    {
        AnalyticsService.Instance.RecordEvent(new StatUpgradeEvent
        {
            StatName = _statName,
            StatType = _statType,
            StatLevelReached = _statLevelReached
        });

        Debug.LogWarning($"StatUpgradeEvent Recorded: StatName={_statName}, StatType={_statType}, StatLevelReached={_statLevelReached}");
    }

    public void RecordStatUnlockedEvent(string _statName, int _playerLevel, int _currentSigils)
    {
        AnalyticsService.Instance.RecordEvent(new StatUnlockedEvent
        {
            StatName = _statName,
            PlayerLevel = _playerLevel,
            CurrentSigils = _currentSigils
        });

        Debug.LogWarning($"StatUnlockedEvent Recorded: StatName={_statName}, PlayerLevel={_playerLevel}, CurrentSigils={_currentSigils}");
    }
    
    public void RecordSkinPurchasedEvent(string _skinName, int _playerLevel)
    {
        AnalyticsService.Instance.RecordEvent(new SkinPurchasedEvent
        {
            SkinName = _skinName,
            PlayerLevel = _playerLevel
        });

        Debug.LogWarning($"SkinPurchasedEvent Recorded: SkinName={_skinName}, PlayerLevel={_playerLevel}");
    }

    public void RecordRunSurrenderEvent(int _waveReached, int _soulsGained)
    {
        AnalyticsService.Instance.RecordEvent(new RunSurrenderEvent
        {
            WaveReached = _waveReached,
            SoulsGained = _soulsGained,
        });

        Debug.LogWarning($"RunSurrenderEvent Recorded: WaveReached={_waveReached}, SoulsGained={_soulsGained}");
    }
}