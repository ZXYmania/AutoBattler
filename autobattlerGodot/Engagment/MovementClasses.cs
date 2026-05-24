using System;
using System.Collections.Generic;
using static AffectsMovement;
using static Movement;
using static Phase;

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
            Charge
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

public struct MovementOrders : Trigger
{
    public Order protagonistOrder;
    public Order antagonistOrder;
    public PhaseType currentPhase;

    public Trigger.TriggerType GetTriggerType()
    {
        return Trigger.TriggerType.OnMove;
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
    Guid protagonist;
    public MovementType protagonistResult;

    public MovementType antagonistResult;
    
    public PhaseType resultPhase;

    public MovementStep() {}
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
}

