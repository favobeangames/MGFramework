using System;

namespace FavobeanGames.MGFramework.ECS;

public class Component<T>
{
    public readonly T Value;
    public readonly Type Type;

    public Component(T value)
    {
        Value = value;
        Type = value?.GetType();
    }
}