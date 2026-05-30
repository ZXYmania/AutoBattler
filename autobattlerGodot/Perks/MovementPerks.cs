using System;
using static Phase;

public interface AffectsMovement
{
    public Nullable<MovementModifier> GetMovementModifier();

    public struct MovementModifier
    {
        public Guid troopId;
        public int value;
        public MovementModifierType type;
        public Nullable<bool> overrideSuccess;
        public PhaseType currentPhase;
        // public MovementType movement;
    }
    
    public enum MovementModifierType
    {
        Additive,
        Multiplicative,
        Override
    }
}