using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private Coroutine updateCoroutine;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

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