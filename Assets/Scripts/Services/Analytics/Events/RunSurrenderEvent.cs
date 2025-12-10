public class RunSurrenderEvent : Unity.Services.Analytics.Event
{
    public RunSurrenderEvent() : base("run_surrender")
    {
    }

    public int WaveReached { set { SetParameter("wave_reached", value); } }
    public int SoulsGained { set { SetParameter("souls_gained", value); } }
}