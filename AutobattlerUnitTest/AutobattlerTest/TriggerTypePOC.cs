using System.Collections.Generic;
using static TriggerTypePOCTest.PhaseEmotion;

[TestClass]
public sealed class TriggerTypePOCTest
{
    [TestMethod]
    public void TestEventTriggerPOCSubscriptionWorks()
    {
        PerkEngagement engagement1 = new PerkEngagement();
        Perk2 firstPerk2 = new Perk2(3, new HashSet<Modes>{Modes.sad, Modes.medium});
        Perk2 secondPerk2 = new Perk2(5, new HashSet<Modes>{Modes.medium});
        Perk1 firstPerk1 = new Perk1();
        PerkEngagement.EngagementSubscriptionhandler subscirbe = new PerkEngagement.EngagementSubscriptionhandler(engagement1);

        firstPerk2.InitialiseSubscription(subscirbe);
        secondPerk2.InitialiseSubscription(subscirbe);
        firstPerk1.InitialiseSubscription(subscirbe);
        engagement1.TriggerPhaseEmotion(Modes.happy);
        Assert.AreEqual(true, firstPerk2.active);
        Assert.AreEqual(true, firstPerk2.active);
        Assert.AreEqual(false, firstPerk1.active);
    }

    [TestMethod]
    public void TestEventTriggerPOCListIteration()
    {
        PerkEngagement engagement1 = new PerkEngagement();
        List<PerkPOC> perkList = new List<PerkPOC>(){
                new Perk2(3, new HashSet<Modes>{Modes.sad, Modes.medium}),
                new Perk2(5, new HashSet<Modes>{Modes.medium}),
                new Perk1(),
        };
        PerkEngagement.EngagementSubscriptionhandler subscirbe = new PerkEngagement.EngagementSubscriptionhandler(engagement1);

        foreach(PerkPOC perk in perkList)
        {
            perk.InitialiseSubscription(subscirbe);
        }

        engagement1.TriggerPhaseEmotion(Modes.happy);
        
        foreach(PerkPOC perk in perkList)
        {
            if(perk is Perk2 perk2)
            {
                Assert.IsTrue(perk.IsActive());
                perk2.IncreaseIntensity();
            }
            if(perk is Perk1)
            {
                Assert.IsFalse(perk.IsActive());
            }
        }
    }

    [TestMethod]
    public void TestUnsubscribeWorks()
    {
        PerkEngagement engagement1 = new PerkEngagement();
        List<Perk2> perk2List = [new Perk2(3, new HashSet<Modes>{Modes.sad, Modes.medium}), new Perk2(5, new HashSet<Modes>{Modes.medium})];
        PerkEngagement.EngagementSubscriptionhandler subscirbe = new PerkEngagement.EngagementSubscriptionhandler(engagement1);
        foreach(Perk2 perk in perk2List)
        {
            perk.InitialiseSubscription(subscirbe);
        }
        perk2List[1].Dispose();
        engagement1.TriggerPhaseEmotion(Modes.happy);
        Assert.AreEqual(true, perk2List[0].active);
        Assert.AreEqual(false, perk2List[1].active);
        perk2List.RemoveAt(1);
        engagement1.TriggerPhaseEmotion(Modes.happy);
        perk2List.RemoveAt(0);
        engagement1.TriggerPhaseEmotion(Modes.happy);
    }

    [TestMethod]
    public void TestScopeUnsubscribe()
    {
        PerkEngagement engagement1 = new PerkEngagement();
        List<Perk2> perk2List = new List<Perk2>();
        PerkEngagement.EngagementSubscriptionhandler subscribe = new PerkEngagement.EngagementSubscriptionhandler(engagement1);
        for(int i = 0; i < 2; i++)
        {
            Perk2 perk = new Perk2(5, new HashSet<Modes>(){ Modes.sad});
            perk.InitialiseSubscription(new PerkEngagement.EngagementSubscriptionhandler(engagement1));
            perk2List.Add(perk);
        }
        perk2List[1].Dispose();
        perk2List[1].Dispose();
        engagement1.TriggerPhaseEmotion(Modes.happy);
        Assert.AreEqual(true, perk2List[0].active);
        Assert.AreEqual(false, perk2List[1].active);
        perk2List.RemoveAt(1);
        engagement1.TriggerPhaseEmotion(Modes.happy);
        perk2List.RemoveAt(0);
        engagement1.TriggerPhaseEmotion(Modes.happy);

    }

    public class PerkTroop
    {
        public List<PerkPOC> perkPool;
        public PerkTroop()
        {
            perkPool = new List<PerkPOC>();
        }
        public PerkTroop(List<PerkPOC> perk)
        {
            perkPool = perk;
        }
    }

    public class PerkEngagement
    {
        PerkTroop left;
        PerkTroop right;
        public event Action<PhaseEmotion> PhaseEmotionEvent = delegate{};
        public event Action<ItHappened> ItHappenedEvent = delegate{};  

        public PerkEngagement()
        {
            left = new PerkTroop();
            right = new PerkTroop();
        }

