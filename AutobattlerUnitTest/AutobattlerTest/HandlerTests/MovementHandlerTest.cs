using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Movement;
using static MovementHandlerTestData;
using static Phase;
using static Troop;

[TestClass]
public sealed class MovementHandlerTest
{

    [TestMethod]
    public void GetProtagonistForEachPhaseType()
    {
        foreach(ProtagonistMovementTestData test in ProtagonistMovementTestData.allProtagonistMovement)
        {
            TroopContext leftContext = new TroopContext(){id=Guid.NewGuid()};
            TroopContext rightContext = new TroopContext(){id=Guid.NewGuid()};
            MockUnitTestRNG rng = new MockUnitTestRNG(new List<float>(), new List<int>());
            TestCaptain protagonistCaptain = new TestCaptain();
            protagonistCaptain.SetupOrder(test.leftPhase);
            TestCaptain antagonistCaptain = new TestCaptain();
            antagonistCaptain.SetupOrder(test.rightPhase);
            Order left = protagonistCaptain.GetCaptain().GetOrder(leftContext, rightContext, test.currentPhase);
            Order right = antagonistCaptain.GetCaptain().GetOrder(rightContext, leftContext, test.currentPhase);
            MovementOrders output = new MovementOrders(left, right, test.currentPhase);
            if(left.troopId == output.protagonistOrder.troopId)
            {
                Assert.AreEqual(test, new ProtagonistMovementTestData(output.protagonistOrder.desiredPhase, output.antagonistOrder.desiredPhase, test.currentPhase, output.protagonistOrder.protagonist, output.antagonistOrder.protagonist));
            }
            else
            {
                Assert.AreEqual(test, new ProtagonistMovementTestData(output.antagonistOrder.desiredPhase, output.protagonistOrder.desiredPhase, test.currentPhase, output.antagonistOrder.protagonist, output.protagonistOrder.protagonist));
            }
        }
    }
    
    [TestMethod]
    public void TestFleePhaseRemainsFlee()
    {
        Guid protagonist = Guid.NewGuid();
        List<MovementStep> testData = new List<MovementStep>(){
                new MovementStep(protagonist, MovementType.Flee, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Flee, MovementType.Charge, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Flee, MovementType.March, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Flee, MovementType.Advance, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Flee, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Flee, MovementType.Stay, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Charge, MovementType.Charge, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Charge, MovementType.March, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Charge, MovementType.Advance, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Charge, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(protagonist, MovementType.Charge, MovementType.Stay, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.March, MovementType.March, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.March, MovementType.Advance, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.March, MovementType.Fallback, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.March, MovementType.Stay, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.Advance, MovementType.Advance, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.Advance, MovementType.Fallback, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.Advance, MovementType.Stay, PhaseType.Flee),
                new MovementStep( protagonist,MovementType.Fallback, MovementType.Fallback, PhaseType.Flee)
        };
        foreach(MovementStep step in testData)
        {
            TestCaptain protagonistCaptain = new TestCaptain();
            protagonistCaptain.SetupOnMovement(step.protagonistResult);
            TestCaptain antagonistCaptain = new TestCaptain();
            antagonistCaptain.SetupOnMovement(step.antagonistResult);

            MockUnitTestRNG rng = new MockUnitTestRNG(new List<float>(), new List<int>());
            var currentPhase = PhaseType.OutOfCombat;
            TroopContext nullContext = new TroopContext();
            Order protagonistOrder = protagonistCaptain.GetOrder(currentPhase);
            Order antagonistOrder = antagonistCaptain.GetOrder(currentPhase);
    
            Assert.AreEqual(step.resultPhase, new MovementStep(nullContext, protagonistOrder, nullContext, antagonistOrder, step.resultPhase, rng).resultPhase);
        }
    }

    [TestMethod]
    [Ignore("Mock Captain breaks the data creator")]
    public void GenerateTestData()
    {
        List<OrderTestData> save = new List<OrderTestData>();
        MockUnitTestRNG rng = new MockUnitTestRNG(new List<float>(), new List<int>());
        TroopContext nullContext = new TroopContext();
        foreach(MovementType protagonistMovement in allMovementType)
        {
            foreach(MovementType antagonistMovement in allMovementType)
            {
                foreach(PhaseType currentPhase in allPhasetype)
                {
                    if(GetMovementPriority(protagonistMovement) >= GetMovementPriority(antagonistMovement))
                    {
                        TestCaptain protagonistCaptain = new TestCaptain();
                        protagonistCaptain.SetupOnMovement(protagonistMovement);
                        TestCaptain antagonistCaptain = new TestCaptain();
                        antagonistCaptain.SetupOnMovement(antagonistMovement);
                        OrderTestData currentData = new OrderTestData()
                        {
                            protagonistOrder = protagonistCaptain.GetCaptain().GetOrder(new TroopContext(), new TroopContext(), PhaseType.OutOfCombat),
                            antagonistOrder = antagonistCaptain.GetCaptain().GetOrder(new TroopContext(), new TroopContext(), PhaseType.OutOfCombat)
                        };                        
                        currentData.protagonistOrder.SetProtagonist(currentPhase);
                        currentData.antagonistOrder.SetAntagonist(currentPhase, currentData.protagonistOrder.movement);
                        currentData.currentPhase = currentPhase;
                        currentData.result = new MovementStep(nullContext, currentData.protagonistOrder, nullContext, currentData.antagonistOrder, currentPhase, rng);
                        save.Add(currentData);
                    }
                }
            }
        }
        using(TextWriter writer = new StreamWriter(System.IO.Directory.GetCurrentDirectory()+"testData,json", false))
        {
            var contents = JsonConvert.SerializeObject(save);
            writer.Write(contents);
        }


    }
    // [TestMethod]
    // public void ResolveMovementStepTest()
    // {
    //     MovementHandler handler = new MovementHandler();
    //     handler.ResolveMovementStep();
    // }

}
