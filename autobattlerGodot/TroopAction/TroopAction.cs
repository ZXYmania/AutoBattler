using System;

public struct ActionRequest
{
	public ActionType type;
	public Guid protagonist;
}

public enum ActionType
{
	Deploy,
	Strike,
	Push,
	Rotate
}

public interface TroopAction : Trigger
{
	UnitEvent GetEvent();
}

