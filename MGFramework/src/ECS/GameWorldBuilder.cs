namespace FavobeanGames.MGFramework.ECS;

public class GameWorldBuilder
{
    private GameWorld gameWorld;

    public GameWorldBuilder()
    {
        gameWorld = new GameWorld();
    }

    public GameWorldBuilder AddSystem(IGameSystem gameSystem)
    {
        gameWorld.AddSystem(gameSystem);
        return this;
    }

    public GameWorld Build()
    {

        return gameWorld;
    }
}