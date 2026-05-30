using Moq;
using static Engagement;
using static Troop;

[TestClass]
public sealed class ShieldDestroyedTest
{

    [TestMethod]
    public void TestInit()
    {
        var triggerController = new Mock<TriggerController>();
        triggerController.SetupAdd(m=> m.OnCombat += It.IsAny<Func<Combat, TroopPerk?>>());
        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);

        ShieldDestroyed perk = new ShieldDestroyed(Guid.NewGuid(), new ShieldDestroyed.HackOutput());
        perk.InitialiseSubscription(subscription.Object);
        // End triggers not implemented yet
        // triggerController.VerifyAdd(m => m.OnStrike += It.IsAny<Func<Strike, TroopPerk?>>(), Times.Exactly(1));
        
    }

    [TestMethod]
    public void TestDispose()
    {
        ShieldDestroyed perk = new ShieldDestroyed(Guid.NewGuid(), new ShieldDestroyed.HackOutput());
        // Assert no Errors
        var triggerController = new Mock<TriggerController>();
        // triggerController.SetupRemove(m=> m.OnOrders -= It.IsAny<Action<MovementOrders>>());
        // triggerController.SetupRemove(m=> m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>());

        var subscription = new Mock<SubscriptionHadler>(triggerController.Object);
        perk.InitialiseSubscription(subscription.Object);
        perk.Dispose();

        // triggerController.VerifyRemove(m => m.OnOrders -= It.IsAny<Action<MovementOrders>>(), Times.Exactly(1));
        // triggerController.VerifyRemove(m => m.OnStrike -= It.IsAny<Func<Strike, TroopPerk?>>(), Times.Exactly(1));
    }

    [TestMethod]
    public void TestClash()
    {
        var troopId = Guid.NewGuid();
        var squadList = new List<Squad>()
        {
            new Squad(1),
            new Squad(2),
            new Squad(3),
            new Squad(4),
            new Squad(5)
        };
         ShieldDestroyed perk1 = new ShieldDestroyed(troopId, new ShieldDestroyed.HackOutput()
         {
             shieldsDestroyed = new List<Squad>()
             {
                squadList[0],
                squadList[3],
                squadList[4],
             }
         });
         ShieldDestroyed perk2 = new ShieldDestroyed(troopId, new ShieldDestroyed.HackOutput()
        {
             shieldsDestroyed = new List<Squad>()
             {
                squadList[1],
                squadList[2],
                squadList[3],
             }
         });

        ShieldDestroyed result = (ShieldDestroyed) perk1.ResolveClash(perk2);

         Assert.AreEqual(new Stats(0,0,-25 * 5,0,0), result.GetStats());

    }
}
