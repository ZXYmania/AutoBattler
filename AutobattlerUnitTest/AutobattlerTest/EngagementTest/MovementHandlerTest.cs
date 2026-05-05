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
            TroopContext context = new TroopContext();
            Order left = new PhaseCaptainTestClass(new List<PhaseType>(){test.leftPhase}).GetOrder(context, context, test.currentPhase);
            Order right = new PhaseCaptainTestClass(new List<PhaseType>(){test.rightPhase}).GetOrder(context, context, test.currentPhase);
            MovementHandler handler = new MovementHandler();
            Tuple<Order,Order> output = handler.InitialiseOrder(left, right, test.currentPhase);
            if(output.Item1.troopId == left.troopId)
            {
                Assert.AreEqual(test, new ProtagonistMovementTestData(output.Item1.desiredPhase, output.Item2.desiredPhase, test.currentPhase, output.Item1.protagonist, output.Item2.protagonist));
            }
            else
            {
                Assert.AreEqual(test, new ProtagonistMovementTestData(output.Item2.desiredPhase, output.Item1.desiredPhase, test.currentPhase, output.Item2.protagonist, output.Item1.protagonist));
            }

        }
    }
    [TestMethod]
    public void TestFleePhaseRemainsFlee()
    {
        List<MovementStep> testData = new List<MovementStep>(){
                new MovementStep(MovementType.Flee, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(MovementType.Flee, MovementType.Charge, PhaseType.Flee),
                new MovementStep(MovementType.Flee, MovementType.March, PhaseType.Flee),
                new MovementStep(MovementType.Flee, MovementType.Advance, PhaseType.Flee),
                new MovementStep(MovementType.Flee, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(MovementType.Flee, MovementType.Stay, PhaseType.Flee),
                new MovementStep(MovementType.Charge, MovementType.Charge, PhaseType.Flee),
                new MovementStep(MovementType.Charge, MovementType.March, PhaseType.Flee),
                new MovementStep(MovementType.Charge, MovementType.Advance, PhaseType.Flee),
                new MovementStep(MovementType.Charge, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(MovementType.Charge, MovementType.Stay, PhaseType.Flee),
                new MovementStep(MovementType.March, MovementType.March, PhaseType.Flee),
                new MovementStep(MovementType.March, MovementType.Advance, PhaseType.Flee),
                new MovementStep(MovementType.March, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(MovementType.March, MovementType.Stay, PhaseType.Flee),
                new MovementStep(MovementType.Advance, MovementType.Advance, PhaseType.Flee),
                new MovementStep(MovementType.Advance, MovementType.Fallback, PhaseType.Flee),
                new MovementStep(MovementType.Advance, MovementType.Stay, PhaseType.Flee),
                new MovementStep(MovementType.Fallback, MovementType.Fallback, PhaseType.Flee)
        };
        foreach(MovementStep step in testData)
        {
            MovementHandler handler = new MovementHandler();
            TroopContext nullContext = new TroopContext();
            nullContext.activePerkList = new List<Perk>();
            Order protagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(step.protagonistResult, 100), PhaseType.OutOfCombat, PhaseType.OutOfCombat);
            Order antagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(step.antagonistResult, 100), PhaseType.OutOfCombat, PhaseType.OutOfCombat);
    
            Assert.AreEqual(step.resultPhase, handler.GetMovementResult(nullContext, protagonistOrder, nullContext, antagonistOrder, step.resultPhase).resultPhase);
        }
    }

    [TestMethod]
    public void GenerateTestData()
    {
        List<OrderTestData> save = new List<OrderTestData>();
        MovementHandler handler = new MovementHandler();
        TroopContext nullContext = new TroopContext();
        nullContext.activePerkList = new List<Perk>();
        foreach(MovementType protagonistMovement in allMovementType)
        {
            foreach(MovementType antagonistMovement in allMovementType)
            {
                foreach(PhaseType currentPhase in allPhasetype)
                {
                    if(GetMovementPriority(protagonistMovement) >= GetMovementPriority(antagonistMovement))
                    {
                        OrderTestData currentData = new OrderTestData();
                        currentData.protagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(protagonistMovement, 10), PhaseType.OutOfCombat, currentData.currentPhase);
                        currentData.antagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(antagonistMovement, 10), PhaseType.OutOfCombat, currentData.currentPhase);
                        currentData.protagonistOrder.SetProtagonist(currentPhase);
                        currentData.antagonistOrder.SetAntagonist(currentPhase, currentData.protagonistOrder.movement);
                        currentData.currentPhase = currentPhase;
                        currentData.result = handler.GetMovementResult(nullContext, currentData.protagonistOrder, nullContext, currentData.antagonistOrder, currentPhase);
                        save.Add(currentData);
                    }
                }
            }
        }
        using(TextWriter writer = new StreamWriter("C:\\Users\\Dean\\Documents\\Autobattler\\AutobattlerUnitTest\\AutobattlerTest\\testData,json", false))
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
