using Moq;
using static Engagement;
using static Movement;
using static Phase;
using static Strike;
using static TriggerTypePOCTest;
using static Troop;

[TestClass]
public sealed class HackTest
{

    [TestMethod]
    public void TestInit()
    {
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupAdd(m=> m.OnStrike += It.IsAny<Func<Strike, TroopPerk?>>());
        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);

        Hack perk = new Hack(Guid.NewGuid());
        perk.InitialiseSubscription(subscription.Object);
        triggerController.VerifyAdd(m => m.OnStrike += It.IsAny<Func<Strike, TroopPerk?>>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestDispose()
    {
        Hack perk = new Hack(Guid.NewGuid());
        // Assert no Errors
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupRemove(m=> m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>());

        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);
        perk.InitialiseSubscription(subscription.Object);
        perk.Dispose();

        triggerController.VerifyRemove(m => m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestOnStrikeSquadsUpdated()
    {
        Hack perk = new Hack(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        var squads = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4)
        };

        perk.InitialiseSubscription(subscription.Object);
        Guid antagonist = Guid.NewGuid();
        var result = triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Leg}},
                {squads[1], new Hit{area=BodyPart.Shield}},
                {squads[2], new Hit{area=BodyPart.Shield}},
                {squads[3], new Hit{area=BodyPart.Leg}}
            }
        });
        Assert.IsTrue(perk.debuffs!.Item2.Contains(squads[1]));
        Assert.IsTrue(perk.debuffs!.Item2.Contains(squads[2]));
        Assert.AreEqual(2, perk.debuffs!.Item2.Count());
        Assert.AreEqual(perk, result.First());
        Assert.IsTrue(perk.IsActive());
    }

    [TestMethod]
    public void TestNoPerkReturn()
    {
        Hack perk = new Hack(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        var squads = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4)
        };

        perk.InitialiseSubscription(subscription.Object);
        Guid antagonist = Guid.NewGuid();
        var result = triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Leg}},
                {squads[1], new Hit{area=BodyPart.Leg}},
                {squads[2], new Hit{area=BodyPart.Leg}},
                {squads[3], new Hit{area=BodyPart.Leg}}
            }
        });

        Assert.AreEqual(null, perk.debuffs);
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void TestOnStrikeSquadsAddSecondRound()
    {
        Hack perk = new Hack(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        var squads = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4)
        };

        perk.InitialiseSubscription(subscription.Object);

        Guid antagonist = Guid.NewGuid();
        triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Leg}},
                {squads[1], new Hit{area=BodyPart.Shield}},
                {squads[2], new Hit{area=BodyPart.Shield}},
                {squads[3], new Hit{area=BodyPart.Leg}}
            }
        });
        
        var result = triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Leg}},
                {squads[1], new Hit{area=BodyPart.Leg}},
                {squads[2], new Hit{area=BodyPart.Shield}},
                {squads[3], new Hit{area=BodyPart.Shield}}
            }
        });

        Assert.AreEqual(antagonist, perk.debuffs!.Item1);
        Assert.IsTrue(perk.debuffs!.Item2.Contains(squads[2]));
        Assert.IsTrue(perk.debuffs!.Item2.Contains(squads[3]));
        Assert.AreEqual(2, perk.debuffs.Item2.Count());
        Assert.AreEqual(perk, result.First());
        Assert.IsTrue(perk.IsActive());
    }

    [TestMethod]
    public void TestOnStrikeDebuff()
    {
        Hack perk = new Hack(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        var squads = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4)
        };

        perk.InitialiseSubscription(subscription.Object);
        Guid antagonist = Guid.NewGuid();
        var result = triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Leg}},
                {squads[1], new Hit{area=BodyPart.Leg}},
                {squads[2], new Hit{area=BodyPart.Shield}},
                {squads[3], new Hit{area=BodyPart.Leg}}
            }
        });

        Buff debuff = perk.GetDebuff(antagonist);
        Assert.AreEqual(perk, result.First());
        Assert.AreEqual(new Stats(0,0,-25,0,0) ,debuff.GetStats());
        Assert.AreEqual(null, perk.debuffs);
        Assert.IsFalse(perk.IsActive());
    }

    [TestMethod]
    public void TestOnStrikeDebuffs()
    {
        Hack perk = new Hack(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        var squads = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4)
        };

        perk.InitialiseSubscription(subscription.Object);
        Guid antagonist = Guid.NewGuid();
        var result = triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            antagonistId = antagonist,
            hit = new Dictionary<Squad, Hit>(){
                {squads[0], new Hit{area=BodyPart.Shield}},
                {squads[1], new Hit{area=BodyPart.Shield}},
                {squads[2], new Hit{area=BodyPart.Shield}},
                {squads[3], new Hit{area=BodyPart.Leg}}
            }
        });

        Buff debuff = perk.GetDebuff(antagonist);
        Assert.AreEqual(perk, result.First());
        Assert.AreEqual(new Stats(0,0,-75,0,0) ,debuff.GetStats());
        Assert.AreEqual(null, perk.debuffs);
        Assert.IsFalse(perk.IsActive());
    }

    [TestMethod]
    public void TestHackClash()
    {
        Hack perk1 = new Hack(Guid.NewGuid());
        Hack perk2 = new Hack(Guid.NewGuid());
        var result = perk1.ResolveClash(perk2);
    }
}