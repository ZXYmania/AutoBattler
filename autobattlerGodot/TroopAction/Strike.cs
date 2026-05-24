using System;
using System.Collections.Generic;
using static Troop;

public struct Strike : TroopAction
{
	public Guid protagonistId;
	public Guid antagonistId;

	public Dictionary<Squad, Hit> hit;

    public Dictionary<Squad,StrikeOutcome>? outcome;

    public Strike(TroopContext protagonist, TroopContext antagonist, IRandom rng)
    {
		protagonistId = protagonist.id;
		antagonistId = antagonist.id;
		hit = new Dictionary<Squad, Hit>();

		foreach(Squad squad in protagonist.frontline ?? throw new NotImplementedException("Invalid frontline for squad. Frontline must exist."))
		{
			Hit hit = new Hit();
			decimal randomNumber = rng.RandiRange(0, 100);
            hit.effort = (int)randomNumber / 10;
			StrikeChance chance = GetStrikeValues(protagonist.stats, antagonist.stats);
			
			decimal deflectionRatio = 0m;
			randomNumber = 100 - randomNumber;
			if(randomNumber < chance.head)
			{
				hit.area = BodyPart.Head;
			}
			else if(randomNumber < chance.body)
			{
				hit.area = BodyPart.Body;
				deflectionRatio = randomNumber/chance.body;
			}
			else if(randomNumber < chance.arm)
			{
				hit.area = BodyPart.Arm;
				deflectionRatio = randomNumber/chance.arm;
			}
			else if(randomNumber < chance.leg)
			{
				hit.area = BodyPart.Leg;
				deflectionRatio = randomNumber/chance.leg;
			}
			else
			{
				hit.area = BodyPart.Shield;
			}
			
			hit.deflectionRatio = deflectionRatio;
			this.hit[squad] = hit; 
		}
	}

	public bool IsProtagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new NotImplementedException("Invalid troop id of: " + troopId);
		}
		return troopId == protagonistId;
	}

	public bool IsAnttagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new NotImplementedException("Invalid troop id of: " + troopId);
		}
		return troopId == antagonistId;
	}

	public struct Hit
	{
		public BodyPart area;
		public int effort;
		public decimal deflectionRatio;
	}

	public struct StrikeChance
	{
		public int head;
		public int body;
		public int leg;
		public int arm;

		public override string ToString()
		{
			return "Head: " + head + ", body: " + body + ", leg: " + leg + ", arm: " + arm;
		}
	}

	public static StrikeChance GetStrikeValues(Stats protagonist, Stats antagonist)
	{
		float modifier = (int.Clamp(protagonist.strike - antagonist.block, -700, 800)/800f) + 1;
		StrikeChance output = new StrikeChance{
			head = (int) MathF.Floor(10*modifier),
		};
		output.body = output.head + (int) MathF.Floor(30*modifier);
		output.arm = output.body + (int) MathF.Floor(30*modifier);
		output.leg = output.arm + (int) MathF.Floor(30*modifier);
		return output;
	}

    public UnitEvent GetEvent()
    {
        throw new NotImplementedException();
    }

    public Trigger.TriggerType GetTriggerType()
    {
        return Trigger.TriggerType.OnAttackResult;
    }

    public struct StrikeOutcome
    {
        public bool damaged;
        public Stats debuffs;
		public StrikeOutcome()
		{
			debuffs.strike = 0;
			debuffs.block = 0;
			debuffs.power = 0;
			debuffs.formation = 0;
			debuffs.endurance = 0;
			damaged = false;
		}
    }
}





