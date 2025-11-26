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

    [SerializeField] private GameObject lockedPanel;
    [SerializeField] private TextMeshProUGUI locked;

    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI purchaseCostText;

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
        isHubMode = hubMode;
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
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

            if (lockedPanel != null)
            {
                lockedPanel.SetActive(false);
            }

            return;
        }

        if (!runtimeStat.IsUnlocked(StatsManager.Instance.CurrentLevel))
        {
            locked.text = $"Unlocks at Lvl {runtimeStat.definition.unlockLevel}";
            upgradeButton.interactable = false;

            if (lockedPanel != null)
            {
                lockedPanel.SetActive(true);
            }

            return;
        }
        else if (!runtimeStat.isPurchased)
        {
            if (lockedPanel != null)
            {
                lockedPanel.SetActive(false);
            }

            purchaseCostText.text = $"{runtimeStat.definition.sigilsPurchaseCost}";

            int sigilCost = runtimeStat.definition.sigilsPurchaseCost;
            bool canAffordPurchase = StatsManager.Instance.CurrentSigils >= sigilCost;

            purchaseButton.interactable = canAffordPurchase;

            if (purchasePanel != null)
            {
                purchasePanel.SetActive(true);
            }
        }
        else
        {
            if (lockedPanel != null)
            {
                lockedPanel.SetActive(false);
            }

            if (purchasePanel != null)
            {
                purchasePanel.SetActive(false);
            }
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
    }

    private void OnPurchaseClicked()
    {
        if (runtimeStat == null) return;

        StatsManager.Instance.PurchaseStat(runtimeStat);
    }
}