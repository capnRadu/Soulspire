using UnityEngine;

public class MenuButtonNotification : MonoBehaviour
{
    private enum NotificationType
    {
        Research,
        Stats
    }

    [SerializeField] private NotificationType notificationType;
    [SerializeField] private GameObject notificationIcon;

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
        bool hasAffordableStat = false;
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
                    hasAffordableStat = true;
                    break;
                }
            }
            else
            {
                if (stat.isPurchased) continue;

                if (StatsManager.Instance.CurrentSigils >= stat.definition.sigilsPurchaseCost)
                {
                    hasAffordableStat = true;
                    break;
                }
            }
        }

        notificationIcon.SetActive(hasAffordableStat);
    }
}