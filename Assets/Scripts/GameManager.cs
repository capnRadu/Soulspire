#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform tower;

    [SerializeField] private float minSpawnRadius = 13f;
    [SerializeField] private float maxSpawnRadius = 15f;

    [SerializeField] private float debugYLevelOffset = 0.8f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (enemyPrefabs.Length != 0 && tower != null)
            {
                SpawnObjectNearTower();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (enemyPrefabs.Length != 0 && tower != null)
            {
                SpawnObjectNearTower();
            }
            else
            {
                Debug.LogWarning("Enemy prefabs array is empty or tower reference is missing.");
            }
        }
    }

    public void SpawnObjectNearTower()
    {
        GameObject objectToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        float x = Mathf.Cos(randomAngle) * randomRadius;
        float z = Mathf.Sin(randomAngle) * randomRadius;

        Vector3 spawnPosition = new Vector3(tower.position.x + x, objectToSpawn.transform.position.y, tower.position.z + z);
        
        Vector3 lookDirection = tower.position - spawnPosition;
        Quaternion rotationToTower = Quaternion.LookRotation(lookDirection);
        rotationToTower.x = 0;
        rotationToTower.z = 0;

        Instantiate(objectToSpawn, spawnPosition, rotationToTower);
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