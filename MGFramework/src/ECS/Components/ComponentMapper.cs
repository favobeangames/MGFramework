using System;
using System.Linq;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

public abstract class ComponentMapper
{
    public int Id { get; }
    public Type Type { get; }

    protected ComponentMapper(int id, Type type)
    {
        Id = id;
        Type = type;
    }

    public abstract bool Has(int entityId);
    public abstract void Delete(int entityId);
}

public class ComponentMapper<T> : ComponentMapper
    where T : class
{
    public event Action<int> OnPut;
    public event Action<int> OnDelete;

    private readonly Action<int> onComponentChanged;
    public Bag<T> Components { get; }

    public ComponentMapper(int id, Action<int> onComponentChanged)
        : base(id, typeof(T))
    {
        Components = new Bag<T>();
        this.onComponentChanged = onComponentChanged;
    }

    public void Put(int entityId, T component)
    {
        Components[entityId] = component;
        onComponentChanged(entityId);
        OnPut?.Invoke(entityId);
    }

    public T Get(Entity entity)
    {
        return Get(entity.Id);
    }

    public T Get(int entityId)
    {
        return Components[entityId];
    }

    public override bool Has(int entityId)
    {
        if (entityId >= Components.Count)
            return false;

        return Components[entityId] != null;
    }

    public override void Delete(int entityId)
    {
        Components[entityId] = null;
        onComponentChanged(entityId);
        OnDelete?.Invoke(entityId);
    }
}