using Godot;
using System;
using System.Collections.Generic;
using static Movement;
using static MovementHandler;
using static Phase;


public struct Order
{
    public PhaseType currentPhase;

    public Guid troopId;
    public PhaseType desiredPhase {get; private set;} // 4 outside, 3 Outrange, 2 Duel, 1 Engage
    public MovementType movement {get; private set;}
    public int effort {get; private set;}
    public bool protagonist {get; private set;}
    public Captain captain;

    public Order(Guid troopId, Captain captain, PhaseType desiredPhase, PhaseType currentPhase)
    {
        this.currentPhase = currentPhase;
        this.troopId = troopId;
        this.desiredPhase = desiredPhase;
        this.captain = captain;       
    }

    public struct StandingOrder
    {
        public MovementType movement {get; private set;}
        public int effort {get; private set;}
        public StandingOrder(MovementType movement, int effort)
        {
            this.movement = movement;
            this.effort = effort;
        }
    }

    public void SetProtagonist(PhaseType currentPhase)
    {
        StandingOrder standingOrder = captain.GetMovement(currentPhase, desiredPhase);
        movement = standingOrder.movement;
        effort = standingOrder.effort;
        protagonist = true;
    }

    public void SetAntagonist(PhaseType currentPhase, MovementType protagonistMovement)
    {
        StandingOrder standingOrder = captain.GetMovement(currentPhase, desiredPhase, protagonistMovement);
        movement = standingOrder.movement;
        effort = standingOrder.effort;
        protagonist = false;
    }
}