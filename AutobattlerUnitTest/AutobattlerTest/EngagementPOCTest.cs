using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Battlefield;
using static Troop;

namespace AutobattlerTest;

[TestClass]
public sealed class EngagementPOCTest
{

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
            test.Add(left.id, left);
            test.Add(right.id, right);
        }

    }


    // [TestMethod]
    // public void TestReference()
    // {
    //     Unit testUnit = new Unit(new Stats(), null, new List<UnitPerk>());
    //     Troop testTroop = new Troop(testUnit);
    //     Troop testRight = new Troop(testUnit);
    //     ExampleBattlefield testBattlefield = new ExampleBattlefield(testTroop, testRight);
    //     StandAlone(testBattlefield.test[testTroop.id]);
    // }

    public List<TroopPerk> GetNullPerks()
    {
        return new List<TroopPerk>();
    } 

    [TestMethod]
    public void TestEquality()
    {
        TroopContext first = new TroopContext{id=Guid.NewGuid()};
        TroopContext second = new TroopContext{id=Guid.NewGuid()};
        if(first == second)
        {
            throw new Exception("False positive");
        }

        TroopContext duplicate = new TroopContext{id = first.id, stats = new Stats(1,1,1,1)};
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

    public struct ListExample
    {
        public List<int> data;
        public Guid id;

    }

    [TestMethod]
    public void TestEqualityDict()
    {
        Guid firstId = Guid.NewGuid();
        Guid secondId = Guid.NewGuid();
        Dictionary<Guid, ListExample> data1 = new Dictionary<Guid, ListExample>()
        {
            {firstId, new ListExample{id=firstId, data=new List<int>(){3,4,5}}},
        };

        Dictionary<Guid, ListExample> data2 = new Dictionary<Guid, ListExample>()
        {
            {firstId, new ListExample{id= firstId, data=new List<int>(){4,6}}},
            {secondId, new ListExample{id=secondId, data=new List<int>(){1,2}}}
        };
        
        List<Guid> test = data1.Keys.Intersect(data2.Keys).ToList();

    }
}
