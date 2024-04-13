using System;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.ECS;

public abstract class EntityUpdateSystem : GameSystem, IUpdateSystem
{
    public EntityUpdateSystem(AspectBuilder builder)
        : base(builder)
    {
    }
    public virtual void Update(GameTime gameTime) { }
}