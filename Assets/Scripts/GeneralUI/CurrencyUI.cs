using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI soulsText;
    [SerializeField] private TextMeshProUGUI sigilsText;

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
        coinsText.text = StatsManager.Instance.CurrentCoins.ToString();
        soulsText.text = StatsManager.Instance.CurrentSouls.ToString();
        sigilsText.text = StatsManager.Instance.CurrentSigils.ToString();
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
        levelText.text = $"Lvl {StatsManager.Instance.CurrentLevel}";
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