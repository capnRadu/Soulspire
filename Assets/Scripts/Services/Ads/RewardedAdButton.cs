using UnityEngine;

public class RewardedAdButton : MonoBehaviour
{
    private void OnEnable()
    {
        RewardedAdManager.Instance.OnRewardGained += HandleRewardGained;
    }

    private void OnDisable()
    {
        RewardedAdManager.Instance.OnRewardGained -= HandleRewardGained;
    }

    private void HandleRewardGained()
    {
        Debug.Log("Rewarded Ad COMPLETED - granted reward to the player");
        StatsManager.Instance.EarnDiamonds(10);
        RewardedAdManager.Instance.RewardedVideoAd.LoadAd();
    }

    private void Start()
    {
        Debug.Log("[LevelPlaySample] LoadRewardedVideo triggered");
        RewardedAdManager.Instance.RewardedVideoAd.LoadAd();
    }

    public void ShowRewardedAd()
    {
        Debug.Log("[LevelPlaySample] ShowRewardedVideoButtonClicked");
        if (RewardedAdManager.Instance.RewardedVideoAd.IsAdReady())
        {
            RewardedAdManager.Instance.RewardedVideoAd.ShowAd();
        }
        else
        {
            Debug.Log("[LevelPlaySample] LevelPlay Rewarded Video Ad is not ready");
        }
    }
}