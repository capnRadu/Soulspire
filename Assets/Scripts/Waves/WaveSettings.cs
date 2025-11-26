using UnityEngine;

[System.Serializable]
public struct EnemySpawnConfig
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance;
}

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave System/Wave Settings")]
public class WaveSettings : ScriptableObject
{
    public float duration = 15f;
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    public float healthMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;

    public EnemySpawnConfig[] enemies;
}