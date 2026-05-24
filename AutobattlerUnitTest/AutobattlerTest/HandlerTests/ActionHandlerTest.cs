using static Strike;
using static Troop;

[TestClass]
public sealed class ActionHandlerTest
{
    [TestMethod]
    public void TestCreateStrike()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        Squad squad = new Squad(1);
        TroopContext left = new TroopContext(){frontline=new List<Squad>(){squad}};
        TroopContext right = new TroopContext();
        var strike = handler.DoStrike(left, right, Phase.PhaseType.Duel);
        Assert.AreEqual(5, strike.hit[squad].effort);
        Assert.AreEqual(BodyPart.Arm, strike.hit[squad].area);
    }

    public static IEnumerable<ValueTuple<TroopContext, TroopContext, int, BodyPart>> GetDefaultStrikeTable()
    {
        var defaultProtagonist = new TroopContext(){frontline=new List<Squad>(){new Squad(1)}};
        return new List<(TroopContext, TroopContext, int, BodyPart)>()
        {
            new (defaultProtagonist, new TroopContext(), 100, BodyPart.Head),
            new (defaultProtagonist, new TroopContext(), 90, BodyPart.Body),
            new (defaultProtagonist, new TroopContext(), 60, BodyPart.Arm),
            new (defaultProtagonist, new TroopContext(), 30, BodyPart.Leg),
            new (defaultProtagonist, new TroopContext(), 0, BodyPart.Shield)
        };
    }

    [TestMethod]
    [DynamicData(nameof(GetDefaultStrikeTable), DynamicDataSourceType.Method)]
    public void TestStrikeHitCorrect(TroopContext left, TroopContext right, int effort, BodyPart expectedHit)
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){effort});
        ActionHandler handler = new ActionHandler(rng);
        var strike = handler.DoStrike(left, right, Phase.PhaseType.Duel);
        if(left.frontline != null && left.frontline.Count() == 1)
        {
            Assert.AreEqual(expectedHit, strike.hit[left.frontline.First()].area);
        }
        else
        {
            throw new NotImplementedException("Invalid test data of " + left);  
        }
    }

    [TestMethod]
    public void TestGetChanceStrikeEqualBlock()
    {
        TroopContext left = new TroopContext(){frontline=new List<Squad>(){new Squad()}};
        TroopContext right = new TroopContext();
        Assert.AreEqual(new StrikeChance{head=10, body=40, arm=70, leg=100}, Strike.GetStrikeValues(left.stats, right.stats));
        TestStrikeHitCorrect(left, right, 100, BodyPart.Head);
        TestStrikeHitCorrect(left, right, 90, BodyPart.Body);
        TestStrikeHitCorrect(left, right, 60, BodyPart.Arm);
        TestStrikeHitCorrect(left, right, 30, BodyPart.Leg);
        TestStrikeHitCorrect(left, right, 0, BodyPart.Shield);
    }

    [TestMethod]
    public void TestGetChanceStrikeGreaterBlock()
    {
        TroopContext left = new TroopContext(){stats=new Stats{strike=800}, frontline=new List<Squad>(){new Squad()}};
        TroopContext right = new TroopContext();
        Assert.AreEqual(new StrikeChance{head=20, body=80, arm=140, leg=200}, Strike.GetStrikeValues(left.stats, right.stats));
        TestStrikeHitCorrect(left, right, 100, BodyPart.Head);
        TestStrikeHitCorrect(left, right, 80, BodyPart.Body);
        TestStrikeHitCorrect(left, right, 20, BodyPart.Arm);
    }
    
    [TestMethod]
    public void TestGetChanceStrikeGreaterBlockOverflow()
    {
        TroopContext left = new TroopContext(){stats=new Stats{strike=1000},frontline=new List<Squad>(){new Squad()}};
        TroopContext right = new TroopContext();
        Assert.AreEqual(new StrikeChance{head=20, body=80, arm=140, leg=200}, Strike.GetStrikeValues(left.stats, right.stats));
        TestStrikeHitCorrect(left, right, 100, BodyPart.Head);
        TestStrikeHitCorrect(left, right, 80, BodyPart.Body);
        TestStrikeHitCorrect(left, right, 20, BodyPart.Arm);
    }

    [TestMethod]
    public void TestGetChanceStrikeLowerBlock()
    {
        TroopContext left = new TroopContext(){frontline=new List<Squad>(){new Squad()}};
        TroopContext right = new TroopContext(){stats=new Stats{block=700}};
        Assert.AreEqual(new StrikeChance{head=1, body=4, arm=7, leg=10}, Strike.GetStrikeValues(left.stats, right.stats));
        TestStrikeHitCorrect(left, right, 100, BodyPart.Head);
        TestStrikeHitCorrect(left, right, 99, BodyPart.Body);
        TestStrikeHitCorrect(left, right, 96, BodyPart.Arm);
        TestStrikeHitCorrect(left, right, 93, BodyPart.Leg);
        TestStrikeHitCorrect(left, right, 90, BodyPart.Shield);
    }

    [TestMethod]
    public void TestGetChanceStrikeLowerBlockOverflow()
    {
        TroopContext left = new TroopContext(){frontline=new List<Squad>(){new Squad()}};
        TroopContext right = new TroopContext(){stats=new Stats{block=1000}};
        Assert.AreEqual(new StrikeChance{head=1, body=4, arm=7, leg=10}, Strike.GetStrikeValues(left.stats, right.stats));
        TestStrikeHitCorrect(left, right, 100, BodyPart.Head);
        TestStrikeHitCorrect(left, right, 99, BodyPart.Body);
        TestStrikeHitCorrect(left, right, 96, BodyPart.Arm);
        TestStrikeHitCorrect(left, right, 93, BodyPart.Leg);
        TestStrikeHitCorrect(left, right, 90, BodyPart.Shield);
    }

    [TestMethod]
    public void TestStrikeOutcomeHead()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), new Hit(){area=BodyPart.Head}}
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(true, result[antagonist.frontline.First()].damaged);
    }

    [TestMethod]
    public void TestStrikeOutcomeBodyPowerGreater()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), new Hit(){area=BodyPart.Body}}
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(true, result[antagonist.frontline.First()].damaged);
        Assert.AreEqual(new Stats(), result[antagonist.frontline.First()].debuffs);
    }

    [TestMethod]
    public void TestStrikeOutcomeBodyArmourGreater()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{
            frontline=new List<Squad>(1){new Squad()},
            armour=new Armour{body=150}
        };
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), 
                        new Hit(){
                            area=BodyPart.Body,
                            deflectionRatio=1
                        }
                    }
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(false, result[antagonist.frontline.First()].damaged);
        Assert.AreEqual(new Stats{endurance=-5}, result[antagonist.frontline.First()].debuffs);
    }

    [TestMethod]
    public void TestStrikeOutcomeArm()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{
            frontline=new List<Squad>(1){new Squad()},
        };
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), 
                        new Hit(){
                            area=BodyPart.Arm,
                            deflectionRatio=1
                        }
                    }
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(false, result[antagonist.frontline.First()].damaged);
        Assert.AreEqual(new Stats{block=-5}, result[antagonist.frontline.First()].debuffs);
    }

    [TestMethod]
    public void TestStrikeOutcomeLeg()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{
            frontline=new List<Squad>(1){new Squad()},
        };
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), 
                        new Hit(){
                            area=BodyPart.Leg,
                            deflectionRatio=1
                        }
                    }
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(false, result[antagonist.frontline.First()].damaged);
        Assert.AreEqual(new Stats{formation=-5}, result[antagonist.frontline.First()].debuffs);
    }

    [TestMethod]
    public void TestStrikeOutcomeShield()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);
        TroopContext protagonist = new TroopContext{frontline=new List<Squad>(1){new Squad()}};
        TroopContext antagonist = new TroopContext{
            frontline=new List<Squad>(1){new Squad()},
        };
        var result = handler.GetOutcome(
            new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.First(), 
                        new Hit(){
                            area=BodyPart.Shield,
                            deflectionRatio=1
                        }
                    }
                }
            },
            protagonist,
            antagonist
        );
        Assert.AreEqual(false, result[antagonist.frontline.First()].damaged);
        Assert.AreEqual(new Stats(), result[antagonist.frontline.First()].debuffs);
    }

    [TestMethod]
    public void TestStrikeOutcomeAllHitArea()
    {
        var rng = new UnitTestRNG(rngInt: new List<int>(){50});
        ActionHandler handler = new ActionHandler(rng);

        TroopContext protagonist = new TroopContext{frontline=new List<Squad>()
            {
                new Squad(1),
                new Squad(2),
                new Squad(3),
                new Squad(4),
                new Squad(5),
            }
        };

        TroopContext antagonist = new TroopContext{frontline=new List<Squad>()
            {
                new Squad(1),
                new Squad(2),
                new Squad(3),
                new Squad(4),
                new Squad(5),
            }
        };
        Strike input = new Strike{
                hit= new Dictionary<Squad, Hit>(){
                    {protagonist.frontline.FirstOrDefault(t => t.column == 1), 
                        new Hit(){area=BodyPart.Head}},
                    {protagonist.frontline.FirstOrDefault(t => t.column == 2), 
                        new Hit(){
                            area=BodyPart.Body,
                            deflectionRatio=1
                        }
                    },
                    {protagonist.frontline.FirstOrDefault(t => t.column == 3), 
                        new Hit(){
                            area=BodyPart.Arm,
                            deflectionRatio=1
                        }
                    },
                    {protagonist.frontline.FirstOrDefault(t => t.column == 4), 
                        new Hit(){
                            area=BodyPart.Leg,
                            deflectionRatio=1
                        }
                    },
                    {
                        protagonist.frontline.FirstOrDefault(t => t.column == 5), 
                        new Hit(){
                            area=BodyPart.Shield,
                            deflectionRatio=1
                        }
                    }
                }
        };

        var result = handler.GetOutcome(input, protagonist, antagonist);
        
        Assert.AreEqual(true, result[antagonist.frontline.FirstOrDefault(t => t.column == 1)].damaged);
        Assert.AreEqual(new Stats(), result[antagonist.frontline.FirstOrDefault(t => t.column == 1)].debuffs);

        Assert.AreEqual(true, result[antagonist.frontline.FirstOrDefault(t => t.column == 2)].damaged);
        Assert.AreEqual(new Stats(), result[antagonist.frontline.FirstOrDefault(t => t.column == 2)].debuffs);

        Assert.AreEqual(false, result[antagonist.frontline.FirstOrDefault(t => t.column == 3)].damaged);
        Assert.AreEqual(new Stats{block=-5}, result[antagonist.frontline.FirstOrDefault(t => t.column == 3)].debuffs);

        Assert.AreEqual(false, result[antagonist.frontline.FirstOrDefault(t => t.column == 4)].damaged);
        Assert.AreEqual(new Stats{formation=-5}, result[antagonist.frontline.FirstOrDefault(t => t.column == 4)].debuffs);
        
        Assert.AreEqual(false, result[antagonist.frontline.FirstOrDefault(t => t.column == 5)].damaged);
        Assert.AreEqual(new Stats(), result[antagonist.frontline.FirstOrDefault(t => t.column == 5)].debuffs);
    }
}