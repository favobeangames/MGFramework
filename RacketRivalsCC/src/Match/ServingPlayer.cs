using MonoGame.Extended;

namespace RacketRivalsCC.Match;

public enum ServingSide
{
    Left,
    Right
}
public class ServingPlayer
{
    public readonly ServingSide ServingSide;
    public readonly RectangleF ServingBounds;

    public ServingPlayer(ServingSide servingSide, RectangleF servingBounds)
    {
        ServingSide = servingSide;
        ServingBounds = servingBounds;
    }
}