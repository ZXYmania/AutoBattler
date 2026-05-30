using Godot;
using Moq;
using static Troop;

[TestClass]
public sealed class TestMatchUps
{
    public static IEnumerable<ValueTuple<Stats, Stats>> GetStats()
    {
        return new List<ValueTuple<Stats,Stats>>()
        {
            new ValueTuple<Stats,Stats>(new Stats(500, 500, 500, 500, 500),new Stats(500, 500, 500, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(500, 500, 1000, 500, 500),new Stats(500, 500, 1000, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(500, 500, 500, 1000, 500),new Stats(500, 500, 500, 1000, 500)),
            new ValueTuple<Stats,Stats>(new Stats(1000, 500, 500, 500, 500),new Stats(1000, 500, 500, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(500, 1000, 500, 500, 500),new Stats(500, 1000, 500, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(1000, 1000, 500, 500, 500),new Stats(500, 500, 1000, 1000, 500)),
            new ValueTuple<Stats,Stats>(new Stats(1000, 500, 500, 500, 500),new Stats(500, 500, 500, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(500, 500, 500, 500, 500),new Stats(1000, 500, 500, 500, 500)),
            new ValueTuple<Stats,Stats>(new Stats(500, 500, 500, 500, 500),new Stats(500, 500, 1000, 500, 500)),

        };
    } 

    [TestMethod]
    [DynamicData(nameof(GetStats), DynamicDataSourceType.Method)]
    public void TestEven(Stats leftStats, Stats rightStats)
    {
        IRandom rng = new UnitTestRNG();

        int index = 0;
        List<int> leftWinner = new List<int>(){0,0};
        List<int> rightWinner = new List<int>(){0,0};
        float averageSquadLeft = 0;
        float averageSquadRight = 0;
        int doubleDown = 0;

        int endurance = 0;
        
        for(int i = 0; i < 1000; i++)
        {
            int currIndex = 0;
            var leftCaptain = new TestCaptain();
            leftCaptain.SetupOrder(Phase.PhaseType.Poke);
            leftCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
            var rightCaptain = new TestCaptain();
            rightCaptain.SetupOrder(Phase.PhaseType.Poke);
            rightCaptain.SetupOnMovement(Movement.MovementType.Stay, 0);
            TestUnit leftUnit = new TestUnit(leftStats, leftCaptain.GetCaptain(), new List<Mock<TroopPerk>>());
            TestUnit rightUnit = new TestUnit(rightStats, rightCaptain.GetCaptain(), new List<Mock<TroopPerk>>());
            Troop leftTroop = new Troop(leftUnit.GetUnit());
            leftTroop.armour = new Armour(){body=500};
            leftTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
            leftTroop.backline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
            Troop rightTroop = new Troop(rightUnit.GetUnit());
            rightTroop.frontline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
            rightTroop.backline = new List<Squad>(){new Squad(1), new Squad(2), new Squad(3), new Squad(4), new Squad(5)};
            rightTroop.armour = new Armour(){body=500};

            Engagement engagement = new Engagement(leftTroop, rightTroop, true, rng);
            engagement.currentPhase = Phase.PhaseType.Poke;
            while(!leftTroop.routed && ! rightTroop.routed)
            {
                engagement.ResolveRound();
                currIndex++;
            }
            index += currIndex;
            if(!leftTroop.routed || ! rightTroop.routed)
            {
                if(leftTroop.routed)
                {
                    if(leftTroop.stats.endurance + leftTroop.stamina < 100)
                    {
                        endurance++;
                        rightWinner[1]++;
                    }
                    else
                    {
                        rightWinner[0]++;
                        averageSquadLeft += (leftTroop.frontline.Count + leftTroop.backline.Count);
                    }
                }
                if(rightTroop.routed)
                {
                    if(rightTroop.stats.endurance + rightTroop.stamina < 100)
                    {
                        endurance++;
                        leftWinner[1]++;
                    }
                    else
                    {
                        leftWinner[0]++;
                        averageSquadRight += (rightTroop.frontline.Count + rightTroop.backline.Count);
                    }
                }
            }
            else
            {
                doubleDown++;
                if(rightTroop.stats.endurance + rightTroop.stamina < 100 || leftTroop.stats.endurance + leftTroop.stamina < 100)
                {
                    endurance++;
                }
                averageSquadLeft += (leftTroop.frontline.Count + leftTroop.backline.Count);
                averageSquadRight += (rightTroop.frontline.Count + rightTroop.backline.Count);
            }
        }
        index = index/1000;
        averageSquadLeft /= rightWinner[0]+doubleDown;
        averageSquadRight /= leftWinner[0]+doubleDown;
    }
}