public class PlayerDeathEvent : Unity.Services.Analytics.Event
{
    public PlayerDeathEvent() : base("player_death")
    {
    }

    public int WaveReached { set { SetParameter("wave_reached", value); } }
    public int SoulsGained { set { SetParameter("souls_gained", value); } }
}