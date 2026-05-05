using System.Collections.Generic;
using static Engagement;
using static Movement;
using static MovementHandler;
using static Phase;

public struct MovementStartTrigger : TriggerType
{
    public MovementStartTrigger(HashSet<PhaseType> phase, HashSet<MovementType> protagonistMovement, HashSet<MovementType> antagonistMovement)
    {
        this.phase = phase;
        this.protagonistMovement = protagonistMovement;
        this.antagonistMovement = antagonistMovement;
    }

    public HashSet<PhaseType> phase;
    public HashSet<MovementType> protagonistMovement;
    public HashSet<MovementType> antagonistMovement;
    public bool TriggerMet(TriggerType trigger)
    {
        if(trigger is MovementStartTrigger movementTrigger)
        {
            return movementTrigger.phase.IsSubsetOf(phase) &&
            movementTrigger.protagonistMovement.IsSubsetOf(protagonistMovement) &&
            movementTrigger.antagonistMovement.IsSubsetOf(antagonistMovement);
        }
        return false;
    }
}

public interface AffectsMovement
{
    public MovementStep GetGap(TroopAction resultOfAction);
    public PhaseType GetActivationPhase();
    public MovementModifier GetMovementModifier();
    public ActorType MovementAppliedTo();

    public struct MovementModifier
    {
        public int value;
        public MovementModifierType type;
        public bool? overrideSuccess;
        // public MovementType movement;
    }
    
    public enum MovementModifierType
    {
        Additive,
        Multiplicative,
        Override
    }
}