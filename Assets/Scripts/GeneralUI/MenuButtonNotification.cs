using UnityEngine;

public class MenuButtonNotification : MonoBehaviour
{
    private enum NotificationType
    {
        Research,
        Stats,
        Skins
    }

    [SerializeField] private NotificationType notificationType;
    [SerializeField] private GameObject notificationIcon;
    [SerializeField] private SkinManager skinManager;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        bool hasAffordableThing = false;

        if (notificationType == NotificationType.Skins)
        {
            if (skinManager != null)
            {
                foreach (var skin in skinManager.GetAllSkins())
                {
                    if (skin.isUnlocked) continue;

                    if (StatsManager.Instance.CurrentDiamonds >= skin.skinDefinition.cost)
                    {
                        hasAffordableThing = true;
                        break;
                    }
                }
            }
        }
        else
        {
            int currentLevel = StatsManager.Instance.CurrentLevel;

            foreach (RuntimeStat stat in StatsManager.Instance.GetAllRuntimeStats())
            {
                if (!stat.IsUnlocked(currentLevel))
                {
                    continue;
                }

                if (notificationType == NotificationType.Research)
                {
                    if (!stat.isPurchased) continue;
                    if (stat.IsMaxedOut()) continue;

                    if (StatsManager.Instance.CurrentSouls >= stat.GetSoulCost())
                    {
                        hasAffordableThing = true;
                        break;
                    }
                }
                else if (notificationType == NotificationType.Stats)
                {
                    if (stat.isPurchased) continue;

                    if (StatsManager.Instance.CurrentSigils >= stat.definition.sigilsPurchaseCost)
                    {
                        hasAffordableThing = true;
                        break;
                    }
                }
            }
        }

        if (notificationIcon != null)
        {
            notificationIcon.SetActive(hasAffordableThing);
        }
    }
}