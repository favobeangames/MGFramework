namespace RacketRivalsCC.Match;

public class MatchPlayer
{
    /// <summary>
    /// The "team" that the player resides on in the match.
    /// Essentially which side that the player is on
    /// 0 or 1
    /// </summary>
    public int Team;

    /// <summary>
    /// Player is currently serving in the match
    /// </summary>
    public bool Serving;

    /// <summary>
    /// Player has thrown serve up
    /// </summary>
    public bool ThrownServe;

    /// <summary>
    /// Player has served the ball
    /// </summary>
    public bool HasServed;

    /// <summary>
    /// Player is currently diving
    /// </summary>
    public bool Diving;

    /// <summary>
    /// Player is currently sliding
    /// </summary>
    public bool Sliding;
}