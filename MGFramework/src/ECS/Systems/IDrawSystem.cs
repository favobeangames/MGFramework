using FavobeanGames.MGFramework.DataStructures.Collections;

namespace FavobeanGames.MGFramework;

public interface IDrawSystem : IGameSystem
{
    public void Draw(params Entity[] entities);
    public void Draw(EntityList entities);
}