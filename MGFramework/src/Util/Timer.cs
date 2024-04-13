namespace FavobeanGames.MGFramework.Util;

public class Timer
{
    public bool Finished;
    public bool Started;

    private float elapsed;
    private float duration;

    public Timer()
    {

    }

    public void Start(float duration)
    {
        elapsed = 0;
        this.duration = duration;
        Finished = false;
        Started = false;
    }

    public void Stop()
    {
        elapsed = duration;
        Finished = true;
    }

    public void Reset()
    {
        elapsed = 0;
        Finished = false;
    }

    public void Reset(float newDuration)
    {
        elapsed = 0;
        duration = newDuration;
        Finished = false;
    }

    public void Update(float elapsedTime)
    {
        if (Finished) return;

        elapsed += elapsedTime;
        if (elapsed >= duration)
            Stop();
    }
}