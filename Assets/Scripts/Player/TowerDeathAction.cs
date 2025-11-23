using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerDeathAction : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        StatsManager.Instance.OnRunEnded();
        SceneManager.LoadScene("SampleScene");
    }
}