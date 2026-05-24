using Moq;
using static Movement;
using static Phase;
using static Troop;

public class TestCaptain
{
    public Mock<Captain> captain;
    public int round;
    public List<PhaseType> desiredPhaseType;
    public List<MovementType> movementOrders;
    public TestCaptain()
    {
        captain = new Mock<Captain>();
        round = 0;
        desiredPhaseType = new List<PhaseType>();
        movementOrders = new List<MovementType>();

    }
    
    public Captain GetCaptain()
    {
        return captain.Object;
    }

        public void SetupOrder(PhaseType phaseList)
    {
        desiredPhaseType = new List<PhaseType>(){phaseList};
        captain.Setup(c => c.GetOrder(
            It.IsAny<TroopContext>(), 
            It.IsAny<TroopContext>(),
            It.IsAny<PhaseType>()
            )
        ).Returns((TroopContext me, TroopContext enemy, PhaseType currentPhase) => new Order(me.id, captain.Object, desiredPhaseType[round], currentPhase));
    }

    public void SetupOrder(List<PhaseType> phaseList)
    {
        desiredPhaseType = phaseList;
        captain.Setup(c => c.GetOrder(
            It.IsAny<TroopContext>(), 
            It.IsAny<TroopContext>(),
            It.IsAny<PhaseType>()
            )
        ).Returns((TroopContext me, TroopContext enemy, PhaseType currentPhase) => new Order(me.id, captain.Object, desiredPhaseType[round], currentPhase));
    }

    public void SetupOnMovement(MovementType movement, int effort = 5)
    {
        movementOrders = new List<MovementType>(){movement};
        captain.Setup(c => c.GetMovement(
            It.IsAny<PhaseType>(), 
            It.IsAny<PhaseType>(),
            It.IsAny<MovementType?>())
        ).Returns(new Order.StandingOrder(movementOrders[round], effort));
    }

    public void SetupOnMovement(List<MovementType> movementList, int effort = 5)
    {
        movementOrders = movementList;
        captain.Setup(c => c.GetMovement(
            It.IsAny<PhaseType>(), 
            It.IsAny<PhaseType>(),
            It.IsAny<MovementType?>())
        ).Returns(new Order.StandingOrder(movementOrders[round], effort));
    }
}