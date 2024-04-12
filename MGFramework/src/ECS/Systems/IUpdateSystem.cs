using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework;

public interface IUpdateSystem : IGameSystem
{
    public void Update(GameTime gameTime);
}