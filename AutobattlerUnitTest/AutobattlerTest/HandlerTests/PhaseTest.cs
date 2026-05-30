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
        new DifferenceTestData(PhaseType.Duel, PhaseType.OutOfCombat, MovementType.Disengage),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Poke, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Duel, MovementType.Stay),
        new DifferenceTestData(PhaseType.Duel, PhaseType.Engage, MovementType.Advance),
        new DifferenceTestData(PhaseType.Engage, PhaseType.OutOfCombat, MovementType.Disengage),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Poke, MovementType.Disengage),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Duel, MovementType.Fallback),
        new DifferenceTestData(PhaseType.Engage, PhaseType.Engage, MovementType.Stay),
    };

    [TestMethod]
    public void TestDifferenceBetweenPhaseType()
    {
        foreach(DifferenceTestData testData in differenceTestDataList)
        {
            Assert.AreEqual(testData.expected,DifferenceBetweenPhaseType(testData.currentPhase, testData.desiredPhase));
        }
    }

    public struct MovementPriorityTestData
    {
        public MovementType leftMovement;
        public MovementType rightMovement;
        public MovementType expectedMovement;
        public MovementPriorityTestData(MovementType leftMovement, MovementType rightMovement, MovementType expected)
        {
            this.leftMovement = leftMovement;
            this.rightMovement = rightMovement;
            this.expectedMovement = expected;
        }

        public override string ToString()
        {
            return "Movement: " + leftMovement +", Phase: " + rightMovement + ", Result: " + expectedMovement;
        }
    }

    public List<MovementPriorityTestData> movmentPriorityTestDataList = new List<MovementPriorityTestData>()
    {

        new MovementPriorityTestData(MovementType.Flee, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.Charge, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.March, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.Advance, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.Stay, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.Fallback, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Flee, MovementType.Disengage, MovementType.Flee),
        
        new MovementPriorityTestData(MovementType.Charge, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Charge, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Charge, MovementType.March, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Charge, MovementType.Advance, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Charge, MovementType.Stay, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Charge, MovementType.Fallback, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Charge, MovementType.Disengage, MovementType.Charge),

        new MovementPriorityTestData(MovementType.March, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.March, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.March, MovementType.March, MovementType.March),
        new MovementPriorityTestData(MovementType.March, MovementType.Advance, MovementType.March),
        new MovementPriorityTestData(MovementType.March, MovementType.Stay, MovementType.March),
        new MovementPriorityTestData(MovementType.March, MovementType.Fallback, MovementType.March),
        new MovementPriorityTestData(MovementType.March, MovementType.Disengage, MovementType.March),
        
        new MovementPriorityTestData(MovementType.Advance, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Advance, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Advance, MovementType.March, MovementType.March),
        new MovementPriorityTestData(MovementType.Advance, MovementType.Advance, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Advance, MovementType.Stay, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Advance, MovementType.Fallback, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Advance, MovementType.Disengage, MovementType.Advance),

        new MovementPriorityTestData(MovementType.Fallback, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.March, MovementType.March),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.Advance, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.Stay, MovementType.Fallback),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.Fallback, MovementType.Fallback),
        new MovementPriorityTestData(MovementType.Fallback, MovementType.Disengage, MovementType.Disengage),

        new MovementPriorityTestData(MovementType.Disengage, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.March, MovementType.March),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.Advance, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.Stay, MovementType.Disengage),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.Fallback, MovementType.Disengage),
        new MovementPriorityTestData(MovementType.Disengage, MovementType.Disengage, MovementType.Disengage),

        new MovementPriorityTestData(MovementType.Stay, MovementType.Flee, MovementType.Flee),
        new MovementPriorityTestData(MovementType.Stay, MovementType.Charge, MovementType.Charge),
        new MovementPriorityTestData(MovementType.Stay, MovementType.March, MovementType.March),
        new MovementPriorityTestData(MovementType.Stay, MovementType.Advance, MovementType.Advance),
        new MovementPriorityTestData(MovementType.Stay, MovementType.Stay, MovementType.Stay),
        new MovementPriorityTestData(MovementType.Stay, MovementType.Fallback, MovementType.Fallback),
        new MovementPriorityTestData(MovementType.Stay, MovementType.Disengage, MovementType.Disengage),

    };

    [TestMethod]
    public void TestMovementPriority()
    {
        MockUnitTestRNG rng = new MockUnitTestRNG(new List<float>(), new List<int>());
        foreach(MovementPriorityTestData testData in movmentPriorityTestDataList)
        {
            if(GetMovementPriority(testData.leftMovement) < GetMovementPriority(testData.rightMovement))
            {
                Assert.AreEqual(testData.expectedMovement, testData.leftMovement);
            }
            else
            {
                Assert.AreEqual(testData.expectedMovement, testData.rightMovement);
            }
        }
    }
}