public class SkinPurchasedEvent : Unity.Services.Analytics.Event
{
    public SkinPurchasedEvent() : base("skin_purchased")
    {
    }

    public string SkinName { set { SetParameter("skin_name", value); } }
    public int PlayerLevel { set { SetParameter("player_level", value); } }
}