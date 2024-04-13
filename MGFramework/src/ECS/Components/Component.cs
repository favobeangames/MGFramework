using System;

namespace FavobeanGames.MGFramework.ECS;

public abstract class Component
{
    public Type Type { get; protected set; }
}

public class Component<T> : Component
    where T : class
{
    public readonly T Value;

    public Component(T value)
    {
        Value = value;
        Type = value?.GetType();
    }
}