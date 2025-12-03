using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatSlot : MonoBehaviour
{
    private RuntimeStat runtimeStat;

    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private AudioSource buySound;

    private bool isHubMode;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateUI;
        StatsManager.Instance.OnLevelUp += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateUI;
        StatsManager.Instance.OnLevelUp -= UpdateUI;
    }

    public void Initialize(RuntimeStat stat, bool hubMode)
    {
        runtimeStat = stat;
        icon.sprite = stat.definition.icon;
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

        if (runtimeStat.IsMaxedOut())
        {
            cost.text = "Max";
            upgradeButton.interactable = false;

            return;
        }

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

        if (isHubMode)
        {
            StatsManager.Instance.UpgradePermanentStat(runtimeStat.definition.type);
        }
        else
        {
            StatsManager.Instance.UpgradeRunStat(runtimeStat.definition.type);
        }

        buySound.Play();
    }
}