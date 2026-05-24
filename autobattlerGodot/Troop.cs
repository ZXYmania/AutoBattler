using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Battlefield;
using static TroopPerk;
using static Phase;
using static Strike;

public partial class RenderTroop : Node
{
	private Sprite2D render;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        render = new Sprite2D();
		render.Texture = ResourceLoader.Load<Texture2D>("res://sword.png");
		render.Scale = new Vector2(0.25f, 0.25f);
		render.Position = new Vector2(50, 50);
		AddChild(render);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}

public class Troop
{
	public struct TroopContext : IEquatable<TroopContext>, IEquatable<Troop>
	{
		public Guid id;
		private Stats baseStats;
		private Stats computedStats;
		public Stats stats
		{
			get {return computedStats;} 
			set
			{
				baseStats = value;
				UpdateBuffs();
			}
		}
		
		public int stamina;
		public Dictionary<Guid, TroopPerk> activePerks;
		public IEnumerable<Squad>? frontline;
		public bool routed;

		// public Weapon
		public Armour armour;

		public TroopContext()
		{
			frontline = new List<Squad>();
			activePerks = new Dictionary<Guid, TroopPerk>();
		}

		public TroopContext(Troop troop)
		{
			this.id = troop.id;
			this.baseStats = troop.stats;
			this.stamina = troop.stamina;
			this.frontline = troop.frontline;
			this.routed = troop.routed;
			this.activePerks = troop.perkPool.Where(p => p.Value.IsActive()).ToDictionary()?? new Dictionary<Guid, TroopPerk>();
			UpdateBuffs();
		}

		public Stats GetStats()
		{
			return computedStats;
		}

		public void UpdateBuffs()
		{
			computedStats = baseStats;
			foreach(Buff buff in activePerks.Values.ToList()?? new List<TroopPerk>())
			{
				computedStats += buff.GetStats();
			}
		}

        public bool Equals(TroopContext other)
        {
            return id == other.id;
        }

        public bool Equals(Troop? other)
        {
			if(other == null) {return false;}
            return id == other.id;
        }

        public override bool Equals(object? obj)
        {
			if(obj is TroopContext && obj != null)
			{
				return Equals((TroopContext) obj);
			}
			if(obj is Troop && obj != null)
			{
				return Equals((Troop) obj);
			}
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public Squad GetOpposite(Squad givenSquad)
        {
            if(frontline == null)
			{
				throw new NotImplementedException("Frontline must be initialised");
			}
			return frontline.FirstOrDefault(t => t.column == givenSquad.column);
        }

        public static bool operator ==(TroopContext left, TroopContext right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TroopContext left, TroopContext right)
		{
			return !left.Equals(right);
		}

    }
	
	public Guid id;
	public Stats stats;
	public int stamina;
	public List<Squad> frontline;
	public List<Squad> backline;
	public bool routed;

	// public Weapon
	public Armour armour;

	public Captain captain {get; protected set;}
	public Position position;
	public Dictionary<Guid, TroopPerk> perkPool;

	public TroopContext GetContext()
	{
		return new TroopContext(this);
	}
	
	public Troop()
	{
		throw new NotImplementedException("Debugging only");
	}
	
	public Troop(Unit unit)
	{
		this.id = Guid.NewGuid();
		this.stats = unit.stats;
		this.frontline = new List<Squad>();
		this.backline = new List<Squad>();
		captain = unit.captain;
		perkPool = new Dictionary<Guid, TroopPerk>();
		TroopContext context = GetContext();
		foreach(UnitPerk unitPerk in unit.perkList)
		{
			TroopPerk perk = unitPerk.InstantiatePerk(context);
			if(perkPool.ContainsKey(perk.GetId()))
			{
				perkPool[perk.GetId()] = perkPool[perk.GetId()].ResolveClash(perk);
			}
			else
			{
				perkPool.Add(perk.GetId(), perk);
			}
		}
	}

	public float GetRange()
	{
		return 1;
	}

	public Order GetOrder(TroopContext antagonist, PhaseType currentPhase)
	{
		return captain.GetOrder(GetContext(), antagonist, currentPhase);
	}

	public void UpdateSubscription(Engagement.SubscriptionHadler subscriptionHadler)
	{
		foreach(TroopPerk perk in perkPool.Values)
		{
			perk.InitialiseSubscription(subscriptionHadler);
		}
	}

	public bool CanStrike(PhaseType currentPhase)
	{
		return captain.CanStrike();
	}

	public void TakeDamage(Strike strike)
	{
		foreach((Squad squad, StrikeOutcome outcome) in strike.outcome ?? throw new NotImplementedException("Invalid strike resolved"))
		{
			if(outcome.damaged)
			{
				frontline.Remove(squad);
				int currentGap = -1;
				Squad reinforcement = new Squad();
				foreach(Squad backline in backline)
				{
					if (currentGap == -1 || Mathf.Abs(squad.column - backline.column) < currentGap)
					{
						reinforcement = backline;
						currentGap = Mathf.Abs(squad.column - backline.column);
					}
				}

				if(!reinforcement.IsNull() && stats.formation > currentGap*200)
				{
					reinforcement.column = squad.column;
					frontline.Remove(squad);
					backline.Remove(reinforcement);
					frontline.Add(reinforcement);
				}
				else
				{
					routed = true;
				}
			}
			stats += outcome.debuffs;
		}
	}

	public void UnitTest(Stats updateStats)
	{
		stats += updateStats;
	}
 
	public struct Squad : IEquatable<Squad>
    {
		public Guid guid;
		public int column;

		public Squad(int column)
		{
			guid = Guid.NewGuid();
			this.column = column;
		}

        public bool Equals(Squad other)
        {
            return this.guid == other.guid;
        }

		public bool IsNull()
		{
			return this.guid == Guid.Empty;
		}
    }

	public enum BodyPart
	{
		Null = 0,
		Head,
		Body,
		Leg,
		Arm,
		Shield
	}

	public struct Armour
	{
		public int head;
		public int body;
		public int arm;
		public int leg;
		public int shield;

		public int GetArmourforBodyPart(BodyPart bodyPart)
		{
			switch(bodyPart)
			{
				case BodyPart.Head: return head;
				case BodyPart.Body: return body;
				case BodyPart.Arm: return arm;
				case BodyPart.Leg: return leg;
				case BodyPart.Shield: return shield;
				default: 
					throw new NotImplementedException("Invalid bodypat of: " + bodyPart);
			}
		}
	}
}