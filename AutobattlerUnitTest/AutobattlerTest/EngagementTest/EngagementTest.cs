using Moq;
using static Phase;
using static Troop;

[TestClass]
public sealed class EngagementTest
{
    [TestMethod]
    public void TestEngagementStrike()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,50,50,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>());
        Troop leftTroop = new Troop(unit.GetUnit());
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(unit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(new Stats(500,500,485,500,500), engagement.rightTroop.stats);
    }

    [TestMethod]
    public void TestEngagementStrikeRouted()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,50,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>());
        Troop leftTroop = new Troop(unit.GetUnit());
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(unit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(0, rightTroop.frontline.Where(t => t.column == 3).Count());
        Assert.IsTrue(rightTroop.routed);
    }

    [TestMethod]
    public void TestEngagementStrikeDamaged()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,50,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>());
        Troop leftTroop = new Troop(unit.GetUnit());
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(unit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(5, rightTroop.frontline.Count());
        Assert.AreEqual(0, rightTroop.backline.Count());
        Assert.IsTrue(rightTroop.frontline.Contains(backline));
        Assert.IsFalse(rightTroop.routed);
    }

    [TestMethod]
    public void TestResolveConflict()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,50,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        Mock<TroopPerk> perk = new Mock<TroopPerk>();
        TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>(){perk});
        
        Troop leftTroop = new Troop(unit.GetUnit());
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(unit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(new Stats(500,500,500,500,500),engagement.leftTroop.stats);
        Assert.AreEqual(new Stats(500,500,488,500,500), engagement.rightTroop.stats);
        Assert.AreEqual(5, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(5, engagement.rightTroop.frontline.Count);
        Assert.IsTrue(engagement.rightTroop.frontline.Contains(backline));
    }

    [TestMethod]
    public void TestStrikeHandlePerk()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,0,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        Guid leftTroopId = Guid.NewGuid();
        var perk = new Hack(leftTroopId);
        TestUnit leftUnit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<TroopPerk>(){perk});
        TestUnit rightUnit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<TroopPerk>(){});

        Troop leftTroop = new Troop(leftUnit.GetUnit());
        leftTroop.id = leftTroopId;
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(rightUnit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(new Stats(500,500,500,500,500),engagement.leftTroop.stats);
        Assert.AreEqual(new Stats(500,500,466,500,500), engagement.rightTroop.GetContext().stats);
        Assert.AreEqual(5, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(5, engagement.rightTroop.frontline.Count);
        Assert.IsTrue(engagement.rightTroop.frontline.Contains(backline));
    }

    [TestMethod]
    public void TestMovement()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,0,50});
        Stats result = new Stats(500, 500, 500, 500, 500);
        Guid leftTroopId = Guid.NewGuid();
        var perk = new PhalanxFormation(leftTroopId);
        var leftCaptain = new TestCaptain();
        leftCaptain.SetupOrder(Phase.PhaseType.Poke);
        leftCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
        var rightCaptain = new TestCaptain();
        rightCaptain.SetupOrder(Phase.PhaseType.Engage);
        rightCaptain.SetupOnMovement(Movement.MovementType.Advance);
        TestUnit leftUnit = new TestUnit(result, leftCaptain.GetCaptain(), new List<TroopPerk>(){perk});
        TestUnit rightUnit = new TestUnit(result, rightCaptain.GetCaptain(), new List<TroopPerk>(){});
         Troop leftTroop = new Troop(leftUnit.GetUnit());
        leftTroop.id = leftTroopId;
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(rightUnit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.HandleMovementRound();
        Assert.AreEqual(PhaseType.Poke, engagement.currentPhase);
        Assert.AreEqual(-5, engagement.rightTroop.stamina);
        Assert.IsTrue(engagement.leftTroop.perkPool.ContainsKey(Attacked.id));
    }

    [TestMethod]
    public void AttackedOnce()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,0,50, 100, 100, 100, 100, 100});
        Stats result = new Stats(500, 500, 500, 500, 500);
        Guid leftTroopId = Guid.NewGuid();
        var perk = new PhalanxFormation(leftTroopId);
        var leftCaptain = new TestCaptain();
        leftCaptain.SetupOrder(Phase.PhaseType.Poke);
        leftCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
        var rightCaptain = new TestCaptain();
        rightCaptain.SetupOrder(Phase.PhaseType.Engage);
        rightCaptain.SetupOnMovement(Movement.MovementType.Advance);
        TestUnit leftUnit = new TestUnit(result, leftCaptain.GetCaptain(), new List<TroopPerk>(){perk});
        TestUnit rightUnit = new TestUnit(result, rightCaptain.GetCaptain(), new List<TroopPerk>(){});
         Troop leftTroop = new Troop(leftUnit.GetUnit());
        leftTroop.id = leftTroopId;
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(rightUnit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.HandleMovementRound();
        Assert.AreEqual(PhaseType.Poke, engagement.currentPhase);
        Assert.AreEqual(5, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(5, engagement.rightTroop.frontline.Count);
        Assert.AreEqual(-5, engagement.rightTroop.stamina);
        Assert.IsTrue(engagement.leftTroop.perkPool.ContainsKey(Attacked.id));
        engagement.ResolveStrike(leftTroop.GetContext(), rightTroop.GetContext());
        Assert.AreEqual(-5, engagement.rightTroop.stamina);
        Assert.AreEqual(5, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(5, engagement.rightTroop.frontline.Count);
    }

    [TestMethod]
    public void TestResolveRound()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){50,50,100,0,50, 100, 100, 100, 100, 100});
        Stats result = new Stats(500, 500, 500, 500, 500);
        Guid leftTroopId = Guid.NewGuid();
        var perk = new PhalanxFormation(leftTroopId);
        var leftCaptain = new TestCaptain();
        leftCaptain.SetupOrder(Phase.PhaseType.Poke);
        leftCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
        var rightCaptain = new TestCaptain();
        rightCaptain.SetupOrder(Phase.PhaseType.Engage);
        rightCaptain.SetupOnMovement(Movement.MovementType.Advance);
        TestUnit leftUnit = new TestUnit(result, leftCaptain.GetCaptain(), new List<TroopPerk>(){perk});
        TestUnit rightUnit = new TestUnit(result, rightCaptain.GetCaptain(), new List<TroopPerk>(){});
         Troop leftTroop = new Troop(leftUnit.GetUnit());
        leftTroop.id = leftTroopId;
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Troop rightTroop = new Troop(rightUnit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        Squad backline = new Squad(3);
        rightTroop.backline = new List<Squad>(){backline};
        
        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.ResolveRound();
        Assert.AreEqual(PhaseType.Poke, engagement.currentPhase);
        Assert.AreEqual(0, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(-25, engagement.leftTroop.stamina);
        Assert.AreEqual(5, engagement.rightTroop.frontline.Count);
        Assert.AreEqual(-55, engagement.rightTroop.stamina);
        Assert.IsTrue(engagement.leftTroop.perkPool.ContainsKey(Attacked.id));
        Assert.IsTrue(engagement.rightTroop.perkPool.ContainsKey(Attacked.id));
    }

       [TestMethod]
    public void TestResolveRoundNoPerk()
    {
        var rng = new MockUnitTestRNG(rngInt: new List<int>(){
            50, 50, 91, 0, 50, 
            50, 50, 91, 0, 50,
            50, 50, 91, 0, 50,
            50, 50, 91, 0, 50,
        });
        Stats result = new Stats(500, 500, 500, 500, 500);
        var leftCaptain = new TestCaptain();
        leftCaptain.SetupOrder(Phase.PhaseType.Poke);
        leftCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
        var rightCaptain = new TestCaptain();
        rightCaptain.SetupOrder(Phase.PhaseType.Poke);
        rightCaptain.SetupOnMovement(Movement.MovementType.Stay);
        TestUnit leftUnit = new TestUnit(result, leftCaptain.GetCaptain(), new List<TroopPerk>(){});
        TestUnit rightUnit = new TestUnit(result, rightCaptain.GetCaptain(), new List<TroopPerk>(){});
        Troop leftTroop = new Troop(leftUnit.GetUnit());
        leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        leftTroop.backline = new List<Squad>(){new Squad(3)};
        Troop rightTroop = new Troop(rightUnit.GetUnit());
        rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
        rightTroop.backline = new List<Squad>(){new Squad(3)};

        Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
        engagement.currentPhase = PhaseType.Poke;
        engagement.ResolveRound();
        engagement.ResolveRound();
        Assert.AreEqual(PhaseType.Poke, engagement.currentPhase);
        
        Assert.AreEqual(4, engagement.leftTroop.frontline.Count);
        Assert.AreEqual(-58, engagement.leftTroop.stamina);
        Assert.IsTrue(engagement.leftTroop.perkPool.ContainsKey(Attacked.id));

        Assert.AreEqual(4, engagement.rightTroop.frontline.Count);
        Assert.AreEqual(-58, engagement.rightTroop.stamina);
        Assert.IsTrue(engagement.rightTroop.perkPool.ContainsKey(Attacked.id));
    }

    // [TestMethod]
    // public void TestStrikeTriggersPerkEngagement()
    // {
    //     var rng = new UnitTestRNG(rngInt: new List<int>(){50});
    //     Stats result = new Stats(1, 2, 3, 4, 5);
    //     Mock<Buff> buff = new Mock<Buff>();
    //     buff.Setup( b => b.GetStats()).Returns(new Stats(2,2,2,2,2));
    //     buff.As<TroopPerk>().Setup( b => b.IsActive()).Returns(true);
    //     ActionHandler handler = new ActionHandler(rng);
    //     TestUnit unit = new TestUnit(result, new TestCaptain().GetCaptain(), new List<Mock<TroopPerk>>(){buff.As<TroopPerk>()});

    //     Engagement engagement = new Engagement();
    // }
}