using System.Collections.Generic;
using static TriggerTypePOCTest.Trigger2;

[TestClass]
public sealed class TriggerTypePOCTest
{
    [TestMethod]
    public void TestTriggerTypePOC()
    {
        List<PerkPOC> perks = new List<PerkPOC>() {new Perk1(), new Perk1(), new Perk2(3, new HashSet<Modes>{Modes.happy, Modes.sad}), new Perk2(5, new HashSet<Modes>{Modes.medium})};
        Trigger1 called1 = new Trigger1 { value = 6 };
        foreach(PerkPOC perk in perks)
        {
            perk.TriggerCheck(called1);
        }
        Trigger2 called2 = new Trigger2{modes = new HashSet<Modes>(){Modes.medium}};
        foreach(PerkPOC perk in perks)
        {
            perk.TriggerCheck(called2);
        }

        var unitTest = perks.GroupBy(x => x.GetId()).Where( group => group.Count() > 1);
        foreach(IGrouping<Guid,PerkPOC> group in unitTest)
        {
            foreach(PerkPOC perk in group)
            {
                if(perk is Perk2 perk2)
                {
                    perk2.IncreaseIntensity();
                }
            }
        }
        
    }

    public interface TriggerTypePOC
    {
        public Guid GetId();
        public bool TriggerMet(TriggerTypePOC trigger);
    }

    public interface PerkPOC : IEquatable<PerkPOC>
    {
        public Guid GetId();
        public TriggerTypePOC GetTrigger();
        public void TriggerCheck(TriggerTypePOC triggerCheck);
        public PerkPOC ResolveClash(PerkPOC otherPerkPoc);
    }

    public struct Trigger1 : TriggerTypePOC
    {
        static Guid id = Guid.NewGuid();

        public int value;
        public bool Equals(TriggerTypePOC? other)
        {
            if(other is Trigger1 trigger1)
            {
                return value == trigger1.value;
            }
            return false;
        }

        public bool TriggerMet(TriggerTypePOC trigger)
        {
            return trigger is Trigger1;
        }

        public Guid GetId()
        {
            return id;
        }
    }

    public struct Trigger2 : TriggerTypePOC
    {

        static Guid id = Guid.NewGuid();

        public enum Modes
        {
            happy,
            sad, 
            medium
        }

        public ISet<Modes> modes;

        public bool TriggerMet(TriggerTypePOC trigger)
        {
            if(trigger is Trigger2 f)
            {
                return f.modes.IsSubsetOf(modes);
            }
            return false;
        }

        public Guid GetId()
        {
            return id;
        }
    }

    public class Perk1 : PerkPOC
    {
        public static Guid id {get; private set;} = Guid.NewGuid(); 
        public int power;
        public bool active;
        public Trigger1 trigger;

        public Perk1()
        {
            power = 0;
            active = false;
            trigger = new Trigger1();
        }

        public Guid GetId()
        {
            return id;
        }

        public bool Equals(PerkPOC? other)
        {
            if(other == null)
            {
                return false;
            }
            return id == other.GetId();
        }

        public TriggerTypePOC GetTrigger()
        {
            return trigger;
        }

        public void Trigger(Trigger1 trigger)
        {
            active = true;
            power = trigger.value + 3;
        }

        public PerkPOC ResolveClash(PerkPOC otherPerkPoc)
        {
            if(this != otherPerkPoc)
            {
                throw new NotImplementedException("Perks must be equatable to create a clash");
            }
            return this;
        }

        public void TriggerCheck(TriggerTypePOC triggerCheck)
        {
            if(triggerCheck is Trigger1 trigger1)
            {
                if(this.trigger.TriggerMet(triggerCheck))
                {
                    Trigger(trigger1);
                }
            }
        }
    }

    public class Perk2 : PerkPOC
    {
        public static Guid id {get; private set;} = Guid.NewGuid(); 
        public Dictionary<Guid,TriggerTypePOC> triggerDict;
        public bool active;
        public int intensity;

        public Perk2(int intensity, HashSet<Modes> modes)
        {
            var trigger = new Trigger2{modes = modes};
            triggerDict = new Dictionary<Guid, TriggerTypePOC>();
            triggerDict.Add(trigger.GetId(), trigger);
            active = false;
            this.intensity = intensity;
        }

        public bool Equals(PerkPOC? other)
        {
            if(other is Perk2 perk2)
            {
                return id == other.GetId() && intensity == perk2.intensity;
            }
             return false;
        }

        public void IncreaseIntensity()
        {
            intensity++;
        }

        public Guid GetId()
        {
            return id;
        }

        public TriggerTypePOC GetTrigger()
        {
            return triggerDict.First().Value;
        }

        public PerkPOC ResolveClash(PerkPOC otherPerkPoc)
        {
            if(this == otherPerkPoc && otherPerkPoc is Perk2 perk2)
            {
                if(this.intensity > perk2.intensity)
                {
                    return this;
                }
                return otherPerkPoc;
            }
            throw new NotImplementedException("Perks must be equatable for a clash to occur");
        }

        public void TriggerCheck(TriggerTypePOC triggerCheck)
        {
            if(triggerDict.ContainsKey(triggerCheck.GetId()))
            {
                if(triggerDict[triggerCheck.GetId()].TriggerMet(triggerCheck))
                {
                    Trigger(triggerCheck);
                }
            }
        }

        public void Trigger(TriggerTypePOC trigger)
        {
            if(trigger is Trigger2 trigger2)
            {
                active = true;
                switch(trigger2.modes.First())
                {
                    case Modes.happy : intensity *= 2;
                        return;
                    case Modes.sad : intensity = (int) (intensity * 0.5);
                        return;
                }
            }
        }
    }
}