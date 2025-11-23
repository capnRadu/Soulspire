using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI soulsText;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
        UpdateCurrencyDisplay();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay()
    {
        coinsText.text = StatsManager.Instance.CurrentCoins.ToString();
        soulsText.text = StatsManager.Instance.CurrentSouls.ToString();
    }
}