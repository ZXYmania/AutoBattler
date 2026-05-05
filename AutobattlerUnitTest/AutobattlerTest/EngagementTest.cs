using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Battlefield;
using static Troop;

namespace AutobattlerTest;

[TestClass]
public sealed class EngagementTest
{
    // [TestMethod]
    // public void TestMethod1()
    // {
    //     List<Captain> combattants = new List<Captain>{ new TestCaptain(), new TestCaptain() };
    //     Engagement test = new Engagement(combattants);
    //     test.ResolveRound();
        
    // }

    public class ExampleBattlefield
    {
        public Dictionary<Guid, Troop> test = new Dictionary<Guid, Troop>();
        public Troop reference;
        private Troop UpdateTroopCall(Guid troopId, Troop givenTroop)
        {
            test[troopId] = givenTroop;
            return test[troopId];
        }
        private Troop GetTroopCall(Guid troopId)
        {
            return test[troopId];
        }
        public ExampleBattlefield(Troop left, Troop right)
        {
            reference = left;
            test.Add(left.context.id, left);
            test.Add(right.context.id, right);
        }

    }


    [TestMethod]
    public void TestReference()
    {
        Unit testUnit = new TestUnit(new Stats(), null);
        Troop testTroop = new Troop(testUnit);
        Troop testRight = new Troop(testUnit);
        ExampleBattlefield testBattlefield = new ExampleBattlefield(testTroop, testRight);
        StandAlone(testBattlefield.test[testTroop.context.id]);
    }

    [TestMethod]
    public void TestEquality()
    {
        TroopContext first = new TroopContext(Guid.NewGuid(), new Stats(0,0,0,0), new List<Perk>());
        TroopContext second = new TroopContext(Guid.NewGuid(), new Stats(0,0,0,0), new List<Perk>());
        if(first == second)
        {
            throw new Exception("False positive");
        }

        TroopContext duplicate = new TroopContext(first.id, new Stats(1,1,1,1), new List<Perk>());
        if(first != duplicate)
        {
            throw new Exception("False Negative");
        }
        first.id = Guid.NewGuid();
        if(first == duplicate)
        {
            throw new Exception("False positive");
        }
    }

    public void StandAlone(Troop test)
    {
        test.UnitTest(new Stats(0,7,0,0,0));
    }
}
