using System.Collections.Generic;
using System.Linq;
using static Movement;
using static Order;
using static Phase;
using static Troop;

public class TestUnit : Unit
{
    public TestUnit(Stats givenStats, Captain? givenCaptain)
    {
        
    }
}

public class PhaseCaptainTestClass : Captain
{
    int round;
    List<PhaseType> desiredPhaseList;
    public PhaseCaptainTestClass(List<PhaseType> desiredPhaseList)
    {
        round = 0;
        this.desiredPhaseList = desiredPhaseList;
    }
    public bool CanStrike()
    {
        return true;
    }

    public StandingOrder GetMovement(Phase.PhaseType currentPhase, Phase.PhaseType desiredPhase, MovementType? antagonistMovement = null)
    {
        return new StandingOrder(DifferenceBetweenPhaseType(currentPhase, desiredPhase), 10);
    }

    public Order GetOrder(TroopContext context, TroopContext antagonist, PhaseType currentPhase)
    {
        return new Order(Guid.NewGuid(), this, desiredPhaseList[round], currentPhase);
    }

    public List<Perk> ResolveConflicts(List<Perk> triggeredPerks, List<Perk> activePerks)
    {
        throw new NotImplementedException();
    }
}

public class MovementCaptainTestClass : Captain
{
    public MovementType movement;
    public int effort;
    public MovementCaptainTestClass(MovementType movement, int effort)
    {
        this.movement = movement;
        this.effort = effort;
    }
    public bool CanStrike()
    {
        return true;
    }

    public StandingOrder GetMovement(PhaseType currentPhase, PhaseType desiredPhase, MovementType? antagonistMovement = null)
    {
        return new StandingOrder(movement, effort);
    }

    public Order GetOrder(TroopContext context, TroopContext antagonist, PhaseType currentPhase)
    {
        throw new NotImplementedException();
    }

    public List<Perk> ResolveConflicts(List<Perk> triggeredPerks, List<Perk> activePerks)
    {
        throw new NotImplementedException();
    }
}