public class UnitTestTriggerController : TriggerController
{
    public event Action<MovementOrders> OnOrders = delegate{};
    public event Action<MovementStep> OnMovement = delegate{};

    public event Func<Strike, TroopPerk?>? OnStrike;

    public List<TroopPerk> TriggerOnStrike(Strike strike)
    {
        var triggerPerks = new List<TroopPerk>();
        if(OnStrike != null)
        {
            foreach(Func<Strike, TroopPerk?> eventTrigger in OnStrike.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(strike);
				if(perk != null)
				{
					triggerPerks.Add(perk);
				}
			}
            return triggerPerks;
        }
        else
        {
            throw new NotImplementedException("Strike Event must exist");
        }
    }

    public void TriggerOnOrders(MovementOrders orders)
    {
        OnOrders(orders);
    }

    public void TriggerOnMovement(MovementStep step)
    {
        OnMovement(step);
    }
}