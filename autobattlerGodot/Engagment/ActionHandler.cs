using System;
using System.ComponentModel;

public class ActionHandler // : IComponent
{
	public Strike DoStrike(ActionType actionType, Troop protagonist, Troop antagonist)
	{
		// Retrieve current Action if exists

		// switch(actionType)
		// {
		// 	case ActionType.Strike:
		// 		if(protagonist.CanStrike())
		// 		{
		// 			return new Strike(protagonist.stats, antagonist.stats);
		// 		}	
		// 		return null;
		// }
		throw new NotImplementedException("Invalid action type of: " + actionType);
	}
}