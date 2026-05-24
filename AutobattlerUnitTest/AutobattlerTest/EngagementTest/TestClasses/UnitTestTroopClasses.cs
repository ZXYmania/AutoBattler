using System.Collections.Generic;
using System.Linq;
using Moq;
using static Movement;
using static Order;
using static Phase;
using static Troop;

public struct UnitTestRNG : IRandom
{

    public List<float> rngFloat;
    public List<int> rngInt;
    public static List<float> defaultFloatList = new List<float>();

    public UnitTestRNG(List<float>? rngFloat = null, List<int>? rngInt = null)
    {
        if(rngFloat != null)
        {
            this.rngFloat = rngFloat;
        }
        if(rngInt != null)
        {
            this.rngInt = rngInt;
        }
    }

    public float RandfRange(float left, float right)
    {
        throw new NotImplementedException();
    }

    public int RandiRange(int left, int right)
    {
        var first = rngInt.First();
        rngInt.Remove(first);
        return first;
    }
}

public class TestEngagement : Engagement
{
    public static TestEngagement GenerateTestEngagement(Captain? leftCaptain = null, Captain? rightCaptain = null)
    {
        if(leftCaptain == null)
        {
            TestCaptain test = new TestCaptain();
            leftCaptain = test.GetCaptain();
        }
        if(rightCaptain == null)
        {
            TestCaptain test = new TestCaptain();
            rightCaptain = test.GetCaptain();
        }
        Troop leftTroop = new Troop(new Unit(new Stats(),leftCaptain, new List<UnitPerk>()));
        Troop rightTroop = new Troop(new Unit(new Stats(), rightCaptain, new List<UnitPerk>()));
        return new TestEngagement(leftTroop, rightTroop, true, new UnitTestRNG());
    }
    public TestEngagement(Troop leftTroop, Troop rightTroop, bool facing, IRandom rng) : base(leftTroop, rightTroop, facing, rng){}
}