using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Movement;
using static Phase;
using static Strike;
using static Troop;


public class Engagement : TriggerController
{
	List<UnitEvent> tickEvent;
	public MovementHandler movementHandler;
	public ActionHandler actionHandler;
	public SubscriptionHadler subscriptionHadler;
	public Troop leftTroop;
	public PhaseType currentPhase;
	public Troop rightTroop;
	public bool facing;

    public event Func<Strike, TroopPerk?>? OnStrike;


    // public event Action<Strike> OnStrike  = delegate{};
    public event Action<MovementOrders> OnOrders  = delegate{};
    public event Action<MovementStep> OnMovement  = delegate{};

    public class SubscriptionHadler
	{
		private TriggerController context;
		public SubscriptionHadler(TriggerController context)
		{
			this.context = context;
		}

		public void UpdateMovementOrderSubscription(Action<MovementOrders> onMovementOrder, bool subscribe)
		{
			if(subscribe)
			{
				context.OnOrders += onMovementOrder;
			}
			else
			{
				context.OnOrders -= onMovementOrder;
			}
		}

		public void UpdateMovementStepSubscription(Action<MovementStep> onMovement, bool subscribe)
		{
			if(subscribe)
			{
				context.OnMovement += onMovement;
			}
			else
			{
				context.OnMovement -= onMovement;
			}
		}

		public void UpdateStrikeSubscription(Func<Strike, TroopPerk?> onStrike, bool subscribe)
		{
			if(subscribe)
			{
				context.OnStrike += onStrike;
			}
			else
			{
				context.OnStrike -= onStrike;
			}
		}
	}

	public Engagement(Troop leftTroop, Troop rightTroop, bool facing, IRandom rng)
	{
		this.leftTroop = leftTroop;
		this.rightTroop = rightTroop;
		this.facing = facing;
		movementHandler = new MovementHandler(rng);
		actionHandler = new ActionHandler(rng);
		tickEvent = new List<UnitEvent>();
		subscriptionHadler = new SubscriptionHadler(this);
		foreach((Guid id, TroopPerk perk) in leftTroop.perkPool)
		{
			perk.InitialiseSubscription(subscriptionHadler);
		}
		foreach((Guid id,TroopPerk perk) in rightTroop.perkPool)
		{
			perk.InitialiseSubscription(subscriptionHadler);
		}
	}

	public List<UnitEvent> ResolveRound()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		List<UnitEvent> tickEvent = new List<UnitEvent>();
		HandleMovementRound();

		return tickEvent;
	}

	public void HandleMovementRound()
	{
		MovementOrders movementOrder = movementHandler.InitialiseOrder(leftTroop.GetOrder(rightTroop.GetContext(), currentPhase), rightTroop.GetOrder(leftTroop.GetContext(), currentPhase), currentPhase);
		if(movementOrder.protagonistOrder.protagonist || movementOrder.protagonistOrder.protagonist)
		{
			ResolveMovement(leftTroop, rightTroop, movementOrder);
		}
	}

	public void ResolveMovement(Troop protagonistTroop, Troop antagonistTroop, MovementOrders movementOrders)
	{
		bool protagonistMovementDone = false;
		bool antagonistMovementDone = false;
		while(!(protagonistMovementDone && antagonistMovementDone))
		{
			// HandlePerks(OnOrders, movementOrders, protagonistTroop, antagonistTroop);

			MovementStep result = movementHandler.GetMovementResult(protagonistTroop.GetContext(), movementOrders.protagonistOrder, antagonistTroop.GetContext(), movementOrders.antagonistOrder, currentPhase);
			currentPhase = result.resultPhase;
			protagonistMovementDone = result.protagonistResult == MovementType.Stay || movementOrders.protagonistOrder.desiredPhase == currentPhase || protagonistMovementDone;
			antagonistMovementDone = result.antagonistResult == MovementType.Stay || (movementOrders.antagonistOrder.desiredPhase == currentPhase && protagonistMovementDone) || antagonistMovementDone;
		}
	}

	public void ResolveStrike(Troop protagonist, Troop antagonist)
	{
		TroopContext protagonistContext = protagonist.GetContext();
		TroopContext antagonistContext = antagonist.GetContext();
		Strike strike = actionHandler.DoStrike(protagonistContext, antagonistContext, currentPhase);
		// TriggerPerks
		strike.outcome = actionHandler.GetOutcome(strike, protagonistContext, antagonistContext);
		HandlePerks(OnStrike, strike, protagonist, antagonist);
		antagonist.TakeDamage(strike);
		// Reform frontline
	}

	public void HandlePerks<T>(Func<T, TroopPerk?>? eventHandler, T trigger, Troop protagonist, Troop antagonist)
	{
		List<TroopPerk> triggerPerks = new List<TroopPerk>();
		if(eventHandler != null)
		{
			foreach(Func<T, TroopPerk?> eventTrigger in eventHandler.GetInvocationList())
			{
				TroopPerk? perk = eventTrigger(trigger);
				if(perk != null)
				{
					triggerPerks.Add(perk);
				}
			}
		}

		List<ActionRequest> requestedActions = new List<ActionRequest>();
		foreach(TroopPerk perk in triggerPerks ?? new List<TroopPerk>())
		{
			if(perk is ActionRequester actionRequster)
			{
				foreach(ActionRequest action in actionRequster.RequestAction())
				requestedActions.Add(action);
			}
			if( perk is DebuffApplier applier)
			{
				if(perk.GetTroop() == protagonist.id)
				{
					TroopPerk debuff = (TroopPerk) applier.GetDebuff(antagonist.id);
					antagonist.perkPool.Add(debuff.GetId(), debuff);
				}
				else if(perk.GetId() == antagonist.id)
				{
					TroopPerk debuff = (TroopPerk) applier.GetDebuff(protagonist.id);
					protagonist.perkPool.Add(debuff.GetId(), debuff);
				}
				else
				{
					throw new NotImplementedException("Must Debuff a unit that is in the engagement: " + perk.GetTroop());
				}
			}
		}

		
		// Action Result
		foreach(ActionRequest request in requestedActions.Where(t => t.type == ActionType.Strike))
		{
			if(protagonist.id == request.protagonist)
			{
				ResolveStrike(protagonist, antagonist);
			}
			else
			{
				ResolveStrike(antagonist, protagonist);
			}
		}

		// Repeat for Push
		// Repeat for Refresh
		// Resolve Clashes

	}
}