using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static Engagement;
using static Troop;
using static Phase;
using static Movement;

public class MovementHandler // : IComponent 
{

    public Tuple<Order, Order> InitialiseOrder(Order leftOrder, Order rightOrder, PhaseType currentPhase)
	{
		MovementType leftMovement = DifferenceBetweenPhaseType(currentPhase, leftOrder.desiredPhase);
		MovementType rightMovement = DifferenceBetweenPhaseType(currentPhase, rightOrder.desiredPhase);
		int leftPriority = GetMovementPriority(leftMovement);
		int rightPriority = GetMovementPriority(rightMovement);
		
		if(leftPriority == rightPriority)
		{
			if(leftMovement == MovementType.Stay && rightMovement == MovementType.Stay)
			{
				return new Tuple<Order, Order>(leftOrder, rightOrder);
			}
			if(leftOrder.effort > rightOrder.effort)
			{
				return SetLeftProtagonist(leftOrder, rightOrder, currentPhase);
			}
			else
			{
				return SetRightProtagonist(leftOrder, rightOrder, currentPhase);
			}
		}
		else if (leftPriority < rightPriority)
		{
			return SetLeftProtagonist(leftOrder, rightOrder, currentPhase);
		}
		else
		{
			return SetRightProtagonist(leftOrder, rightOrder, currentPhase);
		}
	}

	public Tuple<Order,Order> SetLeftProtagonist(Order leftOrder, Order rightOrder, PhaseType currentPhase)
	{
		leftOrder.SetProtagonist(currentPhase);
		rightOrder.SetAntagonist(currentPhase, leftOrder.movement);
		return new Tuple<Order,Order>(leftOrder, rightOrder);
	}

	public Tuple<Order,Order> SetRightProtagonist(Order leftOrder, Order rightOrder, PhaseType currentPhase)
	{
		rightOrder.SetProtagonist(currentPhase);
		leftOrder.SetAntagonist(currentPhase, rightOrder.movement);
		return new Tuple<Order,Order>(leftOrder, rightOrder);
	}

	public MovementStep GetMovementResult(TroopContext protagonist, Order protagonistOrder, TroopContext antagonist, Order antagonistOrder, PhaseType currentPhase)
	{
		// Apply movement perks
		MovementCheck protagonistCheck = new MovementCheck();
		MovementCheck antagonistCheck = new MovementCheck();

		IEnumerable<AffectsMovement> protagonistMovements = protagonist.activePerkList.OfType<AffectsMovement>();
		IEnumerable<AffectsMovement> antagonistMovements = antagonist.activePerkList.OfType<AffectsMovement>();

		foreach(AffectsMovement perk in protagonistMovements)
		{
			if(perk.GetActivationPhase() != currentPhase)
			{
				throw new NotImplementedException("Invaid activation phase of perk");
			}

			switch(perk.MovementAppliedTo())
			{
				case ActorType.protagonist:
					protagonistCheck = protagonistCheck.ModifyMovementCheck(perk.GetMovementModifier(), currentPhase);
					break;
				case ActorType.antagonist:
					antagonistCheck = antagonistCheck.ModifyMovementCheck(perk.GetMovementModifier(), currentPhase);
					break;
				default:
					throw new NotImplementedException("Invalid actor type of: " +perk.MovementAppliedTo());
			}
		}
		foreach(AffectsMovement perk in antagonistMovements)
		{
			if(perk.GetActivationPhase() != currentPhase)
			{
				throw new NotImplementedException("Invaid activation phase of perk");
			}

			switch(perk.MovementAppliedTo())
			{
				case ActorType.protagonist:
					protagonistCheck = protagonistCheck.ModifyMovementCheck(perk.GetMovementModifier(), currentPhase);
					break;
				case ActorType.antagonist:
					antagonistCheck = antagonistCheck.ModifyMovementCheck(perk.GetMovementModifier(), currentPhase);
					break;
				default:
					throw new NotImplementedException("Invalid actor type of: " + perk.MovementAppliedTo());
			}
		}
		MovementStep resultMovement = new MovementStep();
		resultMovement.resultPhase = currentPhase;
		if(protagonistOrder.effort > protagonistCheck.threshold)
		{
			resultMovement.protagonistResult = NormaliseMovement(protagonistOrder.movement);
			resultMovement.resultPhase = AddMovementToPhaseType(currentPhase, resultMovement.protagonistResult);
		}
		else
		{
			resultMovement.protagonistResult = MovementType.Stay;
		}
		if(protagonistOrder.effort > protagonistCheck.threshold)
		{
			resultMovement.antagonistResult = NormaliseMovement(antagonistOrder.movement);	
			resultMovement.resultPhase = AddMovementToPhaseType(currentPhase, resultMovement.protagonistResult);
		}
		else
		{
			resultMovement.antagonistResult = MovementType.Stay;
		}
		return resultMovement;
	}


    public void HandlePhaseChange()
    {					
		// MovementStep movementDone = new MovementStep(MovementType.NoEffect, MovementType.NoEffect);
		// if(protagonistPerk != null)
		// {
		// 	ActionType protagonistActionToDo = protagonistPerk.OnMovement(protagonistOrder, antagonistOrder);
        //     // Send Action Event
		// 	// TroopAction protagonistAction = DoAction(context, protagonistActionToDo);
		// 	// movementDone = CombineMovementStep(movementDone, protagonistPerk.GetGap(protagonistAction));
		// }
		// if(antagonistPerk != null)
		// {
		// 	ActionType antagonistToDo = antagonistPerk.OnMovement(protagonistOrder, antagonistOrder);
		// 	// TroopAction antagonistAction = DoAction(), antagonistToDo);
		// 	// movementDone = CombineMovementStep(movementDone, antagonistPerk.GetGap(antagonistAction));
		// }

		// get results
			// compare stats
    }
}