using System;
using System.Collections.Generic;
using System.Linq;
using static ShieldDestroyed;
using static Strike;
using static Troop;

public class Hack : TroopPerk, DebuffApplier
{
    public Engagement.SubscriptionHadler? subscriber;
    public static Guid id = Guid.NewGuid();
    public Guid troopId;
    public Tuple<Guid,List<Squad>>? debuffs;

    public Hack(Guid troopId)
    {
        this.troopId = troopId;
    }

    public void InitialiseSubscription(Engagement.SubscriptionHadler subscriber)
    {
        if(this.subscriber != null)
        {
            throw new NotImplementedException("Unable to initialise already initialised subscription");
        }
        this.subscriber = subscriber;
        this.subscriber.UpdateStrikeSubscription(Trigger, true);
    }

    public void Dispose()
    {
        if(this.subscriber != null)
        {
            this.subscriber.UpdateStrikeSubscription(Trigger, false);
            this.subscriber = null;
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

    public Guid GetTroop()
    {
        return troopId;
    }

    public TroopPerk? Trigger(Strike strike)
    {
        if(troopId == strike.protagonistId)
        {

            bool updated = false;
            debuffs = new Tuple<Guid, List<Squad>>(strike.antagonistId, new List<Squad>());
            foreach((Squad squad, Hit hit) in strike.hit)
            {
                if(hit.area == BodyPart.Shield)
                {
                    debuffs.Item2.Add(squad);
                    updated = true;
                }
            }
            if(updated)
            {
                return this;
            }
        }
        debuffs = null;
        return null;
    }

    public bool IsActive()
    {
        return debuffs != null;
    }

    public Buff GetDebuff(Guid targetId)
    {
        if(debuffs == null || debuffs.Item1 != targetId)
        {
            throw new NotImplementedException("Invalid troopId debuff not initialised: " + targetId);
        }
        ShieldDestroyed child = new ShieldDestroyed(targetId, new HackOutput{shieldsDestroyed=debuffs.Item2});
        debuffs = null;
        return child;
    }

    public TroopPerk ResolveClash(TroopPerk otherPerk)
    {
        return this;
    }
}
