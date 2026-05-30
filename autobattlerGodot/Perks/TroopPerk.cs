using System;
using System.Collections.Generic;

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
    public class InvalidDebuffTarget : Exception
    {
        public InvalidDebuffTarget(Guid troopId, Guid? targtId) : base(
            targtId != null? "Debuff id: "+ troopId + " doesn't match target id: " + targtId : "Debuff has no target." )
        {
            
        }
    }
}

public interface ActionRequester
{
    public List<ActionRequest> RequestAction();
}
