using static Engagement;

public class UnitTestTriggerController : TriggerController
{
    public event Func<Hit, TroopPerk?>? OnHit;
    public event Func<Combat, TroopPerk?>? OnCombat;

    public event Func<MovementOrders, TroopPerk?>? OnOrders;
    public event Func<MovementStep, TroopPerk?>? OnMovement;
    public event Action<Engagement.EndOfRound> OnEndOfRound = delegate{};

    public List<TroopPerk> TriggerOnStrike(Hit hit)
    {
        var triggerPerks = new List<TroopPerk>();
        if(OnHit != null)
        {
            foreach(Func<Hit, TroopPerk?> eventTrigger in OnHit.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(hit);
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

    public List<TroopPerk> TriggerOnStrike(Combat combat)
    {
        var triggerPerks = new List<TroopPerk>();
        if(OnCombat != null)
        {
            foreach(Func<Combat, TroopPerk?> eventTrigger in OnCombat.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(combat);
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

    public List<TroopPerk> TriggerOnOrders(MovementOrders orders)
    {
        var triggerPerks = new List<TroopPerk>();
        if(OnOrders != null)
        {
            foreach(Func<MovementOrders, TroopPerk?> eventTrigger in OnOrders.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(orders);
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

    public List<TroopPerk> TriggerOnMovement(MovementStep step)
    {
        var triggerPerks = new List<TroopPerk>();
        if(OnMovement != null)
        {
            foreach(Func<MovementStep, TroopPerk?> eventTrigger in OnMovement.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(step);
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

    public void TriggerOnEndOfRound(EndOfRound endOfRound)
    {
        OnEndOfRound(endOfRound);
    }
}