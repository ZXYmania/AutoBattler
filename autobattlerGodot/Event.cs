using Godot;
using System;
using System.Collections.Generic;

public abstract class BattleEvent
{
	public abstract void OnPlay();
	public abstract void Rewind();
	public abstract String Description();
	public abstract int GetRound();
}

public abstract class UnitEvent : BattleEvent
{
	protected RenderTroop target;
	public UnitEvent(RenderTroop target)
	{
		this.target = target;
	}
}

// Move position
// Notify Perk
// Gap Change/Phase Change
// Health Change
