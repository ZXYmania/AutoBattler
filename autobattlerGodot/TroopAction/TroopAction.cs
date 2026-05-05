using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public enum ActionType
{
	Deploy,
	Strike,
	Push,
	Rotate
}

public interface TroopAction
{
	UnitEvent GetEvent();
}

public class Strike : TroopAction
{
	StrikeResult result;
    public Strike(Stats protagonist, Stats antagonist)
    {
       
	}

    public StrikeResult GetResult()
	{
		return result;
	}

    public UnitEvent GetEvent()
    {
        throw new NotImplementedException();
    }

	public enum StrikeResult
	{
		Head,
		Leg,
		Arm,
		Shield,
		Parry,
		Null
	}
}





