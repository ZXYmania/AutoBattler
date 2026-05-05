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

    public struct OrderTestData2
    {
        PhaseType currentPhase;
        Order protagonistOrder;
        Order antagonistOrder;
        PhaseType expected;

        public OrderTestData2(PhaseType currentPhase,
                            MovementType protagonistMovement,
                             MovementType antagonistMovement,
                             PhaseType expectedPhase, 
                             int protagonistEffort = 10, int antagonistEffort = 10)
        {
            this.currentPhase = currentPhase;
            protagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(protagonistMovement, protagonistEffort), PhaseType.OutOfCombat, PhaseType.OutOfCombat);
            antagonistOrder = new Order(Guid.NewGuid(), new MovementCaptainTestClass(antagonistMovement, antagonistEffort), PhaseType.OutOfCombat, PhaseType.OutOfCombat);
            protagonistOrder.SetProtagonist(PhaseType.OutOfCombat);
            antagonistOrder.SetAntagonist(PhaseType.OutOfCombat, protagonistOrder.movement);
                        expected = expectedPhase;
        }

        // public List<OrderTestData> allOrders = new List<OrderTestData>()
        // {
        //     new MovementStep(MovementType.Stay, MovementType.Stay)
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.Fallback, MovementType.Fallback, MovementType.Fallback, PhaseType.Flee),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.Charge),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.March),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.Advance),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.Fallback),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Flee, MovementType.Stay),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Charge, MovementType.Charge),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Charge, MovementType.March),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Charge, MovementType.Advance),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Charge, MovementType.Fallback),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Charge, MovementType.Stay),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.March, MovementType.March),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.March, MovementType.Advance),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.March, MovementType.Fallback),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.March, MovementType.Stay),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Advance, MovementType.Advance),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Advance, MovementType.Fallback),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Advance, MovementType.Stay),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Fallback, MovementType.Fallback),
        //     new OrderTestData(PhaseType.OutOfCombat, MovementType.Stay, MovementType.Stay),

        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.Fallback, MovementType.Fallback, MovementType.Fallback, PhaseType.Flee),
        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.Charge),
        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.March),
        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.Advance),
        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Poke, MovementType.Flee, MovementType.Stay),
        //     new OrderTestData(PhaseType.Poke, MovementType.Charge, MovementType.Charge),
        //     new OrderTestData(PhaseType.Poke, MovementType.Charge, MovementType.March),
        //     new OrderTestData(PhaseType.Poke, MovementType.Charge, MovementType.Advance),
        //     new OrderTestData(PhaseType.Poke, MovementType.Charge, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Poke, MovementType.Charge, MovementType.Stay),
        //     new OrderTestData(PhaseType.Poke, MovementType.March, MovementType.March),
        //     new OrderTestData(PhaseType.Poke, MovementType.March, MovementType.Advance),
        //     new OrderTestData(PhaseType.Poke, MovementType.March, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Poke, MovementType.March, MovementType.Stay),
        //     new OrderTestData(PhaseType.Poke, MovementType.Advance, MovementType.Advance),
        //     new OrderTestData(PhaseType.Poke, MovementType.Advance, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Poke, MovementType.Advance, MovementType.Stay),
        //     new OrderTestData(PhaseType.Poke, MovementType.Fallback, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Poke, MovementType.Stay, MovementType.Stay),

        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.Fallback, MovementType.Fallback, MovementType.Fallback, PhaseType.Flee),
        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.Charge),
        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.March),
        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.Advance),
        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Duel, MovementType.Flee, MovementType.Stay),
        //     new OrderTestData(PhaseType.Duel, MovementType.Charge, MovementType.Charge),
        //     new OrderTestData(PhaseType.Duel, MovementType.Charge, MovementType.March),
        //     new OrderTestData(PhaseType.Duel, MovementType.Charge, MovementType.Advance),
        //     new OrderTestData(PhaseType.Duel, MovementType.Charge, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Duel, MovementType.Charge, MovementType.Stay),
        //     new OrderTestData(PhaseType.Duel, MovementType.March, MovementType.March),
        //     new OrderTestData(PhaseType.Duel, MovementType.March, MovementType.Advance),
        //     new OrderTestData(PhaseType.Duel, MovementType.March, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Duel, MovementType.March, MovementType.Stay),
        //     new OrderTestData(PhaseType.Duel, MovementType.Advance, MovementType.Advance),
        //     new OrderTestData(PhaseType.Duel, MovementType.Advance, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Duel, MovementType.Advance, MovementType.Stay),
        //     new OrderTestData(PhaseType.Duel, MovementType.Fallback, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Duel, MovementType.Stay, MovementType.Stay),

        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.Fallback, MovementType.Fallback, MovementType.Fallback, PhaseType.Flee),
        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.Charge),
        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.March),
        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.Advance),
        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Engage, MovementType.Flee, MovementType.Stay),
        //     new OrderTestData(PhaseType.Engage, MovementType.Charge, MovementType.Charge),
        //     new OrderTestData(PhaseType.Engage, MovementType.Charge, MovementType.March),
        //     new OrderTestData(PhaseType.Engage, MovementType.Charge, MovementType.Advance),
        //     new OrderTestData(PhaseType.Engage, MovementType.Charge, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Engage, MovementType.Charge, MovementType.Stay),
        //     new OrderTestData(PhaseType.Engage, MovementType.March, MovementType.March),
        //     new OrderTestData(PhaseType.Engage, MovementType.March, MovementType.Advance),
        //     new OrderTestData(PhaseType.Engage, MovementType.March, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Engage, MovementType.March, MovementType.Stay),
        //     new OrderTestData(PhaseType.Engage, MovementType.Advance, MovementType.Advance),
        //     new OrderTestData(PhaseType.Engage, MovementType.Advance, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Engage, MovementType.Advance, MovementType.Stay),
        //     new OrderTestData(PhaseType.Engage, MovementType.Fallback, MovementType.Fallback),
        //     new OrderTestData(PhaseType.Engage, MovementType.Stay, MovementType.Stay),
        // };
    }
}