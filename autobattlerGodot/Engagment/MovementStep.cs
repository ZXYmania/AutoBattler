using System;
using System.Collections.Generic;
using System.Linq;
using static AffectsMovement;
using static Movement;
using static Phase;
using static Troop;

public class Movement
{
    public enum MovementType
        {
            Flee,
            Fallback,
            Disengage,
            Stay,
            March,
            Advance,
            Charge,
            Null
        }

        public static MovementType NormaliseMovement(MovementType movement)
        {
            switch(movement)
            {
                case MovementType.Charge:
                case MovementType.March:
                case MovementType.Advance:
                    return MovementType.Advance;
                case MovementType.Flee:
                case MovementType.Disengage:
                case MovementType.Fallback:
                    return MovementType.Fallback;
                case MovementType.Stay:
                    return MovementType.Stay;
                default:
                    throw new NotImplementedException("Invalid movement type of: " + movement);
            }
        }

        public static int GetMovementPriority(MovementType movement)
        {
            switch(movement)
            {
                case MovementType.Flee:
                    return 1;
                case MovementType.Charge:
                    return 2;
                case MovementType.March:
                    return 3;
                case MovementType.Advance:
                    return 4;
                case MovementType.Disengage:
                    return 5;
                case MovementType.Fallback:
                    return 6;
                case MovementType.Stay:
                    return 7;
            };
            
            throw new NotImplementedException("Invalid Movement type of " + movement);
        }
}

public struct MovementCheck
{
    public MovementCheck(MovementType movement, PhaseType currentPhase)
    {
        threshold = 10;
        // outcomePhase = null;
    }
    public int threshold;
    public Nullable<bool> succeeded;
    // public Nullable<PhaseType> outcomePhase;

    public MovementCheck ModifyMovementCheck(MovementModifier modifier, PhaseType currentPhase)
	{
        
		switch(modifier.type)
		{
			case MovementModifierType.Additive:
				threshold += modifier.value;
				return this;
			case MovementModifierType.Multiplicative:
				threshold *= modifier.value;
				return this;
			case MovementModifierType.Override:
                if(succeeded.HasValue && succeeded.Value != false)
                {
                    succeeded = modifier.overrideSuccess;
                }
				succeeded = succeeded == false;
				return this;
			default:
				throw new NotImplementedException("Invalid movement modifier type of: " + modifier.type);
		}
	}
}
	
public struct MovementStep : Trigger
{    
    public Guid protagonist;
    public MovementType protagonistResult;
    public int protagonistEffort;

    public MovementType antagonistResult;
    public int antagonistEffort;
    
    public PhaseType resultPhase;

    public MovementStep(Guid protagonist, MovementType protagonistReslt, MovementType antagonistResult, PhaseType resultPhase)
    {
        this.protagonist = protagonist;
        this.protagonistResult = protagonistReslt;
        this.antagonistResult = antagonistResult;
        this.resultPhase = resultPhase;
    }

    public Trigger.TriggerType GetTriggerType()
    {
        return Trigger.TriggerType.OnMoveResult;
    }

    public MovementStep(in TroopContext protagonist, in Order protagonistOrder, in TroopContext antagonist, in Order antagonistOrder, PhaseType currentPhase, IRandom rng)
	{
		MovementCheck protagonistCheck = new MovementCheck();
		MovementCheck antagonistCheck = new MovementCheck();

		foreach(TroopPerk perk in protagonist.activePerks.Values.ToList() ?? new List<TroopPerk>())
		{
			if(perk is AffectsMovement movementPerk)
			{
				Nullable<MovementModifier> currentModifier = movementPerk.GetMovementModifier();
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
						throw new InvalidTroopId(currentModifier.Value.troopId, protagonist.id, antagonist.id);
					}
				}
			}
		}

		foreach(TroopPerk perk in antagonist.activePerks.Values.ToList() ?? new List<TroopPerk>())
		{
			if(perk is AffectsMovement movementPerk)
			{
				Nullable<MovementModifier> currentModifier = movementPerk.GetMovementModifier();
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
						throw new InvalidTroopId(currentModifier.Value.troopId, protagonist.id, antagonist.id);
					}
				}
			}
		}
		
		resultPhase = currentPhase;
		if(protagonistOrder.effort > protagonistCheck.threshold)
		{
			protagonistResult = NormaliseMovement(protagonistOrder.movement);
			protagonistEffort = protagonistCheck.threshold;
			resultPhase = AddMovementToPhaseType(resultPhase, protagonistResult);
		}
		else
		{
			protagonistResult = MovementType.Stay;
		}
		if(antagonistOrder.effort > antagonistCheck.threshold)
		{
			antagonistResult = NormaliseMovement(antagonistOrder.movement);	
			antagonistEffort = antagonistCheck.threshold;
			resultPhase = AddMovementToPhaseType(resultPhase, antagonistResult);
		}
		else
		{
			antagonistResult = MovementType.Stay;
		}
	}
}

