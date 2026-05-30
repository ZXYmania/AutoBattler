using System;
using static Engagement;

public interface TriggerController
{
    public event Func<Hit, TroopPerk?>? OnHit;
	public event Func<Combat, TroopPerk?> OnCombat;

	public event Func<MovementOrders, TroopPerk?> OnOrders;
	public event Func<MovementStep, TroopPerk?> OnMovement;

    public event Action<EndOfRound> OnEndOfRound;

	// OnPush,
    // OnPushResult,
    // OnAttack,
    // OnAttackResult,
    // OnMove,
    // OnMoveResult,
    // OnReformation
}