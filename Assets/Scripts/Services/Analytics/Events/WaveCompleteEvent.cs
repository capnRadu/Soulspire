public class WaveCompleteEvent : Unity.Services.Analytics.Event
{
    public WaveCompleteEvent() : base("wave_complete")
    {
    }

    public float PlayerCurrentHealth { set { SetParameter("player_current_health", value); } }
    public int WaveReached { set { SetParameter("wave_reached", value); } }
}