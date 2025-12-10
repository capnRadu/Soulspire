public class RunStartEvent : Unity.Services.Analytics.Event
{
    public RunStartEvent() : base("run_start")
    {
    }

    public int CurrentSouls { set { SetParameter("current_souls", value); } }
    public int PlayerLevel { set { SetParameter("player_level", value); } }
}