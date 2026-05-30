using Godot;

public interface IRandom
{
	public float RandfRange(float left, float right);
	public int RandiRange(int left, int right);
}

public class GodotRandom : IRandom
{
	RandomNumberGenerator rng;
	public GodotRandom()
	{
		rng = new RandomNumberGenerator();

	}

    public float RandfRange(float left, float right)
    {
        return rng.RandfRange(left, right);
    }

    public int RandiRange(int left, int right)
    {
        return rng.RandiRange(left, right);
    }
}