using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework;

public class EntityUpdateSystem : EntitySystem, IUpdateSystem
{
    public virtual void Update(GameTime gameTime) { }
}