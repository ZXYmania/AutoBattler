// using Godot;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// public class TroopPhase
// {
// 	public PhaseType phaseType {get;}
// 	public int effort {get;}
	
// 	public void OnPhaseEnter()
//     {
        
//     }
//     public void OnPhaseExit()
//     {
        
//     }
	
// 	public static Tuple<PhaseAction, PhaseAction> GetActions(PhaseType type)
//     {
//         switch(type)
//         {
//             case PhaseType.Engage:
// 				return new Tuple<PhaseAction, PhaseAction>(PhaseAction.Engage, PhaseAction.Engage);
// 			case PhaseType.Duel:
// 				return new Tuple<PhaseAction, PhaseAction>(PhaseAction.Strike, PhaseAction.Strike);
// 			case PhaseType.WithStand:
// 				return new Tuple<PhaseAction, PhaseAction>(PhaseAction.Hold, PhaseAction.Strike);
// 			case PhaseType.Poke:
// 				return new Tuple<PhaseAction, PhaseAction>(PhaseAction.Strike, PhaseAction.Hold);
// 			case PhaseType.Wait: 
// 				return new Tuple<PhaseAction, PhaseAction>(PhaseAction.Hold, PhaseAction.Hold);
//         }
// 		throw new NotImplementedException();
//     }

// 	public static PhaseMotion ComparePhase(PhaseType currPhase, PhaseType desiredPhase)
//     {
//         if(currPhase == desiredPhase)
//         {
//             return PhaseMotion.Flex;
//         }
// 		if(GetActions(desiredPhase).Item1 == PhaseAction.Strike && GetActions(currPhase).Item1 != PhaseAction.Strike )
//         {
//             return PhaseMotion.Advance;
//         }
// 		if(GetActions(desiredPhase).Item2 != PhaseAction.Strike && GetActions(currPhase).Item2 == PhaseAction.Strike)
//         {
//             return PhaseMotion.Retreat;
//         }
// 		else
//         {
//             return PhaseMotion.Flex;
//         }
//     }

// 	public enum PhaseType
// 	{
// 		Wait,
// 		Poke,
// 		WithStand,
// 		Duel,
// 		Engage
// 	}

// 	public static PhaseMotion GetInterSerction(PhaseMotion left, PhaseMotion right)
//     {
//         List<bool> leftSet = MotionToSet(left);
// 		List<bool> rightSet = MotionToSet(right);
// 		List<bool> resultSet = new List<bool>();
// 		for(int i =0; i < 3; i ++)
//         {
//             resultSet.Add(leftSet[i] && rightSet[i]);
//         }
// 		return SetToMotion(resultSet);
//     }

// 	public static List<bool> MotionToSet(PhaseMotion motion)
//     {
//         switch(motion)
//         {
//             case PhaseMotion.Advance: 	return new List<bool>{false, true, true};
// 			case PhaseMotion.Retreat: 	return new List<bool>{true, true, false};
// 			case PhaseMotion.Hold: 		return new List<bool>{false, true, false};
// 			case PhaseMotion.Flex: 		return new List<bool>{true, true, true};
// 			default: throw new NotImplementedException("Unimplemented PhaseMotion");
//         }
//     }

// 	public static PhaseMotion SetToMotion(List<bool> set)
//     {

//             if( set == new List<bool>{false, true, true}) 	{ return PhaseMotion.Advance;}
// 			if( set == new List<bool>{true, true, false}) 	{ return PhaseMotion.Retreat;}
// 			if( set == new List<bool>{false, true, false})	{ return PhaseMotion.Hold;}
// 			if( set == new List<bool>{true, true, true}) 	{ return PhaseMotion.Flex;}
			
// 			throw new NotImplementedException("Unimplemented PhaseMotion");
//     }
    

// 	public enum PhaseMotion
//     {
// 		Advance, // [0, 1, 1]
// 		Retreat, // [1, 1, 0]
//         Hold,    // [0, 1, 0]
// 		Flex	 // [1, 1, 1]
//     }

// 	public enum PhaseAction
// 	{
// 		Hold,
// 		Strike,
// 		Engage
// 	}

// 	public class Order
// 	{
// 		public TroopPhase desiredPhase {get;}
// 		public PhaseMotion movemenlefttions {get;}
		
// 		public Order(PhaseType current, TroopPhase phase, PhaseMotion allowedMovement, int effort)
// 		{
// 			this.desiredPhase = phase;
// 			this.movemenlefttions = GetInterSerction(allowedMovement, ComparePhase(current, desiredPhase.phaseType));
// 		}

// 		// public static Tuple<Order,Order> GetProtagonist(Order left, Order right)
//         // {
// 		// 	if(!left.GetAdvancing() && !left.GetFalling() && !right.GetAdvancing() && !right.GetFalling())
//         //     {
//         //         return null;
//         //     }
// 		// 	if(left.GetAdvancing() && right.GetAdvancing() || (left.GetFalling() && right.GetFalling()))
//         //     {
//         //         if(left.desiredPhase.effort > right.desiredPhase.effort)
//         //         {
//         //             return new Tuple<Order, Order>(left, right);
//         //         }
// 		// 		else
//         //         {
//         //             return new Tuple<Order, Order>(right, left);
//         //         }
//         //     }
// 		// 	if( !left.GetAdvancing() && !left.GetFalling())
//         //     {
//         //         return new Tuple<Order, Order>(right, left);
//         //     }
// 		// 	if(!right.GetAdvancing() && !right.GetFalling())
//         //     {
//         //         return new Tuple<Order, Order>(left, right);
//         //     }

// 		// 	throw new NotImplementedException();
//         // }
		
// 	}
// }
