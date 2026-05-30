using System;
using System.Collections.Generic;
using static Engagement;
using static Hit;
using static Phase;
using static Troop;

public struct Hit
{
	public Guid protagonistId;
	public Guid antagonistId;
	public PhaseType currentPhase;

	public Dictionary<Squad, Strike> strikeList;
	
	public Hit(TroopContext protagonist, TroopContext antagonist, PhaseType currentPhase, IRandom rng)
    {
		protagonistId = protagonist.id;
		antagonistId = antagonist.id;
		this.currentPhase = currentPhase;
		strikeList = new Dictionary<Squad, Strike>();
		StrikeChance chance = GetStrikeValues(protagonist.stats, antagonist.stats);

		foreach(Squad squad in protagonist.frontline ?? throw new NotImplementedException("Invalid frontline for squad. Frontline must exist."))
		{
			Strike hit = new Strike();
			decimal randomNumber = rng.RandiRange(0, 100);
            hit.effort = ((int)randomNumber / 10)+1;
			
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
			this.strikeList[squad] = hit; 
		}
	}


	public struct Strike
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
		decimal modifier = (int.Clamp(protagonist.strike - antagonist.block, -700, 800)/800m) + 1m;
		StrikeChance output = new StrikeChance{
			head = (int) Decimal.Floor(10*modifier),
		};
		output.body = output.head + (int) Decimal.Floor(30*modifier);
		output.arm = output.body + (int) Decimal.Floor(30*modifier);
		output.leg = output.arm + (int) Decimal.Floor(30*modifier);
		return output;
	}

	public bool IsProtagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new InvalidTroopId(troopId, protagonistId, antagonistId);
		}
		return troopId == protagonistId;
	}

	public bool IsAnttagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new InvalidTroopId(troopId, protagonistId, antagonistId);
		}
		return troopId == antagonistId;
	}

}

public struct Combat : TroopAction
{
	public Guid protagonistId;
	public Guid antagonistId;

	public Dictionary<Squad, Strike> strikeList;
    public Dictionary<Squad, StrikeOutcome> outcomeList;

	public Combat(Hit hit, TroopContext protagonist, TroopContext antagonist)
	{
		protagonistId =protagonist.id;
		antagonistId = antagonist.id;
		strikeList = hit.strikeList;
		outcomeList = new Dictionary<Squad, StrikeOutcome>();

		foreach((Squad protagonistSquad, Strike strike) in strikeList)
		{
			StrikeOutcome outcome = new StrikeOutcome();
			int strengthDeflection = (int)decimal.Floor(protagonist.stats.power * strike.deflectionRatio);
			switch(strike.area)
			{
				case BodyPart.Head:
							outcome.damaged = true;
							break;
				case BodyPart.Body:
							if(strengthDeflection > antagonist.armour.GetArmourforBodyPart(BodyPart.Body))
							{
								outcome.damaged = true;
							}
							else
							{
								outcome.debuffs.endurance -= (int)decimal.Floor(strike.deflectionRatio * 5);
							}
							break;
				case BodyPart.Arm:
							outcome.debuffs.block -= (int)decimal.Floor(strike.deflectionRatio * 5);
							break;
				case BodyPart.Leg:
							outcome.debuffs.formation -= (int)decimal.Floor(strike.deflectionRatio * 5);
							break;
				case BodyPart.Shield:
							break;
				default: throw new NotImplementedException("Invalid hit area of: " + strike.area);
			}
			outcomeList.Add(antagonist.GetOpposite(protagonistSquad), outcome);
		}
	}


	public bool IsProtagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new InvalidTroopId(troopId, protagonistId, antagonistId);
		}
		return troopId == protagonistId;
	}

	public bool IsAnttagonist(Guid troopId)
	{
		if(protagonistId != troopId && antagonistId != troopId)
		{
			throw new InvalidTroopId(troopId, protagonistId, antagonistId);
		}
		return troopId == antagonistId;
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

public class Attacked : TroopPerk
{
	public static Guid id = Guid.NewGuid();
	Guid troopId;
	bool active;
	SubscriptionHadler subscriber;
	public Attacked(Guid troopId)
	{
		this.troopId = troopId;
		active = true;
	}

    public void Dispose()
    {
        subscriber.UpdateOnEndOfRoundSubscription(Trigger, false);
    }

    public bool Equals(TroopPerk? other)
    {
		if(other == null)
		{
			return false;
		}
        return id == other.GetId();
    }

    public Guid GetId()
    {
        return id;
    }

    public Guid GetTroop()
    {
        return troopId;
    }

	public void Trigger(EndOfRound endOfRound)
	{
		active = false;
	}

    public void InitialiseSubscription(Engagement.SubscriptionHadler subscriber)
    {
		this.subscriber = subscriber;
    	this.subscriber.UpdateOnEndOfRoundSubscription(Trigger, true);
    }

    public bool IsActive()
    {
        return active;
    }

    public TroopPerk ResolveClash(TroopPerk otherPerk)
    {
		if(otherPerk is Attacked && otherPerk.IsActive())
		{
			return otherPerk;
		}
		return this;
    }
}





