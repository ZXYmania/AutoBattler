

[TestClass]
public sealed class StrikePOC
{
    public struct AppendagePOC
    {
        public int head;
        public int body;
        public int arm;
        public int leg;
        public int shield;
    }

    public enum WoundResultPOC
    {
        Head,
        body,
        arm,
        leg,
        shield
    }

    public static float GetModifierPOC(int strike, int block)
    {
        return (int.Clamp(strike - block, -600, 600)/800f) + 1;
    }

    [TestMethod]
    public void TestModifiedPOC()
    {
        Assert.AreEqual(1.75, GetModifierPOC(600,0));
        Assert.AreEqual(0.25, GetModifierPOC(0, 600));
        Assert.AreEqual(1, GetModifierPOC(0,0));
        var test = new List<AppendagePOC>(){
            GetPOC(GetModifierPOC(600,0)),
            GetPOC(GetModifierPOC(1200,600)),
            GetPOC(GetModifierPOC(0,600)),
            GetPOC(GetModifierPOC(350,350))
        };
    }

    public AppendagePOC GetPOC(float modifier)
    {
        return new AppendagePOC{ 
            head = (int) MathF.Floor(10*modifier), 
            body = (int) MathF.Floor(30*modifier),
            arm = (int) MathF.Floor(30*modifier),
            leg = (int) MathF.Floor(30*modifier),
            shield = int.Clamp(100-(int) MathF.Floor(100*modifier),0, 100)
        };
    }

    [TestMethod]
    public void TestAppendageValues()
    {
        List<AppendagePOC> appendageRates = new List<AppendagePOC>();
        Random rng = new Random();
        List<List<int>> woundResult = new List<List<int>>();

        List<float> percentage = new List<float>()
        {
            2f, 1.875f, 1.75f, 1.5f, 1.2f, 1.1f, 1f, 0.9f, 0.8f, 0.75f, 0.70f, 0.6f, 0.5f, 0.4f, 0.3f, 0.25f, 0.125f, 
        };
  
        foreach(float modifier in percentage)
        {
            AppendagePOC currentAppendage = new AppendagePOC{ 
                                    head = (int) MathF.Floor(10*modifier), 
                                    body = (int) MathF.Floor(30*modifier),
                                    arm = (int) MathF.Floor(30*modifier),
                                    leg = (int) MathF.Floor(30*modifier),
                                    shield = 100-(int) MathF.Floor(100*modifier)
            };
            appendageRates.Add(currentAppendage);

            List<int> output = new List<int>(){0,0,0,0,0,0,0};
            for(int r = 0; r < 1000; r++ )
            {
                Dictionary<WoundResultPOC, int> currentWoundResult = new Dictionary<WoundResultPOC, int>()
                {
                    {WoundResultPOC.Head, 0},
                    {WoundResultPOC.body, 0},
                    {WoundResultPOC.arm, 0},
                    {WoundResultPOC.leg, 0},
                    {WoundResultPOC.shield, 0},
                };
                for(int i = 0; i < 6; i++)
                {           

                    int result = (int)rng.NextInt64(1,100);
                    if( result < currentAppendage.head)
                    {
                        currentWoundResult[WoundResultPOC.Head]++;
                    }
                    else if( result < currentAppendage.body + currentAppendage.head)
                    {
                        currentWoundResult[WoundResultPOC.body]++;
                    }
                    else if( result < currentAppendage.arm + currentAppendage.body + currentAppendage.head)
                    {
                        currentWoundResult[WoundResultPOC.arm]++;
                    }
                    else if(result < currentAppendage.leg + currentAppendage.arm + currentAppendage.body + currentAppendage.head)
                    {
                        currentWoundResult[WoundResultPOC.leg]++;
                    }
                    else
                    {
                        currentWoundResult[WoundResultPOC.shield]++;
                    }
                    
                }

                if(currentWoundResult[WoundResultPOC.Head] > 1)
                {
                    output[0]++;
                }
                if(currentWoundResult[WoundResultPOC.Head] >= 1)
                {
                    output[1]++;
                }
                output[2] += currentWoundResult[WoundResultPOC.Head];
                output[3] += currentWoundResult[WoundResultPOC.body];
                output[4] += currentWoundResult[WoundResultPOC.arm] + currentWoundResult[WoundResultPOC.leg];
                if(currentWoundResult[WoundResultPOC.shield] >= 3)
                {
                    output[5]++;
                }
                output[6] += currentWoundResult[WoundResultPOC.shield];

            }
            woundResult.Add(output);
        }
    }
}



// 2 Modifier
// 80% Potential Lethal
// 20% NonLethal Hit
// 0% Shield

// 1.875
// 75%(18%-57%) Potential lethal Hit
// Nonlethal Hit 25%
// 0% Shield

//  1.75
// 69%(16% - 53%) potential lethal
// 31% nonlethal hit
// 0% shield

// 1.5 Modifier
// 50% potential lethal
// 50% Nonlethal hit
// 0% Shield

// 1 Modifier
// 40% Potential Lethal
// 60% Nonlethal hit
// 0% Shield

// 0.75 Modifier
// 28% Potential Lethal
// 45% NonLethal Hit
// 27% Shield

// 0.5 Modifier
// 20% Potential Lethal
// 30% Nonlethal hit
// Shield 50%

// 0.25
// 9% Potential lethal
// 14% Nonlethal Hit
// 77% Shield

// 0.125 Modifier
// 5% Potential lethal
// 16% Nonlethal
// 90% Shield