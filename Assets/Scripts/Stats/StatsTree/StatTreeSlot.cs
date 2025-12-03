using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatTreeSlot : MonoBehaviour
{
    private RuntimeStat runtimeStat;

    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private Image icon;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TextMeshProUGUI unlockLevelText;
    [SerializeField] private AudioSource purchaseSound;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    public void Initialize(RuntimeStat stat)
    {
        runtimeStat = stat;
        icon.sprite = stat.definition.icon;
        statName.text = stat.definition.statName;
        unlockLevelText.text = $"Unlocks at Lvl {stat.definition.unlockLevel}";
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (runtimeStat == null) return;

        if (runtimeStat.isPurchased)
        {
            lockedOverlay.SetActive(false);
            purchaseButton.interactable = false;
            costText.text = "";

            return;
        }

        if (!runtimeStat.IsUnlocked(StatsManager.Instance.CurrentLevel))
        {
            lockedOverlay.SetActive(true);

            return;
        }

        lockedOverlay.SetActive(false);
        int cost = runtimeStat.definition.sigilsPurchaseCost;
        costText.text = $"{cost}";

        bool canAfford = StatsManager.Instance.CurrentSigils >= cost;
        purchaseButton.interactable = canAfford;
    }

    private void OnPurchaseClicked()
    {
        StatsManager.Instance.PurchaseStat(runtimeStat);
        purchaseSound.Play();
    }
}