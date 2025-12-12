using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StatType
{
    Health,
    HealthRegeneration,
    DamageReduction,
    DodgeChance,
    LifeSteal,
    HealOnKill,
    Damage,
    AttackSpeed,
    CriticalChance,
    DamagePerMeter,
    Range
}

public enum StatCategory
{
    Offensive,
    Defensive
}

[Serializable]
public class RuntimeStat
{
    public StatDefinition definition;
    public int permanentLevel;
    public int runLevel;
    public bool isPurchased = false;

    public float GetValue()
    {
        int totalLevel = permanentLevel + runLevel;
        return definition.GetValue(totalLevel);
    }

    public int GetCoinCost()
    {
        return definition.GetCost(runLevel, definition.baseCoinCost);
    }

    public int GetSoulCost()
    {
        return definition.GetCost(permanentLevel, definition.baseSoulCost);
    }

    public bool IsMaxedOut()
    {
        int totalLevel = permanentLevel + runLevel;
        return totalLevel >= definition.maxLevel;
    }

    public bool IsUnlocked(int playerLevel)
    {
        return playerLevel >= definition.unlockLevel;
    }
}

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [SerializeField] private List<StatDefinition> availableStats = new List<StatDefinition>();

    private Dictionary<StatType, RuntimeStat> stats = new Dictionary<StatType, RuntimeStat>();
    [SerializeField] private List<RuntimeStat> runtimeStatsDebug;

    [SerializeField] private int currentCoins;
    public int CurrentCoins => currentCoins;
    [SerializeField] private int currentSouls;
    public int CurrentSouls => currentSouls;
    [SerializeField] private int currentSigils;
    public int CurrentSigils => currentSigils;
    [SerializeField] private int currentDiamonds;
    public int CurrentDiamonds => currentDiamonds;

    private int currentLevel = 1;
    public int CurrentLevel => currentLevel;
    private float currentXP = 0;
    public float CurrentXP => currentXP;
    private float baseXPReq = 100f; 
    private float xpMultiplier = 1.25f;
    public float RequiredXP => CalculateRequiredXP();

    private int startOfRunLevel;
    public int StartOfRunLevel => startOfRunLevel;
    private int soulsCollectedInRun;
    public int SoulsCollectedInRun => soulsCollectedInRun;
    private float xpCollectedInRun;
    public float XPCollectedInRun => xpCollectedInRun;
    [SerializeField] private int soulsRewardPerLevelUp = 10;

    private bool isXpBuffActive = false;
    public bool IsXpBuffActive => isXpBuffActive;
    private bool isSoulsBuffActive = false;
    public bool IsSoulsBuffActive => isSoulsBuffActive;

    public event Action OnCurrencyChanged;
    public event Action<StatType> OnStatUpgraded;
    public event Action OnXPChange;
    public event Action OnLevelUp;

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

        InitializeStats();
        LoadData();
    }

    private void InitializeStats()
    {
        foreach (var statDef in availableStats)
        {
            RuntimeStat newStat = new RuntimeStat();
            newStat.definition = statDef;
            newStat.permanentLevel = 0;
            newStat.runLevel = 0;

            stats.Add(statDef.type, newStat);
            runtimeStatsDebug.Add(newStat);
        }
    }

    public void SaveData()
    {
        // Currency
        PlayerPrefs.SetInt("Currency_Souls", currentSouls);
        PlayerPrefs.SetInt("Currency_Sigils", currentSigils);
        PlayerPrefs.SetInt("Currency_Diamonds", currentDiamonds);

        // Level Progression
        PlayerPrefs.SetInt("Player_Level", currentLevel);
        PlayerPrefs.SetFloat("Player_XP", currentXP);

        // Stat Upgrades and Purchases
        foreach (var stat in stats.Values)
        {
            string keyPrefix = $"Stat_{stat.definition.type}";
            PlayerPrefs.SetInt($"{keyPrefix}_PermLevel", stat.permanentLevel);
            PlayerPrefs.SetInt($"{keyPrefix}_Purchased", stat.isPurchased ? 1 : 0);
        }

        // IAP Buffs
        PlayerPrefs.SetInt("IAP_DoubleSouls", isSoulsBuffActive ? 1 : 0);
        PlayerPrefs.SetInt("IAP_DoubleXP", isXpBuffActive ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        // Currency
        currentSouls = PlayerPrefs.GetInt("Currency_Souls", 0);
        currentSigils = PlayerPrefs.GetInt("Currency_Sigils", 0);
        currentDiamonds = PlayerPrefs.GetInt("Currency_Diamonds", 0);

        // Level Progression
        currentLevel = PlayerPrefs.GetInt("Player_Level", 1);
        currentXP = PlayerPrefs.GetFloat("Player_XP", 0f);

        // Stat Upgrades and Purchases
        foreach (var stat in stats.Values)
        {
            string keyPrefix = $"Stat_{stat.definition.type}";
            stat.permanentLevel = PlayerPrefs.GetInt($"{keyPrefix}_PermLevel", 0);

            // Check Unlock Level OR Saved Purchase State
            bool purchasedSave = PlayerPrefs.GetInt($"{keyPrefix}_Purchased", 0) == 1;
            stat.isPurchased = purchasedSave;
        }

        // IAP Buffs
        isSoulsBuffActive = PlayerPrefs.GetInt("IAP_DoubleSouls", 0) == 1;
        isXpBuffActive = PlayerPrefs.GetInt("IAP_DoubleXP", 0) == 1;
    }

    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteKey("Currency_Souls");
        PlayerPrefs.DeleteKey("Currency_Sigils");
        PlayerPrefs.DeleteKey("Player_Level");
        PlayerPrefs.DeleteKey("Player_XP");

        foreach (var stat in stats.Values)
        {
            string keyPrefix = $"Stat_{stat.definition.type}";

            PlayerPrefs.DeleteKey($"{keyPrefix}_PermLevel");
            PlayerPrefs.DeleteKey($"{keyPrefix}_Purchased");
        }

        PlayerPrefs.Save();

        currentSouls = 0;
        currentSigils = 0;

        currentLevel = 1;
        currentXP = 0;

        foreach (var stat in stats.Values)
        {
            stat.permanentLevel = 0;
            stat.isPurchased = false;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float GetStatValue(StatType type)
    {
        if (stats.TryGetValue(type, out RuntimeStat stat))
        {
            return stat.GetValue();
        }

        return 0f;
    }

    public void UpgradeRunStat(StatType type)
    {
        RuntimeStat stat = stats[type];

        if (stat.IsMaxedOut())
        {
            Debug.Log($"{type} is already at Max Level!");
            return;
        }

        int cost = stat.GetCoinCost();

        if (currentCoins >= cost)
        {
            stat.runLevel++;
            SpendCoins(cost);
            OnStatUpgraded?.Invoke(type);

            Debug.Log($"Upgraded {type} to Run Level {stat.runLevel}. New Value: {stat.GetValue()}");

            string statName = stat.definition.statName;
            string statType = "Run";
            int statLevelReached = stat.runLevel + stat.permanentLevel;

            AnalyticsEventsManager.Instance.RecordStatUpgradeEvent(statName, statType, statLevelReached);
        }
    }

    public void UpgradePermanentStat(StatType type)
    {
        RuntimeStat stat = stats[type];

        if (stat.IsMaxedOut())
        {
            Debug.Log($"{type} is already at Max Level!");
            return;
        }

        int cost = stat.GetSoulCost();

        if (currentSouls >= cost)
        {
            stat.permanentLevel++;
            SpendSouls(cost);
            SaveData();

            Debug.Log($"Upgraded {type} Permanently to Level {stat.permanentLevel}.");

            string statName = stat.definition.statName;
            string statType = "Permanent";
            int statLevelReached = stat.permanentLevel;

            AnalyticsEventsManager.Instance.RecordStatUpgradeEvent(statName, statType, statLevelReached);
        }
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;
        OnCurrencyChanged?.Invoke();
    }

    public void EarnCoins(int amount)
    {
        currentCoins += amount;
        OnCurrencyChanged?.Invoke();
    }

    public void SpendSouls(int amount)
    {
        currentSouls -= amount;
        OnCurrencyChanged?.Invoke();
    }

    public void EarnSouls(int amount, bool shouldBuff = false)
    {
        if (shouldBuff && isSoulsBuffActive)
        {
            amount *= 2;
        }

        currentSouls += amount;
        soulsCollectedInRun += amount;
        OnCurrencyChanged?.Invoke();
        SaveData();
    }

    public void PurchaseStat(RuntimeStat stat)
    {
        if (stat.isPurchased)
        {
            Debug.Log($"{stat.definition.statName} is already purchased.");
            return;
        }

        int cost = stat.definition.sigilsPurchaseCost;

        if (currentSigils >= cost)
        {
            stat.isPurchased = true;
            SpendSigils(cost);
            SaveData();

            Debug.Log($"Purchased {stat.definition.statName} for {cost} Sigils.");

            string statName = stat.definition.statName;
            int playerLevel = currentLevel;
            int playerCurrentSigils = currentSigils;

            AnalyticsEventsManager.Instance.RecordStatUnlockedEvent(statName, playerLevel, playerCurrentSigils);
        }
    }

    public void SpendSigils(int amount)
    {
        currentSigils -= amount;
        Debug.Log($"Spent {amount} Sigils. Remaining Sigils: {currentSigils}");
        OnCurrencyChanged?.Invoke();
    }

    public void EarnSigils(int amount)
    {
        currentSigils += amount;
        OnCurrencyChanged?.Invoke();
        SaveData();
    }

    public void SpendDiamonds(int amount)
    {
        currentDiamonds -= amount;
        OnCurrencyChanged?.Invoke();
        SaveData();
    }

    public void EarnDiamonds(int amount)
    {
        currentDiamonds += amount;
        OnCurrencyChanged?.Invoke();
        SaveData();
    }

    public void EarnXP(float amount)
    {
        if (isXpBuffActive)
        {
            amount *= 2f;
        }

        currentXP += amount;
        xpCollectedInRun += amount;
        CheckForLevelUp();
        OnXPChange?.Invoke();
    }

    private void CheckForLevelUp()
    {
        float required = CalculateRequiredXP();

        while (currentXP >= required)
        {
            currentXP -= required;
            currentLevel++;

            Debug.Log($"LEVEL UP! Player is now Level {currentLevel}");
            OnLevelUp?.Invoke();

            required = CalculateRequiredXP();
        }

        SaveData();
    }

    private float CalculateRequiredXP()
    {
        return baseXPReq * Mathf.Pow(xpMultiplier, currentLevel - 1);
    }

    public List<RuntimeStat> GetAllRuntimeStatsFromCategory(StatCategory category)
    {
        List<RuntimeStat> filteredStats = new List<RuntimeStat>();

        foreach (var stat in stats.Values)
        {
            if (stat.definition.category == category)
            {
                filteredStats.Add(stat);
            }
        }

        return filteredStats;
    }

    public List<RuntimeStat> GetAllRuntimeStats()
    {
        return new List<RuntimeStat>(stats.Values);
    }

    public void StartNewRun()
    {
        startOfRunLevel = currentLevel;
        soulsCollectedInRun = 0;
        xpCollectedInRun = 0;

        int currentSoulsAtRunStart = currentSouls;
        int currentPlayerLevelAtRunStart = currentLevel;

        AnalyticsEventsManager.Instance.RecordRunStartEvent(currentSoulsAtRunStart, currentPlayerLevelAtRunStart);
    }

    public int CalculateLevelUpBonus()
    {
        int levelsGained = currentLevel - startOfRunLevel;

        if (levelsGained <= 0)
        {
            return 0;
        }

        return levelsGained * soulsRewardPerLevelUp * currentLevel;
    }

    public int CalculateSigilsReward()
    {
        int levelsGained = currentLevel - startOfRunLevel;

        if (levelsGained <= 0)
        {
            return 0;
        }

        return levelsGained;
    }

    public void OnRunEnded()
    {
        foreach (var stat in stats.Values)
        {
            stat.runLevel = 0;
        }

        currentCoins = 0;
    }

    public void ActivateXpBuff()
    {
        isXpBuffActive = true;
        SaveData();
    }

    public void ActivateSoulsBuff()
    {
        isSoulsBuffActive = true;
        SaveData();
    }
}