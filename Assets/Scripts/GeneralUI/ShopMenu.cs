using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
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

    private void UpdateButtonInteractability()
    {
        purchaseSoulsPouchButton.interactable = StatsManager.Instance.CurrentDiamonds >= 50;
        purchaseSoulsUrnButton.interactable = StatsManager.Instance.CurrentDiamonds >= 250;
        purchaseSoulsAltarButton.interactable = StatsManager.Instance.CurrentDiamonds >= 1000;
    }

    public void PurchaseBagOfDiamonds()
    {
        StatsManager.Instance.EarnDiamonds(100);
    }

    public void PurchaseBucketOfDiamonds()
    {
        StatsManager.Instance.EarnDiamonds(600);
    }

    public void PurchaseBarrelOfDiamonds()
    {
        StatsManager.Instance.EarnDiamonds(1300);
    }

    public void PurchaseChestOfDiamonds()
    {
        StatsManager.Instance.EarnDiamonds(2800);
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

    public void PurchaseWizardBundle()
    {
        StatsManager.Instance.isSoulsBuffActive = true;
        StatsManager.Instance.EarnDiamonds(150);
        StatsManager.Instance.EarnSouls(1000);
    }

    public void PurchaseMasteryBundle()
    {
        StatsManager.Instance.isXpBuffActive = true;
        StatsManager.Instance.EarnDiamonds(600);
        StatsManager.Instance.EarnSouls(5000);
    }

    public void PurchaseKingBundle()
    {
        StatsManager.Instance.isSoulsBuffActive = true;
        StatsManager.Instance.isXpBuffActive = true;
        StatsManager.Instance.EarnDiamonds(1500);
        StatsManager.Instance.EarnSouls(20000);
    }

    public void PurchaseDoubleSoulsBuff()
    {
        StatsManager.Instance.isSoulsBuffActive = true;
    }

    public void PurchaseDoubleXPBuff()
    {
        StatsManager.Instance.isXpBuffActive = true;
    }
}