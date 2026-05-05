using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Movement;
using static Phase;
using static Troop;

[TestClass]
public sealed class PhaseTest
{
    public struct DifferenceTestData
    {
        public PhaseType currentPhase;
        public PhaseType desiredPhase;
        public MovementType expected;
        public DifferenceTestData(PhaseType leftPhase, PhaseType rightPhase, MovementType expected)
        {
            this.currentPhase = leftPhase;
            this.desiredPhase = rightPhase;
            this.expected = expected;
        }
    }

    public List<DifferenceTestData> differenceTestDataList = new List<DifferenceTestData>()
    {
        new DifferenceTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, MovementType.Stay),
        new DifferenceTestData(PhaseType.OutOfCombat, PhaseType.Poke, MovementType.Advance),
        new DifferenceTestData(PhaseType.OutOfCombat, PhaseType.Duel, MovementType.March),
        new DifferenceTestData(PhaseType.OutOfCombat, PhaseType.Engage, MovementType.Charge),
        new DifferenceTestData(PhaseType.Poke, PhaseType.OutOfCombat, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Poke, PhaseType.Poke, MovementType.Stay),
        new DifferenceTestData(PhaseType.Poke, PhaseType.Duel, MovementType.Advance),
        new DifferenceTestData(PhaseType.Poke, PhaseType.Engage, MovementType.March),
        new DifferenceTestData(PhaseType.Duel, PhaseType.OutOfCombat, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Poke, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Duel, MovementType.Stay),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Engage, MovementType.Advance),
        new DifferenceTestData(PhaseType.Engage, PhaseType.OutOfCombat, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Poke, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Duel, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Engage, MovementType.Stay),
    };

    [TestMethod]
    public void TestDifferenceBetweenPhaseType()
    {
        foreach(DifferenceTestData testData in differenceTestDataList)
        {
            Assert.AreEqual(DifferenceBetweenPhaseType(testData.currentPhase, testData.desiredPhase),testData.expected);
        }
    }

    public struct MovementPriorityTestData
    {
        public MovementType movement;
        public PhaseType currentPhase;
        public int expected;
        public MovementPriorityTestData(MovementType movement, PhaseType currentPhase, int expected)
        {
            this.movement = movement;
            this.currentPhase = currentPhase;
            this.expected = expected;
        }

        public override string ToString()
        {
            return "Movement: " + movement +", Phase: " + currentPhase + ", Result: " + expected;
        }
    }

    public List<MovementPriorityTestData> movmentPriorityTestDataList = new List<MovementPriorityTestData>()
    {

        // new MovementPriorityTestData(PhaseType.OutOfCombat, PhaseType.Poke, MovementType.Advance),
        // new MovementPriorityTestData(PhaseType.OutOfCombat, PhaseType.Duel, MovementType.March),
        // new MovementPriorityTestData(PhaseType.OutOfCombat, PhaseType.Engage, MovementType.Charge),
        // new MovementPriorityTestData(PhaseType.Poke, PhaseType.OutOfCombat, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Poke, PhaseType.Poke, MovementType.Stay),
        // new MovementPriorityTestData(PhaseType.Poke, PhaseType.Duel, MovementType.Advance),
        // new MovementPriorityTestData(PhaseType.Poke, PhaseType.Engage, MovementType.March),
        // new MovementPriorityTestData(PhaseType.Duel, PhaseType.OutOfCombat, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Duel, PhaseType.Poke, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Duel, PhaseType.Duel, MovementType.Stay),
        // new MovementPriorityTestData(PhaseType.Duel, PhaseType.Engage, MovementType.Advance),
        // new MovementPriorityTestData(PhaseType.Engage, PhaseType.OutOfCombat, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Engage, PhaseType.Poke, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Engage, PhaseType.Duel, MovementType.Fallback),
        // new MovementPriorityTestData(PhaseType.Engage, PhaseType.Engage, MovementType.Stay),
    };

    [TestMethod]
    public void TestMovementPriority()
    {
        MovementHandler handler = new MovementHandler();
        foreach(MovementPriorityTestData testData in movmentPriorityTestDataList)
        {
            Assert.AreEqual(testData.expected, GetMovementPriority(testData.movement));
        }
    }
}