using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI healthText;
 
    private Coroutine updateCoroutine;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = $"{health}";

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        healthText.text = $"{health}";
        updateCoroutine = StartCoroutine(AnimateHealth(health));
    }

    private IEnumerator AnimateHealth(float targetHealth)
    {
        while (Mathf.Abs(slider.value - targetHealth) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * 10f);

            fill.color = gradient.Evaluate(slider.normalizedValue);

            yield return null;
        }

        slider.value = targetHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}