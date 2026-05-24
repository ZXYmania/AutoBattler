using Moq;
using static AffectsMovement;
using static Engagement;
using static Movement;
using static Phase;
using static Strike;
using static Troop;

[TestClass]
public sealed class PhalanxPerkTest
{

    [TestMethod]
    public void TestInit()
    {
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupAdd(m=> m.OnOrders += It.IsAny<Action<MovementOrders>>());
        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);

        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        perk.InitialiseSubscription(subscription.Object);
        triggerController.VerifyAdd(m => m.OnOrders += It.IsAny<Action<MovementOrders>>(), Times.Exactly(1));
    }

    public static IEnumerable<ValueTuple<PhalanxFormation>> GetDefault()
    {
        return new List<ValueTuple<PhalanxFormation>>()
        {
            new ValueTuple<PhalanxFormation>(new PhalanxFormation(Guid.NewGuid())),
            new ValueTuple<PhalanxFormation>(new PhalanxFormation(Guid.NewGuid()){ enemyId= Guid.NewGuid(), hitAmount=15})
        };
    }

    [DataTestMethod]
    [DynamicData(nameof(GetDefault), DynamicDataSourceType.Method)]
    public void TestOnEnd(PhalanxFormation perk)
    {
        perk.ResetPerk();
        Assert.AreEqual(false, perk.actionRequested);
        Assert.AreEqual(false, perk.movementActive);
        Assert.AreEqual(false, perk.IsActive());
        Assert.AreEqual(0, perk.hitAmount);
        Assert.AreEqual(Guid.Empty, perk.enemyId);
    }

    [TestMethod]
    public void TestDispose()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        // Assert no Errors
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupRemove(m=> m.OnOrders -= It.IsAny<Action<MovementOrders>>());
        triggerController.SetupRemove(m=> m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>());

        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);
        perk.InitialiseSubscription(subscription.Object);
        perk.Dispose();

        triggerController.VerifyRemove(m => m.OnOrders -= It.IsAny<Action<MovementOrders>>(), Times.Exactly(1));
        triggerController.VerifyRemove(m => m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestTriggerOnOrder()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);

        orders.protagonistOrder = new Order{captain=protagonistCaptain.GetCaptain()}; 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        orders.antagonistOrder = new Order{troopId=perk.GetTroop()};
        orders.currentPhase = PhaseType.Poke;
        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        TestOnEnd(perk);
    }

    [TestMethod]
    public void TestRequestAction()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);

        orders.protagonistOrder = new Order{captain=protagonistCaptain.GetCaptain()}; 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        orders.antagonistOrder = new Order{troopId=perk.GetTroop()};
        orders.currentPhase = PhaseType.Poke;
        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        var expect = new List<ActionRequest>(){new ActionRequest{type=ActionType.Strike}};
        var actual = perk.RequestAction();
        for(int i = 0; i < actual.Count; i++)
        {
            Assert.AreEqual(expect[i].type, actual[i].type);
        }
        TestOnEnd(perk);
    }

    [TestMethod]
    public void TestRequestActionUninitialised()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        Assert.AreEqual(0, perk.RequestAction().Count());
    }

    [TestMethod]
    public void TestNoHitMovementTrigger()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);

        orders.protagonistOrder = new Order{captain=protagonistCaptain.GetCaptain()}; 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        orders.antagonistOrder = new Order{troopId=perk.GetTroop()};
        orders.currentPhase = PhaseType.Poke;
        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            hit = new Dictionary<Squad, Hit>(),
            outcome = new Dictionary<Squad, StrikeOutcome>()
        });
        Assert.AreEqual(true, perk.movementActive);
        Assert.AreEqual(false, perk.actionRequested);
        var modifier = perk.GetMovementModifier();
        Assert.AreEqual(MovementModifierType.Additive, modifier!.Value.type);
        Assert.AreEqual(-5, modifier.Value.value);
        TestOnEnd(perk);
    }

    [TestMethod]
    public void TestHitTriggerOnMovement()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);

        orders.protagonistOrder = new Order{captain=protagonistCaptain.GetCaptain()}; 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        orders.antagonistOrder = new Order{troopId=perk.GetTroop()};
        orders.currentPhase = PhaseType.Poke;
        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        triggerController.TriggerOnStrike(new Strike
        {
            protagonistId = perk.troopId,
            hit = new Dictionary<Squad, Hit>(){
                {new Squad(1), new Hit{area=BodyPart.Head}},
                {new Squad(2), new Hit{area=BodyPart.Head}},
                {new Squad(3), new Hit{area=BodyPart.Head}},
                {new Squad(4), new Hit{area=BodyPart.Head}}

            }
        });
        Assert.AreEqual(true, perk.movementActive);
        Assert.AreEqual(false, perk.actionRequested);
        var modifier = perk.GetMovementModifier();
        Assert.AreEqual(MovementModifierType.Additive, modifier!.Value.type);
        Assert.IsTrue(modifier.Value.value > 0);
        TestOnEnd(perk);
    }

    [TestMethod]
    public void TestGetMovementModifierNull()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        Assert.AreEqual(null, perk.GetMovementModifier());
    }
}