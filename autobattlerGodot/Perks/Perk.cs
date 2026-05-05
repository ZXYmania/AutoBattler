using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static Engagement;
using static MovementHandler;

public interface TriggerType
{
    public bool TriggerMet(TriggerType trigger);
        // OnPush,
        // OnPushWin,
        // OnAttack,
        // OnStrikeSuccessful,
        // OnBlock,
        // OnBlockSuccessful,
        // OnReformation,
}

// public interface ExpirationTrigger {}

public interface Perk : IEquatable<Perk>
{
    public Guid GetId();
    public Perk? TriggerCheck(TriggerType triggerCheck);
    public bool IsActive();
    public void OnEnd();
    public Perk ResolveClash(Perk rightPerk);

    public static Perk ResolveClash(IEnumerable<Perk> perks)
    {
        Perk output = perks.First();
        foreach(Perk perk in perks)
        {
            if(output != perk)
            {
                throw new NotImplementedException("Perks must be equatable for a clash to occur");
            }
            output = output.ResolveClash(perk);
        }
        return output;
    }
}

public interface Buff
{
    public Stats GetStats();
}

public interface Debuff : Buff
{
    
}

public interface ActionRequester
{
    public ActionType RequestAction();
}

public interface RequiresAction<T> where T : TroopAction
{

    public void ReceiveResults(T results);
}




