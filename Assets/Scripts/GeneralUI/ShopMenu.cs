using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private IAPManager iapManager;

    [Header("Bundles purchase UI")]
    [SerializeField] private TextMeshProUGUI wizardBundleButtonText;
    [SerializeField] private TextMeshProUGUI masteryBundleButtonText;
    [SerializeField] private TextMeshProUGUI kingBundleButtonText;

    [Header("Buffs purchase UI")]
    [SerializeField] private Button purchaseDoubleXpButton;
    [SerializeField] private TextMeshProUGUI doubleXpButtonText;
    [SerializeField] private Button purchaseDoubleSoulsButton;
    [SerializeField] private TextMeshProUGUI doubleSoulsButtonText;

    [Header("Diamonds purchase UI")]
    [SerializeField] private TextMeshProUGUI bagOfDiamondsButtonText;
    [SerializeField] private TextMeshProUGUI bucketOfDiamondsButtonText;
    [SerializeField] private TextMeshProUGUI barrelOfDiamondsButtonText;
    [SerializeField] private TextMeshProUGUI chestOfDiamondsButtonText;

    [Header("Souls purchase UI")]
    [SerializeField] private Button purchaseSoulsPouchButton;
    [SerializeField] private Button purchaseSoulsUrnButton;
    [SerializeField] private Button purchaseSoulsAltarButton;

    [SerializeField] private AudioSource purchaseSound;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateButtonInteractability;
        UpdateButtonInteractability();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateButtonInteractability;
    }

    #region UI
    private void UpdateButtonInteractability()
    {
        purchaseSoulsPouchButton.interactable = StatsManager.Instance.CurrentDiamonds >= 50;
        purchaseSoulsUrnButton.interactable = StatsManager.Instance.CurrentDiamonds >= 250;
        purchaseSoulsAltarButton.interactable = StatsManager.Instance.CurrentDiamonds >= 1000;

        if (StatsManager.Instance.IsXpBuffActive)
        {
            doubleXpButtonText.text = "Active";
            purchaseDoubleXpButton.interactable = false;
        }

        if (StatsManager.Instance.IsSoulsBuffActive)
        {
            doubleSoulsButtonText.text = "Active";
            purchaseDoubleSoulsButton.interactable = false;
        }
    }

    public void UpdateButtonPrice(string productId, string price)
    {
        switch (productId)
        {
            case "wizard_bundle":
                wizardBundleButtonText.text = price;
                break;
            case "mastery_bundle":
                masteryBundleButtonText.text = price;
                break;
            case "king_bundle":
                kingBundleButtonText.text = price;
                break;
            case "xp_doubler":
                doubleXpButtonText.text = price;
                break;
            case "souls_doubler":
                doubleSoulsButtonText.text = price;
                break;
            case "diamonds_100":
                bagOfDiamondsButtonText.text = price;
                break;
            case "diamonds_600":
                bucketOfDiamondsButtonText.text = price;
                break;
            case "diamonds_1300":
                barrelOfDiamondsButtonText.text = price;
                break;
            case "diamonds_2800":
                chestOfDiamondsButtonText.text = price;
                break;
            default:
                Debug.LogWarning($"Unknown product ID: {productId}");
                break;
        }
    }
    #endregion

    #region Methods called by UI buttons to trigger purchases
    public void OnPurchaseWizardBundleButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.WizardBundle);
    }

    public void OnPurchaseMasteryBundleButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.MasteryBundle);
    }

    public void OnPurchaseKingBundleButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.KingBundle);
    }

    public void OnPurchaseDoubleXpButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.XpDoubler);
    }

    public void OnPurchaseDoubleSoulsButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.SoulsDoubler);
    }

    public void OnPurchaseBagOfDiamondsButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.Diamonds100);
    }

    public void OnPurchaseBucketOfDiamondsButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.Diamonds600);
    }

    public void OnPurchaseBarrelOfDiamondsButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.Diamonds1300);
    }

    public void OnPurchaseChestOfDiamondsButtonPressed()
    {
        iapManager.BuyProduct(IAPProductKey.Diamonds2800);
    }

    public void PurchasePouchOfSouls()
    {
        if (StatsManager.Instance.CurrentDiamonds >= 50)
        {
            StatsManager.Instance.SpendDiamonds(50);
            StatsManager.Instance.EarnSouls(500);
            purchaseSound.Play();
        }
    }

    public void PurchaseUrnOfSouls()
    {
        if (StatsManager.Instance.CurrentDiamonds >= 250)
        {
            StatsManager.Instance.SpendDiamonds(250);
            StatsManager.Instance.EarnSouls(2750);
            purchaseSound.Play();
        }
    }

    public void PurchaseAltarOfSouls()
    {
        if (StatsManager.Instance.CurrentDiamonds >= 1000)
        {
            StatsManager.Instance.SpendDiamonds(1000);
            StatsManager.Instance.EarnSouls(12000);
            purchaseSound.Play();
        }
    }
    #endregion

    #region Methods used by IAPManager to reward purchases
    public void PurchaseBagOfDiamonds(int quantity)
    {
        StatsManager.Instance.EarnDiamonds(100 * quantity);
    }

    public void PurchaseBucketOfDiamonds(int quantity)
    {
        StatsManager.Instance.EarnDiamonds(600 * quantity);
    }

    public void PurchaseBarrelOfDiamonds(int quantity)
    {
        StatsManager.Instance.EarnDiamonds(1300 * quantity);
    }

    public void PurchaseChestOfDiamonds(int quantity)
    {
        StatsManager.Instance.EarnDiamonds(2800 * quantity);
    }

    public void PurchaseWizardBundle(int quantity)
    {
        PurchaseDoubleSoulsBuff();
        StatsManager.Instance.EarnDiamonds(150 * quantity);
        StatsManager.Instance.EarnSouls(1000 * quantity);
    }

    public void PurchaseMasteryBundle(int quantity)
    {
        PurchaseDoubleXPBuff();
        StatsManager.Instance.EarnDiamonds(600 * quantity);
        StatsManager.Instance.EarnSouls(5000 * quantity);
    }

    public void PurchaseKingBundle(int quantity)
    {
        PurchaseDoubleSoulsBuff();
        PurchaseDoubleXPBuff();
        StatsManager.Instance.EarnDiamonds(1500 * quantity);
        StatsManager.Instance.EarnSouls(20000 * quantity);
    }

    public void PurchaseDoubleSoulsBuff()
    {
        StatsManager.Instance.ActivateSoulsBuff();
        purchaseDoubleSoulsButton.interactable = false;
        doubleSoulsButtonText.text = "Active";
    }

    public void PurchaseDoubleXPBuff()
    {
        StatsManager.Instance.ActivateXpBuff();
        purchaseDoubleXpButton.interactable = false;
        doubleXpButtonText.text = "Active";
    }
    #endregion
}