        public void TriggerPhaseEmotion(Modes emotion)
        {
            PhaseEmotionEvent(new PhaseEmotion{phaseMode=emotion});
        }

        public class EngagementSubscriptionhandler
        {
            private PerkEngagement context;

            public EngagementSubscriptionhandler(PerkEngagement context)
            {
                this.context = context;
            }

            public void UpdateSubscriptionToItHappendEvent(Action<ItHappened> subscriber, bool subscribe)
            {
                if(subscribe)
                {
                    context.ItHappenedEvent += subscriber;
                }
                else
                {
                    context.ItHappenedEvent -= subscriber;
                }

            }

            public void UpdateSubscriptionToPhaseEmotionEvent(Action<PhaseEmotion> subscriber, bool subscribe)
            {
                if(subscribe)
                {
                    context.PhaseEmotionEvent += subscriber;
                }
                else
                {
                    context.PhaseEmotionEvent -= subscriber;
                }
            }
        }   
    }

    public enum TriggerTypePoc
    {
        ItHappend,
        Emotion
    }

    public interface TriggerPOC
    {
        public TriggerTypePoc GetTriggerType();
    }

    public struct ItHappened : TriggerPOC
    {
        public bool it;
        public int value;
        public TriggerTypePoc GetTriggerType()
        {
            return TriggerTypePoc.ItHappend;
        }

    }

    public struct PhaseEmotion : TriggerPOC
    {
        public enum Modes
        {
            happy,
            sad, 
            medium
        }
        public Modes phaseMode;
        public TriggerTypePoc GetTriggerType()
        {
            return TriggerTypePoc.Emotion;
        }
    }

    public interface PerkPOC : IEquatable<PerkPOC>
    {
        public Guid GetId();
        public PerkPOC ResolveClash(PerkPOC otherPerkPoc);
        public bool IsActive();
        public void InitialiseSubscription(PerkEngagement.EngagementSubscriptionhandler subscriber);
    }

    public interface BuffPOC
    {
        
    }

    public interface DebuffApplierPOC
    {
        
    }

    public interface Debuff
    {
        
    }

    public class Perk1 : PerkPOC
    {
        public static Guid id {get; private set;} = Guid.NewGuid(); 
        public int power;
        public bool active;
        public bool happenedIt;

        public static int index = 0;

        public Perk1()
        {
            power = 0;
            active = false;
            happenedIt = true;
        }

        public Guid GetId()
        {
            return id;
        }

        public bool IsActive()
        {
            return active;
        }

        public bool Equals(PerkPOC? other)
        {
            if(other == null)
            {
                return false;
            }
            return id == other.GetId();
        }

        public void Trigger(ItHappened trigger)
        {
            active = true;
            power = trigger.value + index;
            index++;
        }

        public PerkPOC ResolveClash(PerkPOC otherPerkPoc)
        {
            if(!this.Equals(otherPerkPoc))
            {
                throw new NotImplementedException("Perks must be equatable to create a clash");
            }
            return this;
        }

        public void InitialiseSubscription(PerkEngagement.EngagementSubscriptionhandler subscriber)
        {
            subscriber.UpdateSubscriptionToItHappendEvent(Trigger, true);
        }

    }

    public class Perk2 : PerkPOC
    {
        public static Guid id {get; private set;} = Guid.NewGuid(); 
        public bool active;
        public int intensity;
        public HashSet<Modes> modes;
        PerkEngagement.EngagementSubscriptionhandler? subscriptionHandler;

        public bool IsActive()
        {
            return active;
        }

        public Perk2(int intensity, HashSet<Modes> modes)
        {
            this.modes = modes;
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

        public void Trigger(PhaseEmotion trigger)
        {
            active = true;
            switch(trigger.phaseMode)
            {
                case Modes.happy : intensity *= 2;
                    return;
                case Modes.sad : intensity = (int) (intensity * 0.5);
                    return;
                default:
                    return;
            }
        }

        public void InitialiseSubscription(PerkEngagement.EngagementSubscriptionhandler subscriber)
        {
            this.subscriptionHandler = subscriber;
             this.subscriptionHandler.UpdateSubscriptionToPhaseEmotionEvent(Trigger, true);
        }

        public void Dispose()
        {
           this.subscriptionHandler.UpdateSubscriptionToPhaseEmotionEvent(Trigger, false);
        }
    }

    public class Perk3 : PerkPOC, DebuffApplierPOC
    {
        public static Guid id;
        public bool isActive;
        HashSet<int> values;

        public Perk3()
        {
            values = new HashSet<int>();
        }

        public bool Equals(PerkPOC? other)
        {
            if(other != null)
            {
                return other.GetId() == id;
            }
            return false;
        }

        public Guid GetId()
        {
            return id;
        }

        public void InitialiseSubscription(PerkEngagement.EngagementSubscriptionhandler subscriber)
        {
            subscriber.UpdateSubscriptionToItHappendEvent(Trigger, true);
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Trigger(ItHappened happened)
        {
            values.Add(happened.value);
        }

        public PerkPOC ResolveClash(PerkPOC otherPerkPoc)
        {
            throw new NotImplementedException();
        }
    }
}