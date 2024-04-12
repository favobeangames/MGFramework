using System.Collections.Generic;
using FavobeanGames.MGFramework.ECS;
using MonoGame.Extended;
using MonoGame.Extended.Collections;

namespace RacketRivalsCC.Screens;

/// <summary>
/// Tells the game manager what type of screen is
/// </summary>
public enum ScreenType
{
    World,
    Match
}

public class RRGameScreen : GameScreen
{
    public readonly ScreenType ScreenType;

    protected Bag<Entity> entities;

    public RRGameScreen(RectangleF bounds, ScreenType type)
     : base(bounds)
    {
        ScreenType = type;
        entities = new Bag<Entity>(256);
    }

    public override IEnumerable<int> LoadScreen(GameWorld gameWorld)
    {
        throw new System.NotImplementedException();
    }

    public override void UnloadScreen(GameWorld gameWorld)
    {
        throw new System.NotImplementedException();
    }
}