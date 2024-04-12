namespace RacketRivalsCC;

public class Stats
{
    public float Speed { get; private set; }
    public float Spin { get; private set; }
    public float Power { get; private set; }

    public void SetSpeed(float value)
    {
        // TODO: Clamp these to some values when we figure those out
        Speed = value;
    }

    public void SetSpin(float value)
    {
        // TODO: Clamp these to some values when we figure those out
        Spin = value;
    }

    public void SetPower(float value)
    {
        // TODO: Clamp these to some values when we figure those out
        Power = value;
    }
}