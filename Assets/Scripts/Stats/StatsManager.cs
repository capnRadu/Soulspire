using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Damage
}

public enum StatCategory
{
    Offensive,
    Defensive
}

[System.Serializable]
public class RuntimeStat
{
    public StatDefinition definition;
    public int permanentLevel;
    public int runLevel;

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

    public event Action OnCurrencyChanged;

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

    public float GetStatValue(StatType type)
    {
        if (stats.TryGetValue(type, out RuntimeStat stat))
        {
            return stat.GetValue();
        }

        return 0f;
    }

    public bool TryUpgradeRunStat(StatType type)
    {
        RuntimeStat stat = stats[type];
        int cost = stat.GetCoinCost();

        if (currentCoins >= cost)
        {
            currentCoins -= cost;
            stat.runLevel++;
            OnCurrencyChanged?.Invoke();

            Debug.Log($"Upgraded {type} to Run Level {stat.runLevel}. New Value: {stat.GetValue()}");
            return true;
        }

        return false;
    }

    public bool TryUpgradePermanentStat(StatType type)
    {
        RuntimeStat stat = stats[type];
        int cost = stat.GetSoulCost();

        if (currentSouls >= cost)
        {
            currentSouls -= cost;
            stat.permanentLevel++;
            OnCurrencyChanged?.Invoke();

            Debug.Log($"Upgraded {type} Permanently to Level {stat.permanentLevel}.");
            return true;
        }
        return false;
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

    public void OnRunEnded()
    {
        foreach (var stat in stats.Values)
        {
            stat.runLevel = 0;
        }

        currentCoins = 0;
    }
}