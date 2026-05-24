using static Movement;
using static MovementHandler;
using static Phase;

public class MovementHandlerTestData
{
    public static List<MovementType> allMovementType = new List<MovementType>()
    {
        MovementType.Flee,
        MovementType.Disengage,
        MovementType.Fallback,
        MovementType.Stay,
        MovementType.Advance,
        MovementType.March,
        MovementType.Charge,
    };

    public static List<PhaseType> allPhasetype = new List<PhaseType>()
    {
        PhaseType.Flee,
        PhaseType.OutOfCombat,
        PhaseType.Poke,
        PhaseType.Duel,
        PhaseType.Engage
    };
    // public struct PhaseTestData
    // {
    //     PhaseType leftPhase;
    //     PhaseType rightPhase;
    //     PhaseType currentPhase;
    //     public PhaseTestData(PhaseType leftPhase, PhaseType rightPhase, PhaseType currentPhase)
    //     {
    //         this.leftPhase = leftPhase;
    //         this.rightPhase = rightPhase;
    //         this.currentPhase = currentPhase
    //     }

    //     public static List<PhaseTestData> allPhasesTestData = new List<PhaseTestData>()
    //     {
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Engage),
            
    //         new PhaseTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Engage),

    //         new PhaseTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Engage),

    //         new PhaseTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Engage),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.OutOfCombat),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Poke),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Duel),
    //         new PhaseTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Engage),
    //     };
    // }

    public struct ProtagonistMovementTestData
    {
        public PhaseType leftPhase;
        public PhaseType rightPhase;
        public PhaseType currentPhase;
        public bool leftIsProtagonist;
        public bool rightIsProtagonist;
        
        public ProtagonistMovementTestData(PhaseType leftPhase, PhaseType rightPhase, PhaseType currentPhase, bool leftIsProtagonist, bool rightIsProtagonist)
        {
            this.leftPhase = leftPhase;
            this.rightPhase = rightPhase;
            this.currentPhase = currentPhase;
            this. leftIsProtagonist = leftIsProtagonist;
            this.rightIsProtagonist = rightIsProtagonist;
        }
        public override string ToString()
        {
            return "Left: " + leftPhase +", " + leftIsProtagonist+", " + "Right: " + rightPhase + ", " +rightIsProtagonist+ ", CurrentPhase: " + currentPhase;
        }
        
        public static List<ProtagonistMovementTestData> allProtagonistMovement = new List<ProtagonistMovementTestData>()
        {
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.OutOfCombat, false, false),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.OutOfCombat, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Poke, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Duel, PhaseType.Engage, true, false),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.OutOfCombat, PhaseType.Engage, PhaseType.Engage, true, false),
            
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.OutOfCombat, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Poke, false, false),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Poke, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Duel, PhaseType.Engage, true, false),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Poke, PhaseType.Engage, PhaseType.Engage, true, false),

            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.OutOfCombat, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Poke, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Duel, false, false),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Duel, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Duel, PhaseType.Engage, PhaseType.Engage, true, false),

            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.OutOfCombat, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Poke, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.OutOfCombat, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Poke, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Duel, true, false),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Duel, PhaseType.Engage, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.OutOfCombat, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Poke, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Duel, false, true),
            new ProtagonistMovementTestData(PhaseType.Engage, PhaseType.Engage, PhaseType.Engage, false, false),

        };
    }

    public struct OrderTestData
    {
        public Order protagonistOrder;
        public Order antagonistOrder;
        public PhaseType currentPhase;
        public MovementStep result;
    }
}