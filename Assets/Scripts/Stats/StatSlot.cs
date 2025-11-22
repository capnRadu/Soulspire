using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlot : MonoBehaviour
{
    private RuntimeStat runtimeStat;

    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI cost;

    private bool isHubMode;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateUI;
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    public void Initialize(RuntimeStat stat, bool hubMode)
    {
        runtimeStat = stat;
        isHubMode = hubMode;
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (runtimeStat == null) return;

        statName.text = runtimeStat.definition.statName;
        int totalLevel = runtimeStat.permanentLevel + runtimeStat.runLevel;
        level.text = $"Lvl {totalLevel}";
        value.text = $"{runtimeStat.GetValue()}";

        int costAmount = 0;
        bool canAfford = false;

        if (isHubMode)
        {
            costAmount = runtimeStat.GetSoulCost();
            canAfford = StatsManager.Instance.CurrentSouls >= costAmount;

            cost.text = $"{costAmount}";
        }
        else
        {
            costAmount = runtimeStat.GetCoinCost();
            canAfford = StatsManager.Instance.CurrentCoins >= costAmount;

            cost.text = $"{costAmount}";
        }

        upgradeButton.interactable = canAfford;
    }

    private void OnUpgradeClicked()
    {
        if (runtimeStat == null) return;

        bool success = false;

        if (isHubMode)
        {
            success = StatsManager.Instance.TryUpgradePermanentStat(runtimeStat.definition.type);
        }
        else
        {
            success = StatsManager.Instance.TryUpgradeRunStat(runtimeStat.definition.type);
        }

        if (success)
        {
            UpdateUI();
        }
    }
}