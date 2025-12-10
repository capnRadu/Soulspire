public class StatUpgradeEvent : Unity.Services.Analytics.Event
{
    public StatUpgradeEvent() : base("stat_upgrade")
    {
    }

    public string StatName { set { SetParameter("stat_name", value); } }
    public string StatType { set { SetParameter("stat_type", value); } }
    public int StatLevelReached { set { SetParameter("stat_level_reached", value); } }
}