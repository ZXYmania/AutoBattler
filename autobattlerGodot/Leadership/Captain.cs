using System;
using System.Collections.Generic;
using static Movement;
using static Order;
using static Phase;
using static Troop;

public interface Captain
{   
    public Order GetOrder(TroopContext context, TroopContext antagonist, PhaseType currentPhase);
    public bool CanStrike();
    public StandingOrder GetMovement(PhaseType currentPhase, PhaseType desiredPhase, Nullable<MovementType> antagonistMovement = null);
}