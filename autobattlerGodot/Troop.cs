using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Perk;
using static Phase;

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
		public Stats stats;
		public List<Perk> activePerkList;

        public TroopContext(Guid id, Stats stats, List<Perk> perks) : this()
        {
            this.id = id;
            this.stats = stats;
            this.activePerkList = perks;
        }

        public bool Equals(TroopContext other)
        {
            return id == other.id;
        }

        public bool Equals(Troop other)
        {
            return id == other.context.id;
        }

        public override bool Equals([NotNullWhen(true)] object obj)
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

		public static bool operator ==(TroopContext left, TroopContext right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TroopContext left, TroopContext right)
		{
			return !left.Equals(right);
		}

		public void UpdateStats(Stats givenStats)
		{
			stats = givenStats;
		}

    }
	
	public TroopContext context {get; protected set;}
	public Captain captain {get; protected set;}
	public int position {get; protected set;}
	private List<Perk> perkPool;
	// public List<Health> health { get; protected set; }
	public Troop()
	{
		throw new NotImplementedException("Debugging only");
	}
	public Troop(Unit unit)
	{
		this.context = new TroopContext(Guid.NewGuid(), unit.stats, new List<Perk>());
		captain = unit.captain;
	}

	public float GetRange()
	{
		return 1;
	}

	public Order GetOrder(TroopContext antagonist, PhaseType currentPhase)
	{
		return captain.GetOrder(context, antagonist, currentPhase);
	}

	public void TriggerPerks(TriggerType trigger)
	{
		List<Perk> triggeredPerks = new List<Perk>();
		foreach(Perk perk in perkPool)
		{
			Perk? triggeredPerk = perk.TriggerCheck(trigger);
			if(triggeredPerk != null)
			{
				triggeredPerks.Add(triggeredPerk);
			}
		}
		// context.activePerkList.AddRange(captain.ResolveConflicts(triggeredPerks, context.activePerkList));
	}

	public bool CanStrike()
	{
		return captain.CanStrike();
	}

	public void UnitTest(Stats updateStats)
	{
		context.UpdateStats(context.stats + updateStats);
	}
 
	// public struct Health
    // {
	// 	public bool frontline;
	// 	public int column;
    //     public Stats stats;
	// 	public List<Perk> active;
	// 	public TroopPhase phase;
    // }

}