using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Movement;
using static Phase;
using static Combat;
using static Troop;
using static Hit;


public class Engagement : TriggerController
{
	public event Func<Hit, TroopPerk?>? OnHit;
    public event Func<Combat, TroopPerk?>? OnCombat;
    public event Func<MovementOrders, TroopPerk?>? OnOrders;
    public event Func<MovementStep, TroopPerk?>? OnMovement;
	public event Action<EndOfRound> OnEndOfRound = delegate{};

	List<UnitEvent> tickEvent;
	public SubscriptionHadler subscriptionHadler;
	public Troop leftTroop;
	public Troop rightTroop;
	public Troop GetTroop(TroopContext context)
	{
		if(context.id == leftTroop.id)
		{
			return leftTroop;
		}
		if(context.id == rightTroop.id)
		{
			return rightTroop;
		}
		throw new InvalidTroopId(context.id, leftTroop.id, rightTroop.id);
	}

	public PhaseType currentPhase;
	public bool facing;
	public IRandom rng;

	public interface PartOfRound
	{
		enum RoundPart
		{
			Start,
			End
		}
	}
	
	public struct EndOfRound : PartOfRound
	{
		
	}

    public class SubscriptionHadler
	{
		private TriggerController context;
		public SubscriptionHadler(TriggerController context)
		{
			this.context = context;
		}

		public void UpdateMovementOrderSubscription(Func<MovementOrders, TroopPerk?> onMovementOrder, bool subscribe)
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

		public void UpdateMovementStepSubscription(Func<MovementStep, TroopPerk?> onMovement, bool subscribe)
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

		public void UpdateStrikeSubscription(Func<Combat, TroopPerk?> onStrike, bool subscribe)
		{
			if(subscribe)
			{
				context.OnCombat += onStrike;
			}
			else
			{
				context.OnCombat -= onStrike;
			}
		}

		public void UpdateOnEndOfRoundSubscription(Action<EndOfRound> onEndOfRound, bool subscribe)
		{
			if(subscribe)
			{
				context.OnEndOfRound += onEndOfRound;
			}
			else
			{
				context.OnEndOfRound -= onEndOfRound;
			}
		}
	}

	public Engagement(Troop leftTroop, Troop rightTroop, bool facing, IRandom rng)
	{
		this.leftTroop = leftTroop;
		this.rightTroop = rightTroop;
		this.facing = facing;
		this.rng = rng;
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
		List<UnitEvent> tickEvent = new List<UnitEvent>();
		HandleMovementRound();
		HandleStrikeRound();
		CleanUp();
		OnEndOfRound(new EndOfRound());
		return tickEvent;
	}

	public void HandleMovementRound()
	{
		MovementOrders movementOrder = new MovementOrders(leftTroop.GetOrder(rightTroop.GetContext(), currentPhase), rightTroop.GetOrder(leftTroop.GetContext(), currentPhase), currentPhase);
		if(movementOrder.protagonistOrder.protagonist && !movementOrder.antagonistOrder.protagonist)
		{
			if(movementOrder.protagonistOrder.troopId == leftTroop.id)
			{
				ResolveMovement(leftTroop, rightTroop, movementOrder);
			}
			else
			{
				ResolveMovement(rightTroop, leftTroop, movementOrder);
			}
		}
	}

	public void ResolveMovement(Troop protagonistTroop, Troop antagonistTroop, MovementOrders movementOrders)
	{
		MovementStep result = new MovementStep()
		{
			protagonist=protagonistTroop.id,
			protagonistResult = MovementType.Stay,
			protagonistEffort = movementOrders.protagonistOrder.effort,

			antagonistResult=MovementType.Stay,
			antagonistEffort=movementOrders.antagonistOrder.effort,
			resultPhase = currentPhase
		};

		bool protagonistMovementDone = false;
		bool antagonistMovementDone = false;

		while(!(protagonistMovementDone && antagonistMovementDone))
		{
			HandlePerks(OnOrders, movementOrders, protagonistTroop, antagonistTroop);
			MovementStep step = new MovementStep(protagonistTroop.GetContext(), movementOrders.protagonistOrder, antagonistTroop.GetContext(), movementOrders.antagonistOrder, movementOrders.currentPhase, rng);
			movementOrders.currentPhase = step.resultPhase;
			AddMovements(result.protagonistResult, step.protagonistResult);
			AddMovements(result.antagonistResult, result.antagonistResult);
			protagonistMovementDone = step.protagonistResult == MovementType.Stay || movementOrders.protagonistOrder.desiredPhase == movementOrders.currentPhase || protagonistMovementDone;
			antagonistMovementDone = step.antagonistResult == MovementType.Stay || (movementOrders.antagonistOrder.desiredPhase == movementOrders.currentPhase && protagonistMovementDone) || antagonistMovementDone;
		}
		protagonistTroop.DoMovement(movementOrders.protagonistOrder);
		antagonistTroop.DoMovement(movementOrders.antagonistOrder);

		currentPhase = movementOrders.currentPhase;
		result.resultPhase = currentPhase;
		HandlePerks(OnMovement, result, protagonistTroop, antagonistTroop);
	}

	public void HandleStrikeRound()
	{
		TroopContext leftContext = leftTroop.GetContext();
		TroopContext rightContext = rightTroop.GetContext();
		ResolveStrike(leftContext, rightContext);
		ResolveStrike(rightContext, leftContext);
	}

	public void ResolveStrike(in TroopContext protagonistContext, in TroopContext antagonistContext)
	{
		if(!protagonistContext.activePerks.ContainsKey(Attacked.id))
		{
			Hit hit = new Hit(protagonistContext, antagonistContext, currentPhase,rng);
			HandlePerks(OnHit, hit, GetTroop(protagonistContext), GetTroop(antagonistContext));
			Combat combat = new Combat(hit, protagonistContext, antagonistContext);
			HandlePerks(OnCombat, combat, GetTroop(protagonistContext), GetTroop(antagonistContext));
			Attacked perk = new Attacked(protagonistContext.id);
			perk.InitialiseSubscription(subscriptionHadler);
			Troop protagonistTroop = GetTroop(protagonistContext);
			protagonistTroop.perkPool[Attacked.id] = perk;
			protagonistTroop.DoStrike(hit);
			Troop antagonistTroop = GetTroop(antagonistContext);
			antagonistTroop.TakeDamage(combat);
		}
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
			if(perk is DebuffApplier applier)
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
					throw new InvalidTroopId(perk.GetTroop(), leftTroop.id, rightTroop.id);
				}
			}
		}

		foreach(ActionRequest request in requestedActions)
		{
			switch(request.type)
			{
				case ActionType.Strike:
					TroopContext protagonistContext = protagonist.GetContext();
					TroopContext antagonistContext = antagonist.GetContext();
					if(protagonist.id == request.protagonist)
					{
						ResolveStrike(protagonistContext, antagonistContext);
					}
					else if(antagonist.id == request.protagonist)
					{
						ResolveStrike(antagonistContext, protagonistContext);
					}
					else
					{
						throw new InvalidTroopId(request.protagonist, protagonist.id, antagonist.id);
					}
					break;
				// Repeat for Push
				// Repeat for Refresh
				// Resolve Clashes
			}
		}
	}

	public void CleanUp()
	{
		if((leftTroop.stats.endurance) + leftTroop.stamina < 0 )
		{
			leftTroop.routed = true;
		}

		if((rightTroop.stats.endurance) + rightTroop.stamina < 0 )
		{
			rightTroop.routed = true;
		}
	}
}