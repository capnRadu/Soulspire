public class StatUnlockedEvent : Unity.Services.Analytics.Event
{
    public StatUnlockedEvent() : base("stat_unlocked")
    {
    }

    public string StatName { set { SetParameter("stat_name", value); } }
    public int PlayerLevel { set { SetParameter("player_level", value); } }
    public int CurrentSigils { set { SetParameter("current_sigils", value); } }
}