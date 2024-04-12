namespace FavobeanGames.MGFramework.Components;

public class GameWorldBuilder
{
    private GameWorld gameWorld;

    public GameWorldBuilder()
    {
        gameWorld = new GameWorld();
    }

    public GameWorldBuilder AddSystem(EntitySystem gameSystem)
    {
        gameWorld.AddSystem(gameSystem);
        return this;
    }

    public GameWorld Build()
    {

        return gameWorld;
    }
}