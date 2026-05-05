using System;
using System.Collections.Generic;
using static Movement;
using static Order;
using static Phase;
using static Troop;

public interface Captain
{   
    public Order GetOrder(TroopContext context, TroopContext antagonist, PhaseType currentPhase);
    public List<Perk> ResolveConflicts(List<Perk> triggeredPerks, List<Perk> activePerks);
    public bool CanStrike();
    public StandingOrder GetMovement(PhaseType currentPhase, PhaseType desiredPhase, MovementType? antagonistMovement = null);
}