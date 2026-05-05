using Godot;
using System;

public partial class Battle : CanvasGroup
{
	RenderTroop testFriendly;
	RenderTroop testEnemy;
	// public Battle(Unit friendly, Unit Enemy)
	// {

	// }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("deploy");
		testFriendly = new RenderTroop();
		AddChild(testFriendly);
		testEnemy = new RenderTroop();
		AddChild(testEnemy);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
