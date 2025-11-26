using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunResultsUI : MonoBehaviour
{
    [SerializeField] private GameObject resultsMenu;
    [SerializeField] private Health towerHealth;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI soulsText;
    [SerializeField] private TextMeshProUGUI xpText;

    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private TextMeshProUGUI levelUpText;
    [SerializeField] private TextMeshProUGUI bonusSoulsText;
    [SerializeField] private TextMeshProUGUI sigilsRewardText;

    private void OnEnable()
    {
        towerHealth.OnDeath += ShowResults;
    }

    private void OnDisable()
    {
        towerHealth.OnDeath -= ShowResults;
    }

    private void ShowResults()
    {
        Time.timeScale = 0f;
        resultsMenu.SetActive(true);

        coinsText.text = StatsManager.Instance.CoinsCollectedInRun.ToString();
        soulsText.text = StatsManager.Instance.SoulsCollectedInRun.ToString();
        xpText.text = StatsManager.Instance.XPCollectedInRun.ToString();
    }

    public void ResultsClaim()
    {
        int levelsGained = StatsManager.Instance.CurrentLevel - StatsManager.Instance.StartOfRunLevel;

        if (levelsGained > 0)
        {
            resultsMenu.SetActive(false);
            levelUpMenu.SetActive(true);

            levelUpText.text = $"Leveled Up {levelsGained} times!";
            bonusSoulsText.text = $"+{StatsManager.Instance.CalculateLevelUpBonus()} Souls";
            sigilsRewardText.text = $"+{StatsManager.Instance.CalculateSigilsReward()} Sigils";
        }
        else
        {
            GoToHub();
        }
    }

    public void LevelUpClaim()
    {
        StatsManager.Instance.EarnSouls(StatsManager.Instance.CalculateLevelUpBonus());
        StatsManager.Instance.EarnSigils(StatsManager.Instance.CalculateSigilsReward());
        GoToHub();
    }

    private void GoToHub()
    {
        Time.timeScale = 1f;
        StatsManager.Instance.OnRunEnded();
        SceneManager.LoadScene("MainMenu");
    }
}