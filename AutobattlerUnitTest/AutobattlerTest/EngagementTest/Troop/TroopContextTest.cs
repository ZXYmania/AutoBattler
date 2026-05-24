using Moq;
using static Troop;

[TestClass]
public sealed class TroopContextTest
{
    [TestMethod]
    public void TestGetStats()
    {
        Stats result = new Stats{ strike=1, block=2, formation=3, power=4,endurance=5};
        TroopContext context = new TroopContext(){stats=result};
        Assert.AreEqual(result, context.GetStats());
    }

    [TestMethod]
    public void TestGetStatsWithBuff()
    {
        Stats result = new Stats(1, 2, 3, 4, 5);
        Mock<Buff> buff = new Mock<Buff>();
        buff.Setup( b => b.GetStats()).Returns(new Stats(2,2,2,2,2));
        buff.As<TroopPerk>().Setup( b => b.IsActive()).Returns(true);
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>(){buff.As<TroopPerk>()});
        Mock<Troop> troop = new Mock<Troop>(unit.GetUnit());

        TroopContext context = new TroopContext(troop.Object);
        Assert.AreEqual(new Stats(3,4,5,6,7), context.GetStats());
    }

    [TestMethod]
    public void TestGetStatsWithNegativeBuff()
    {
        Stats result = new Stats(1, 2, 3, 4, 5);
        Mock<Buff> buff = new Mock<Buff>();
        buff.Setup( b => b.GetStats()).Returns(new Stats(-2,-2,-2,-2,-2));
        buff.As<TroopPerk>().Setup( b => b.IsActive()).Returns(true);
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>(){buff.As<TroopPerk>()});
        Mock<Troop> troop = new Mock<Troop>(unit.GetUnit());

        TroopContext context = new TroopContext(troop.Object);
        Assert.AreEqual(new Stats(-1,0,1,2,3), context.GetStats());
    }
}