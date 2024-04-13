using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.ECS;

public interface IUpdateSystem : IGameSystem
{
    public void Update(GameTime gameTime);
}