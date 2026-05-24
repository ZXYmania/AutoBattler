using Godot;
using System;
using System.Collections.Generic;

public struct Unit
{
	public Stats stats;
	public Captain captain;
	public List<UnitPerk> perkList;

	public Unit(Stats stats, Captain captain, List<UnitPerk> perkList)
	{
		this.stats = stats;
		this.captain = captain;
		this.perkList = perkList;
	}
}
