using System;
using System.Collections.Generic;
using System.Linq;
using static Troop;

public class ShieldDestroyed : TroopPerk, Buff
{
    Engagement.SubscriptionHadler? subscriptionHadler;
    public struct HackOutput
    {
        public List<Squad> shieldsDestroyed;
    }
    
    public static Guid id = Guid.NewGuid();
    Guid troopId;
    HackOutput output;

    public ShieldDestroyed(Guid troopId, HackOutput output)
    {
        this.troopId = troopId;
        this.output = output;
    }

    public void InitialiseSubscription(Engagement.SubscriptionHadler subscriber)
    {
        if(this.subscriptionHadler != null)
        {
            throw new NotImplementedException("Unable to initialise already initialised subscription");
        }
        this.subscriptionHadler = subscriber;
    }

    public void Dispose()
    {
        if(this.subscriptionHadler != null)
        {
            this.subscriptionHadler = null;
        }
    }

    public bool Equals(TroopPerk? other)
    {
        if(other != null)
        {
            return id == other.GetId();
        }
        return false;
    }

    public Guid GetId()
    {
        return id;
    }

    public Stats GetStats()
    {
        return new Stats(0,0,-25 * output.shieldsDestroyed.Count,0,0);
    }

    public Guid GetTroop()
    {
        return troopId;
    }

    public bool IsActive()
    {
        return output.shieldsDestroyed.Count > 0;
    }

    public TroopPerk ResolveClash(TroopPerk otherPerk)
    {
        if(otherPerk.GetTroop() != troopId)
        {
            throw new NotImplementedException("TroopIds must match "+ troopId +", "+otherPerk.GetTroop());
        }
        if(otherPerk is ShieldDestroyed debuff)
        {
            output.shieldsDestroyed = debuff.output.shieldsDestroyed.Union(output.shieldsDestroyed).ToList();
        }
        return this;
    }
}