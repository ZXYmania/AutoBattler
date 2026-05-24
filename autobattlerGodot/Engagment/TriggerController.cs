using System;

public interface TriggerController
{
    // public event Action<Strike> OnStrike;
	public event Func<Strike, TroopPerk?> OnStrike;

	public event Action<MovementOrders> OnOrders;
	public event Action<MovementStep> OnMovement;

	// OnPush,
    // OnPushResult,
    // OnAttack,
    // OnAttackResult,
    // OnMove,
    // OnMoveResult,
    // OnReformation
}