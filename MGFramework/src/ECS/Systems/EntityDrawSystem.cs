namespace FavobeanGames.MGFramework.ECS;

public abstract class EntityDrawSystem: GameSystem, IDrawSystem
{
    protected EntityDrawSystem(AspectBuilder builder)
        : base(builder)
    {
    }

    public virtual void Draw() { }
}