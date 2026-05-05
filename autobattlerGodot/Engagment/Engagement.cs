using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Movement;
using static Phase;


public class Engagement
{
	public delegate TroopAction RequestAction(Guid protagonist, ActionType actionType);
	List<UnitEvent> tickEvent;
	public MovementHandler movementHandler;
	public ActionHandler actionHandler;
	public Troop leftTroop;
	public PhaseType currentPhase;
	public Troop rightTroop;

	public bool facing;

	public enum ActorType
	{
		protagonist,
		antagonist
	}

	public Engagement(Troop leftTroop, Troop rightTroop, bool facing)
	{
		this.leftTroop = leftTroop;
		this.rightTroop = rightTroop;
		this.facing = facing;
		movementHandler = new MovementHandler();
		actionHandler = new ActionHandler();
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
		Tuple<Order, Order> movementOrder = movementHandler.InitialiseOrder(leftTroop.GetOrder(rightTroop.context, currentPhase), rightTroop.GetOrder(leftTroop.context, currentPhase), currentPhase);

		if(movementOrder.Item1.protagonist)
		{
			ResolveMovement(leftTroop, movementOrder.Item1, rightTroop, movementOrder.Item2);
		}
		if(movementOrder.Item2.protagonist)
		{
			ResolveMovement(leftTroop, movementOrder.Item1, rightTroop, movementOrder.Item2);
		}
	}

	public void ResolveMovement(Troop protagonistTroop, Order protagonistOrder, Troop antagonistTroop, Order antagonistOrder)
	{
		bool protagonistMovementDone = false;
		bool antagonistMovementDone = false;
		while(!(protagonistMovementDone && antagonistMovementDone))
		{
			HandlePerks(new MovementStartTrigger(    
				new HashSet<PhaseType>() {currentPhase},
				new HashSet<MovementType>() {protagonistOrder.movement},
				new HashSet<MovementType>() {antagonistOrder.movement}
			), protagonistTroop, antagonistTroop);
			antagonistTroop.TriggerPerks(
				new MovementStartTrigger(    
				new HashSet<PhaseType>() {currentPhase},
				new HashSet<MovementType>() {antagonistOrder.movement},
				new HashSet<MovementType>() {protagonistOrder.movement}
			));

			MovementStep result = movementHandler.GetMovementResult(protagonistTroop.context, protagonistOrder, antagonistTroop.context, antagonistOrder, currentPhase);
			currentPhase = result.resultPhase;
			protagonistMovementDone = result.protagonistResult == MovementType.Stay || protagonistOrder.desiredPhase == currentPhase || protagonistMovementDone;
			antagonistMovementDone = result.antagonistResult == MovementType.Stay || (antagonistOrder.desiredPhase == currentPhase && protagonistMovementDone) || antagonistMovementDone;
		}
	}

	public void HandlePerks(TriggerType trigger, Troop protagonist, Troop antagonist)
	{
		// Trigger
		protagonist.TriggerPerks(trigger);
		// Request Action
		HashSet<ActionType> requestedActions = new HashSet<ActionType>();
		foreach(Perk perk in protagonist.context.activePerkList)
		{
			if(perk is ActionRequester actionRequster)
			{
				requestedActions.Add(actionRequster.RequestAction());
			}
		}
		// Action Result
		if(requestedActions.Contains(ActionType.Strike))
		{
			Strike strike = actionHandler.DoStrike(ActionType.Strike, protagonist, antagonist);
			foreach(Perk perk in protagonist.context.activePerkList)
			{
				if(perk is RequiresAction<Strike> requiresAction)
				{
					requiresAction.ReceiveResults(strike);
				}
			}
			// Repeat for Push
			// Repeat for Refresh
		}
		// Resolve Clashes
		IEnumerable<IGrouping<Guid,Perk>> duplicates = protagonist.context.activePerkList.GroupBy(x => x.GetId()).Where( group => group.Count() > 1);
		foreach(IGrouping<Guid, Perk> group in duplicates)
		{
			Perk.ResolveClash(group);
		}
	}
}