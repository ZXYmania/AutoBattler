using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Engagement;
using static MovementHandler;

public interface Trigger
{
    public enum TriggerType
    {
        OnPush,
        OnPushResult,
        OnAttack,
        OnAttackResult,
        OnMove,
        OnMoveResult,
        OnReformation
    }
    public TriggerType GetTriggerType();
}


public interface TroopPerk : IEquatable<TroopPerk>, IDisposable
{
    public Guid GetId();
    public void InitialiseSubscription(Engagement.SubscriptionHadler subscriber);
    public bool IsActive();
    public Guid GetTroop();
    public int GetHashCode();
    public TroopPerk ResolveClash(TroopPerk otherPerk);
}

public interface Buff
{
    public Stats GetStats();
}

public interface DebuffApplier
{
    public Buff GetDebuff(Guid targetId);
}

public interface ActionRequester
{
    public List<ActionRequest> RequestAction();
}
