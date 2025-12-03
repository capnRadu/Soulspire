using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI soulsText;
    [SerializeField] private TextMeshProUGUI sigilsText;
    [SerializeField] private TextMeshProUGUI diamondsText;

    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;

    private Coroutine updateCoroutine;

    private void OnEnable()
    {
        StatsManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
        StatsManager.Instance.OnXPChange += UpdateXPDisplay;
        StatsManager.Instance.OnLevelUp += UpdateLevelDisplay;

        UpdateCurrencyDisplay();
        UpdateXPDisplay();
        UpdateLevelDisplay();
    }

    private void OnDisable()
    {
        StatsManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
        StatsManager.Instance.OnXPChange -= UpdateXPDisplay;
        StatsManager.Instance.OnLevelUp -= UpdateLevelDisplay;
    }

    private void UpdateCurrencyDisplay()
    {
        if (coinsText != null)
        {
            coinsText.text = StatsManager.Instance.CurrentCoins.ToString();
        }

        if (soulsText != null)
        {
            soulsText.text = StatsManager.Instance.CurrentSouls.ToString();
        }

        if (sigilsText != null)
        {
            sigilsText.text = StatsManager.Instance.CurrentSigils.ToString();
        }

        if (diamondsText != null)
        {
            diamondsText.text = StatsManager.Instance.CurrentDiamonds.ToString();
        }
    }

    private void UpdateXPDisplay()
    {
        float current = StatsManager.Instance.CurrentXP;
        float req = StatsManager.Instance.RequiredXP;

        experienceText.text = $"{Mathf.FloorToInt(current)} / {Mathf.FloorToInt(req)}";
        float targetXP = current / req;

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(AnimateExperience(targetXP));
    }

    private void UpdateLevelDisplay()
    {
        levelText.text = $"{StatsManager.Instance.CurrentLevel}";
    }

    private IEnumerator AnimateExperience(float targetXP)
    {
        while (Mathf.Abs(xpSlider.value - targetXP) > 0.01f)
        {
            xpSlider.value = Mathf.Lerp(xpSlider.value, targetXP, Time.deltaTime * 10f);

            yield return null;
        }

        xpSlider.value = targetXP;
    }
}