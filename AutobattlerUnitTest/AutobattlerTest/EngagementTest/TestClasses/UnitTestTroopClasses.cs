using System.Collections.Generic;
using System.Linq;
using Moq;
using static Movement;
using static Order;
using static Phase;
using static Troop;

public struct MockUnitTestRNG : IRandom
{

    public List<float> rngFloat;
    public List<int> rngInt;
    public static List<float> defaultFloatList = new List<float>();

    public MockUnitTestRNG(List<float>? rngFloat = null, List<int>? rngInt = null)
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
public struct UnitTestRNG : IRandom
{
    Random rng;
    List<List<double>> results;
    int index;
    public UnitTestRNG()
    {
        rng = new Random();
        results = new List<List<double>>(){new List<double>()};
        index = 0;
    }
    public float RandfRange(float left, float right)
    {
        return ((float)rng.NextDouble()*(right-left))+left;
    }

    public int RandiRange(int left, int right)
    {
        double next = rng.NextDouble();
        if(results[index].Count < 5)
        {
            results[index].Add(next);
        }
        else
        {
            results.Add(new List<double>(){next});
            index++;
        }
        return (int)MathF.Floor(((float)next*(right-left))+left);
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
        return new TestEngagement(leftTroop, rightTroop, true, new MockUnitTestRNG());
    }
    public TestEngagement(Troop leftTroop, Troop rightTroop, bool facing, IRandom rng) : base(leftTroop, rightTroop, facing, rng){}
}