using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button veryHighButton;
    [SerializeField] private Button highButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button lowButton;
    [SerializeField] private Button veryLowButton;

    [SerializeField] private AudioSource popUpOpenSound;
    [SerializeField] private AudioSource popUpCloseSound;

    private void OnEnable()
    {
        popUpOpenSound.Play();

        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                veryLowButton.interactable = false;
                break;
            case 1:
                lowButton.interactable = false;
                break;
            case 2:
                mediumButton.interactable = false;
                break;
            case 3:
                highButton.interactable = false;
                break;
            case 4:
                veryHighButton.interactable = false;
                break;
        }

        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        popUpCloseSound.Play();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        foreach (Button button in new Button[] { veryLowButton, lowButton, mediumButton, highButton, veryHighButton })
        {
            button.interactable = true;
        }

        switch (qualityIndex)
        {
            case 0:
                veryLowButton.interactable = false;
                break;
            case 1:
                lowButton.interactable = false;
                break;
            case 2:
                mediumButton.interactable = false;
                break;
            case 3:
                highButton.interactable = false;
                break;
            case 4:
                veryHighButton.interactable = false;
                break;
        }
    }
}