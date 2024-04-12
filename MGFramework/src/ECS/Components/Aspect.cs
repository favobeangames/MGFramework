using System.Collections.Generic;

namespace FavobeanGames.MGFramework.ECS;

public class Aspect
{
    public readonly List<IComponent> Components;

    public Aspect()
    {
        Components = new List<IComponent>();
    }
}