using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 2f;
    [SerializeField] private float defaultSpeed = 1f;

    private void Start()
    {
        if (timeSlider != null)
        {
            timeSlider.minValue = minSpeed;
            timeSlider.maxValue = maxSpeed;
            timeSlider.value = defaultSpeed;

            timeSlider.onValueChanged.AddListener(SetTimeScale);
        }

        SetTimeScale(defaultSpeed);
    }

    public void SetTimeScale(float speed)
    {
        Time.timeScale = speed;

        if (speedText != null)
        {
            speedText.text = $"{speed:F1}x";
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}