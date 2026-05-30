using Moq;
using static AffectsMovement;
using static Engagement;
using static Movement;
using static Phase;
using static Combat;
using static Troop;
using static Hit;

[TestClass]
public sealed class PhalanxPerkTest
{

    [TestMethod]
    public void TestInit()
    {
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupAdd(m=> m.OnOrders += It.IsAny<Func<MovementOrders,TroopPerk?>>());
        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);

        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        perk.InitialiseSubscription(subscription.Object);
        triggerController.VerifyAdd(m => m.OnOrders += It.IsAny<Func<MovementOrders,TroopPerk?>>(), Times.Exactly(1));
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
        triggerController.SetupRemove(m=> m.OnOrders -= It.IsAny<Func<MovementOrders,TroopPerk?>>());
        triggerController.SetupRemove(m=> m.OnCombat -= It.IsAny<Func<Combat, TroopPerk?>>());

        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);
        perk.InitialiseSubscription(subscription.Object);
        perk.Dispose();

        triggerController.VerifyRemove(m => m.OnOrders -= It.IsAny<Func<MovementOrders,TroopPerk?>>(), Times.Exactly(1));
        triggerController.VerifyRemove(m => m.OnCombat -= It.IsAny<Func<Combat, TroopPerk?>>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestTriggerOnOrder()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        PhaseType currentPhase = PhaseType.Poke;

        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);
        TestCaptain antagonistCaptain = new TestCaptain(new TroopContext(){id = perk.GetTroop()});
        antagonistCaptain.SetupOnMovement(MovementType.Stay);

        orders.protagonistOrder = protagonistCaptain.GetOrder(currentPhase); 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        orders.antagonistOrder = antagonistCaptain.GetOrder(currentPhase);
        orders.currentPhase = currentPhase;

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
        PhaseType currentPhase = PhaseType.Poke;

        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);
        orders.protagonistOrder = protagonistCaptain.GetOrder(currentPhase); 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        
        TestCaptain antagonistCaptain = new TestCaptain(new TroopContext(){id = perk.GetTroop()});
        antagonistCaptain.SetupOnMovement(MovementType.Stay);
        orders.antagonistOrder = antagonistCaptain.GetOrder(currentPhase);
        orders.currentPhase = currentPhase;

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
    public void TestNoStrikeMovementTrigger()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        var currentPhase = PhaseType.Poke;

        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);
        orders.protagonistOrder = protagonistCaptain.GetOrder(currentPhase); 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        
        TestCaptain antagonistCaptain = new TestCaptain(new TroopContext(){id = perk.GetTroop()});
        antagonistCaptain.SetupOnMovement(MovementType.Stay);
        orders.antagonistOrder = antagonistCaptain.GetOrder(currentPhase);

        orders.currentPhase = currentPhase;
        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        triggerController.TriggerOnStrike(new Combat
        {
            protagonistId = perk.troopId,
            strikeList = new Dictionary<Squad, Strike>(),
            outcomeList = new Dictionary<Squad, StrikeOutcome>()
        });
        Assert.AreEqual(true, perk.movementActive);
        Assert.AreEqual(false, perk.actionRequested);
        var modifier = perk.GetMovementModifier();
        Assert.AreEqual(MovementModifierType.Additive, modifier!.Value.type);
        Assert.AreEqual(-5, modifier.Value.value);
        TestOnEnd(perk);
    }

    [TestMethod]
    public void TestStrikeTriggerOnMovement()
    {
        PhalanxFormation perk = new PhalanxFormation(Guid.NewGuid());
        var triggerController = new UnitTestTriggerController();
        var subscription = new Mock<SubscriptionHadler>(triggerController);
        MovementOrders orders = new MovementOrders();
        var currentPhase = PhaseType.Poke;

        TestCaptain protagonistCaptain = new TestCaptain();
        protagonistCaptain.SetupOnMovement(MovementType.Advance);
        orders.protagonistOrder = protagonistCaptain.GetOrder(currentPhase); 
        orders.protagonistOrder.SetProtagonist(PhaseType.Poke);
        
        TestCaptain antagonistCaptain = new TestCaptain(new TroopContext(){id = perk.GetTroop()});
        antagonistCaptain.SetupOnMovement(MovementType.Stay);
        orders.antagonistOrder = antagonistCaptain.GetOrder(currentPhase);
        orders.currentPhase = currentPhase;

        perk.InitialiseSubscription(subscription.Object);
        triggerController.TriggerOnOrders(orders);
        Assert.AreEqual(true, perk.actionRequested);
        triggerController.TriggerOnStrike(new Combat
        {
            protagonistId = perk.troopId,
            strikeList = new Dictionary<Squad, Strike>(){
                {new Squad(1), new Strike{area=BodyPart.Head}},
                {new Squad(2), new Strike{area=BodyPart.Head}},
                {new Squad(3), new Strike{area=BodyPart.Head}},
                {new Squad(4), new Strike{area=BodyPart.Head}}

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