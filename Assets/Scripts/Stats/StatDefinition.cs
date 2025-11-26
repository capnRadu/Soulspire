using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stats/Stat Definition")]
public class StatDefinition : ScriptableObject
{
    public string statName;
    public StatType type;
    public StatCategory category;
    public int unlockLevel;
    public int sigilsPurchaseCost;
    public int maxLevel;

    public float baseValue;
    public float valuePerLevel;

    public int baseCoinCost;
    public int baseSoulCost;

    public float costMultiplier;

    public float GetValue(int level)
    {
        return baseValue + (valuePerLevel * level);
    }

    public int GetCost(int currentLevel, int baseCost)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }
}