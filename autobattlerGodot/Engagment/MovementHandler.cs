using System;
using System.Collections.Generic;
using System.Linq;
using static Engagement;
using static Troop;
using static Phase;
using static Movement;
using static AffectsMovement;

public class MovementHandler
{
	IRandom rng;
	public MovementHandler(IRandom rng)
	{
		this.rng = rng;
	}

    public MovementOrders InitialiseOrder(in Order leftOrder, in Order rightOrder, PhaseType currentPhase)
	{
		MovementType leftMovement = DifferenceBetweenPhaseType(currentPhase, leftOrder.desiredPhase);
		MovementType rightMovement = DifferenceBetweenPhaseType(currentPhase, rightOrder.desiredPhase);
		int leftPriority = GetMovementPriority(leftMovement);
		int rightPriority = GetMovementPriority(rightMovement);
		if(leftPriority == rightPriority)
		{
			if(leftMovement == MovementType.Stay && rightMovement == MovementType.Stay)
			{
				return NoProtagonist(leftOrder, rightOrder, currentPhase);
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

	public MovementOrders NoProtagonist(in Order leftOrder,in Order rightOrder, PhaseType currentPhase)
	{
		return new MovementOrders{protagonistOrder = leftOrder, antagonistOrder = rightOrder, currentPhase = currentPhase};
	}

	public MovementOrders SetLeftProtagonist(Order leftOrder, Order rightOrder, PhaseType currentPhase)
	{
		leftOrder.SetProtagonist(currentPhase);
		rightOrder.SetAntagonist(currentPhase, leftOrder.movement);
		return new MovementOrders{ protagonistOrder = leftOrder, antagonistOrder = rightOrder, currentPhase = currentPhase};
	}

	public MovementOrders SetRightProtagonist(Order leftOrder, Order rightOrder, PhaseType currentPhase)
	{
		rightOrder.SetProtagonist(currentPhase);
		leftOrder.SetAntagonist(currentPhase, rightOrder.movement);
		return new MovementOrders{ protagonistOrder = rightOrder, antagonistOrder = leftOrder, currentPhase = currentPhase};
	}

	public MovementStep GetMovementResult(in TroopContext protagonist, Order protagonistOrder, in TroopContext antagonist, Order antagonistOrder, PhaseType currentPhase)
	{
		// Apply movement perks
		MovementCheck protagonistCheck = new MovementCheck();
		MovementCheck antagonistCheck = new MovementCheck();

		foreach(AffectsMovement perk in protagonist.activePerks.Values.ToList() ?? new List<TroopPerk>())
		{
			if(perk != null)
			{
				Nullable<MovementModifier> currentModifier = perk.GetMovementModifier();
				if(currentModifier.HasValue)
				{
					if(currentModifier.Value.currentPhase != currentPhase)
					{
						throw new NotImplementedException("Invaid activation phase of perk");
					}
					if(currentModifier.Value.troopId == protagonist.id)
					{
						protagonistCheck = protagonistCheck.ModifyMovementCheck(currentModifier.Value, currentPhase);
					}
					else if(currentModifier.Value.troopId == antagonist.id)
					{
						antagonistCheck = antagonistCheck.ModifyMovementCheck(currentModifier.Value, currentPhase);
					}
					else
					{
						throw new NotImplementedException("Invalid troopId of: " + currentModifier.Value.troopId + ". Valid options are: " +protagonist.id+", " +antagonist.id);
					}
				}
			}
		}

		foreach(AffectsMovement perk in antagonist.activePerks.Values.ToList() ?? new List<TroopPerk>())
		{
			Nullable<MovementModifier> currentModifier = perk.GetMovementModifier();
			if(currentModifier.HasValue)
			{
				if(currentModifier.Value.currentPhase != currentPhase)
				{
					throw new NotImplementedException("Invaid activation phase of perk");
				}

				if(currentModifier.Value.troopId == protagonist.id)
				{
					protagonistCheck = protagonistCheck.ModifyMovementCheck(currentModifier.Value, currentPhase);
				}
				else if(currentModifier.Value.troopId == antagonist.id)
				{
					antagonistCheck = antagonistCheck.ModifyMovementCheck(currentModifier.Value, currentPhase);
				}
				else
				{
					throw new NotImplementedException("Invalid troopId of: " + currentModifier.Value.troopId + ". Valid options are: " +protagonist.id+", " +antagonist.id);
				}
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
		if(antagonistOrder.effort > antagonistCheck.threshold)
		{
			resultMovement.antagonistResult = NormaliseMovement(antagonistOrder.movement);	
			resultMovement.resultPhase = AddMovementToPhaseType(currentPhase, resultMovement.antagonistResult);
		}
		else
		{
			resultMovement.antagonistResult = MovementType.Stay;
		}
		return resultMovement;
	}
}