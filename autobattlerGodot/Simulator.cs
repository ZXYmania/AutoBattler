using Godot;
using System;
using System.Collections.Generic;
using static Engagement;

public class Simulator
{
	private Battlefield battlefield;
	private List<BattleEvent> battle;

	public Simulator(Army left, Army right)
	{
		battle = new List<BattleEvent>();
		battlefield = new Battlefield(left, right);
		
	}
	public List<BattleEvent> Simulate()
	{
		return battle;
	}

	private void RoundSimulation()
	{
		
	}
}
