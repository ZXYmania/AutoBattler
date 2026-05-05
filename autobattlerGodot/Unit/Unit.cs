using Godot;
using System;

public abstract class Unit
{
	public Stats stats { get; protected set; }
	public Captain captain {get; protected set;}
}
