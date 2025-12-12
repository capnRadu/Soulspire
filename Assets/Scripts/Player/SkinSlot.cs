using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSlot : MonoBehaviour
{
    private RuntimeSkinData skinData;
    private SkinManager skinsManager;
    private SkinsMenuManager menuManager;

    [SerializeField] private TextMeshProUGUI skinName;
    [SerializeField] private Image icon;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Button previewButton;
    [SerializeField] private TextMeshProUGUI preview;
    [SerializeField] private AudioSource buySound;
    [SerializeField] private AudioSource selectSound;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateUI;

        if (skinData != null)
        {
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    public void Initialize(RuntimeSkinData skin, SkinManager skinsManager, SkinsMenuManager menuManager)
    {
        skinData = skin;
        this.skinsManager = skinsManager;
        this.menuManager = menuManager;
        icon.sprite = skin.skinDefinition.icon;
        skinName.text = skin.skinDefinition.skinName;

        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        previewButton.onClick.AddListener(OnPreviewClicked);

        UpdateUI();
    }

    public void UpdateUI()
    {
        int costAmount = skinData.skinDefinition.cost;
        bool canAfford = StatsManager.Instance.CurrentDiamonds >= costAmount;

        if (skinData.isUnlocked)
        {
            cost.text = "Owned";
            purchaseButton.interactable = false;
        }
        else
        {
            cost.text = costAmount.ToString();
            purchaseButton.interactable = canAfford;
        }

        RuntimeSkinData equipped = skinsManager.EquippedSkin;
        RuntimeSkinData currentPreview = menuManager.CurrentPreview;

        bool isThisEquipped = (equipped == skinData);
        bool isThisPreviewing = (currentPreview == skinData);

        if (isThisEquipped)
        {
            // This is the skin I own and have equipped
            preview.text = "Equipped";
            previewButton.interactable = false;
        }
        else if (isThisPreviewing)
        {
            // I am looking at this skin right now (but it's not equipped)
            preview.text = "Viewing";
            previewButton.interactable = false;
        }
        else if (skinData.isUnlocked)
        {
            // I own this, it's not equipped, and I'm not looking at it
            preview.text = "Equip";
            previewButton.interactable = true;
        }
        else
        {
            // I don't own this, and I'm not looking at it
            preview.text = "View";
            previewButton.interactable = true;
        }
    }

    private void OnPurchaseClicked()
    {
        int costAmount = skinData.skinDefinition.cost;
        StatsManager.Instance.SpendDiamonds(costAmount);

        skinsManager.UnlockSkin(skinData);
        menuManager.EquipSkin(skinData);

        buySound.Play();

        string skinName = skinData.skinDefinition.skinName;
        int playerLevel = StatsManager.Instance.CurrentLevel;

        AnalyticsEventsManager.Instance.RecordSkinPurchasedEvent(skinName, playerLevel);
    }

    private void OnPreviewClicked()
    {
        if (skinData.isUnlocked)
        {
            menuManager.EquipSkin(skinData);
        }
        else
        {
            menuManager.PreviewSkin(skinData);
        }

        selectSound.Play();
    }
}