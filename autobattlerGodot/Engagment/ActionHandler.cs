using System;
using System.Collections.Generic;
using static Phase;
using static Strike;
using static Troop;

public class ActionHandler
{
	IRandom rng;
	public ActionHandler(IRandom rng)
	{
		this.rng = rng;
	}

	public Strike DoStrike(TroopContext protagonist, TroopContext antagonist, PhaseType currentPhase)
	{		
		return new Strike(protagonist, antagonist, rng);
	}

	public Dictionary<Squad,StrikeOutcome> GetOutcome(in Strike strike, TroopContext protagonistContext, TroopContext antagonistContext)
	{
		Dictionary<Squad, StrikeOutcome> result = new Dictionary<Squad, StrikeOutcome>();
		foreach((Squad protagonistSquad, Hit hit) in strike.hit)
		{
			StrikeOutcome outcome = new StrikeOutcome();
			int strengthDeflection = int.Clamp((int) decimal.Floor(protagonistContext.stats.power * hit.deflectionRatio), 100, 600);
			switch(hit.area)
			{
				case BodyPart.Head:
							outcome.damaged = true;
							break;
				case BodyPart.Body:
							if(strengthDeflection > antagonistContext.armour.GetArmourforBodyPart(BodyPart.Body))
							{
								outcome.damaged = true;
							}
							else
							{
								outcome.debuffs.endurance -= (int)decimal.Floor(hit.deflectionRatio * 5);
							}
							break;
				case BodyPart.Arm:
							outcome.debuffs.block -= (int)decimal.Floor(hit.deflectionRatio * 5);
							break;
				case BodyPart.Leg:
							outcome.debuffs.formation -= (int)decimal.Floor(hit.deflectionRatio * 5);
							break;
				case BodyPart.Shield:
							break;
				default: throw new NotImplementedException("Invalid hit area of: " + hit.area);
			}
			result.Add(antagonistContext.GetOpposite(protagonistSquad), outcome);
		}
		return result;
	}
}