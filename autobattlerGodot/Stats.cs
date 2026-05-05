using Godot;
using System; 

public struct Stats : IEquatable<Stats>
{
	public int strike;
	public int power;
	public int block;
	public int formation;
	public int endurance;

	public Stats(int strike = 0, int power = 0, int block = 0, int formation = 0, int endurance = 0)
	{
		this.strike = strike;
		this.power = power;
		this.block = block;
		this.formation = formation;
		this.endurance = endurance;
	}

    public bool Equals(Stats other)
    {
		return 
		(
			block == other.block &&
			endurance == other.endurance &&
			formation == other.formation &&
			power == other.power &&
			strike == other.strike
		);
	}

    public static Stats operator +(Stats baseStat, Stats addOn)
	{
		baseStat.block += addOn.block;
		baseStat.endurance += addOn.endurance;
		baseStat.formation += addOn.formation;
		baseStat.power += addOn.power;
		baseStat.strike += addOn.strike;
		return baseStat;
	}
}