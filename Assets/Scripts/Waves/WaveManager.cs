#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform tower;
    [SerializeField] private Health towerHealth;
    [SerializeField] private Slider waveTimerSlider;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private List<WaveSettings> waves;
    [SerializeField] private float timeBetweenWaves = 3f;

    [SerializeField] private float minSpawnRadius = 19f;
    [SerializeField] private float maxSpawnRadius = 27.2f;

    [SerializeField] private float debugYLevelOffset = 1.38f;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;
    private float waveTimer = 0f;

    private void Start()
    {
        StatsManager.Instance.StartNewRun();

        if (waves.Count > 0)
        {
            StartNextWave();
        }
    }

    private void Update()
    {
        if (isWaveActive)
        {
            HandleWaveTimer();
        }
    }

    private void StartNextWave()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWaveIndex + 1}";
        }

        WaveSettings currentSettings = waves[currentWaveIndex];
        waveTimer = currentSettings.duration;
        isWaveActive = true;

        StartCoroutine(SpawnRoutine(currentSettings));
    }

    private IEnumerator SpawnRoutine(WaveSettings settings)
    {
        while (isWaveActive)
        {
            float waitTime = Random.Range(settings.minSpawnInterval, settings.maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isWaveActive) break;

            int count = Random.Range(1, 4);

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy(settings);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private void SpawnEnemy(WaveSettings settings)
    {
        GameObject prefabToSpawn = GetRandomEnemyPrefab(settings);

        if (prefabToSpawn == null) return;

        float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float z = Mathf.Sin(randomAngle) * randomRadius;
        Vector3 spawnPosition = new Vector3(tower.position.x + x, prefabToSpawn.transform.position.y, tower.position.z + z);

        Vector3 lookDirection = tower.position - spawnPosition;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);

        GameObject enemyObj = Instantiate(prefabToSpawn, spawnPosition, rotation);
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.Initialize(tower);
            enemyScript.ApplyDifficultyBuffs(settings.healthMultiplier, settings.damageMultiplier);
        }
    }

    private GameObject GetRandomEnemyPrefab(WaveSettings settings)
    {
        float totalWeight = 0f;
        foreach (var config in settings.enemies)
        {
            totalWeight += config.spawnChance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var config in settings.enemies)
        {
            cumulativeWeight += config.spawnChance;
            if (randomValue <= cumulativeWeight)
            {
                return config.prefab;
            }
        }

        return settings.enemies[0].prefab;
    }

    private void HandleWaveTimer()
    {
        waveTimer -= Time.deltaTime;

        if (waveTimerSlider != null)
        {
            float currentWaveDuration = waves[currentWaveIndex].duration;
            waveTimerSlider.value = waveTimer / currentWaveDuration;
        }

        if (waveTimer <= 0)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isWaveActive = false;

        float playerCurrentHealth = towerHealth.CurrentHealth;
        int waveReached = currentWaveIndex + 1;
        AnalyticsEventsManager.Instance.RecordWaveCompleteEvent(playerCurrentHealth, waveReached);

        if (currentWaveIndex < waves.Count - 1)
        {
            currentWaveIndex++;
            StartCoroutine(PrepareNextWave());
        }
        else
        {
            /*Debug.Log("Level Complete!");
            if (waveText != null)
            {
                waveText.text = "Victory!";
            }*/

            currentWaveIndex = 0;
            StartCoroutine(PrepareNextWave());
        }
    }

    private IEnumerator PrepareNextWave()
    {
        if (waveText != null)
        {
            waveText.text = "Incoming...";
        }

        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }

    public int GetCurrentWave()
    {
        return currentWaveIndex + 1;
    }

    private void OnDrawGizmos()
    {
        if (tower == null)
        {
            return;
        }

#if UNITY_EDITOR
        int segments = 50;
        Vector3 center = tower.position;

        var defaultZTest = Handles.zTest;
        Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

        Handles.color = Color.black;
        DrawHandlesCircle(center, minSpawnRadius, segments);

        Handles.color = Color.green;
        DrawHandlesCircle(center, maxSpawnRadius, segments);

        Handles.zTest = defaultZTest;
#endif
    }

#if UNITY_EDITOR
    private void DrawHandlesCircle(Vector3 center, float radius, int segments)
    {
        float yLevel = center.y + debugYLevelOffset;

        float angle = 0f;
        float angleStep = (2f * Mathf.PI) / segments;

        Vector3[] points = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float x = center.x + Mathf.Cos(angle) * radius;
            float z = center.z + Mathf.Sin(angle) * radius;

            points[i] = new Vector3(x, center.y, z);

            angle += angleStep;
        }

        Handles.DrawPolyLine(points);
    }
#endif
}