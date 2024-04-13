using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.ECS;

public abstract class UpdateSystem : IUpdateSystem
{
    public virtual void Initialize(GameWorld gameWorld) { }
    public abstract void Update(GameTime gameTime);
}