using TMPro;
using UnityEngine;

public class RewardsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsReward;
    [SerializeField] private TextMeshProUGUI soulsReward;
    [SerializeField] private TextMeshProUGUI xpReward;

    public void Initialize(int coins, int souls, float xp)
    {
        if (coins == 0)
        {
            coinsReward.gameObject.SetActive(false);
        }
        else
        {
            coinsReward.text = $"+{coins}";
        }

        if (souls == 0)
        {
            soulsReward.gameObject.SetActive(false);
        }
        else
        {
            soulsReward.text = $"+{souls}";
        }

        if (xp == 0)
        {
            xpReward.gameObject.SetActive(false);
        }
        else
        {
            xpReward.text = $"+{xp}";
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}