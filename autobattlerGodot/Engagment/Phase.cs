using System;
using static MovementHandler;
using static Movement;

public class Phase
{
    public enum PhaseType
	{
		OutOfCombat,
		Poke,
		Duel,
		Engage,
		Flee
	}

	private static int GetGapFromPhasetype(PhaseType type)
    {
        switch(type)
        {
            case PhaseType.OutOfCombat: 		return 4;
			case PhaseType.Poke:		return 3;
			case PhaseType.Duel:		return 2;
			case PhaseType.Engage:		return 1;
        }
		throw new NotImplementedException("Unimplemented Phase type of: " + type);
    }

	private static PhaseType GetPhaseTypeFromGap(int phaseGap)
	{
		switch(phaseGap)
		{
			case 4: return PhaseType.OutOfCombat;
			case 3: return PhaseType.Poke;
			case 2: return PhaseType.Duel;
			case 1: return PhaseType.Engage;
		}
		throw new NotImplementedException("Invalid phasegap of: " + phaseGap);
	}

	public static MovementType DifferenceBetweenPhaseType(PhaseType currentPhase, PhaseType desiredPhaseType)
	{
		if(currentPhase == PhaseType.Flee || desiredPhaseType == PhaseType.Flee)
		{
			return MovementType.Flee;
		}
		int phaseGap = GetGapFromPhasetype(currentPhase) - GetGapFromPhasetype(desiredPhaseType);

		switch(phaseGap)
		{
			case 3: return MovementType.Charge;
			case 2: return MovementType.March;
			case 1: return MovementType.Advance;
			case 0: return MovementType.Stay;
			case -1: return MovementType.Fallback;
			case <= -2: return MovementType.Disengage;
			default: throw new NotImplementedException("Invalid gap from phase Subtraction: " + currentPhase + ", " + desiredPhaseType + ", " + phaseGap);
		}
	}

	public static PhaseType AddMovementToPhaseType(PhaseType phase, MovementType movement)
	{
		if(movement == MovementType.Stay || phase == PhaseType.Flee)
		{
			return phase;
		}
		int phaseGap = GetGapFromPhasetype(phase);
		int movementRate = MovementToGap(movement);
		switch(phaseGap-movementRate)
		{
			case <= 1: return PhaseType.Engage;
			case 2: return PhaseType.Duel;
			case 3: return PhaseType.Poke;
			case >= 4: return PhaseType.OutOfCombat;
		}
			
	}

    public static int MovementToGap(MovementType movement)
	{
		switch(movement)
		{
			case MovementType.Flee: return -4;
			case MovementType.Disengage: return -2;
			case MovementType.Fallback: return -1;
			case MovementType.Stay: return 0;
			case MovementType.Advance: return 1;
			case MovementType.March: return 2;
			case MovementType.Charge: return 4;
		}
		throw new NotImplementedException("Invalid movement type of:" + movement);
	} 
}