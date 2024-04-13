using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

public interface IComponentService
{
    ComponentMapper<T> GetMapper<T>() where T : class;
}

public class ComponentService : IComponentService
{
    private int componentMapperCapacity = 256;
    private Bag<ComponentMapper> componentMappers;
    private Dictionary<Type, int> componentTypes;

    public Action<int> ComponentsChanged;

    public ComponentService()
    {
        componentMappers = new Bag<ComponentMapper>(componentMapperCapacity);
        componentTypes = new Dictionary<Type, int>();
    }

    public ComponentMapper<T> CreateComponentMapper<T>(int componentId)
        where T : class
    {
        var mapper = new ComponentMapper<T>(componentId, ComponentsChanged);
        componentMappers[componentId] = mapper;
        return mapper;
    }

    public ComponentMapper GetMapper(int componentId)
    {
        return componentMappers[componentId];
    }

    public ComponentMapper<T> GetMapper<T>()
        where T : class
    {
        var componentId = GetComponentId(typeof(T));

        if (componentMappers[componentId] != null)
        {
            return componentMappers[componentId] as ComponentMapper<T>;
        }

        return CreateComponentMapper<T>(componentId);
    }

    public int GetComponentId(Type type)
    {
        if (componentTypes.TryGetValue(type, out var id))
            return id;

        id = componentTypes.Count;
        componentTypes.Add(type, id);
        return id;
    }

    public void Destroy(int entityId)
    {
        foreach (var mapper in componentMappers)
        {
            mapper?.Delete(entityId);
        }
    }

    public bool EntityContainsAllComponents(int entityId, IEnumerable<Type> types)
    {
        var tmp = componentMappers.Where(m => types.Contains(m.Type)).ToArray();
        return tmp.Length == types.Count() && tmp.All(mapper => mapper.Has(entityId));
    }

    public bool EntityContainsAnyComponents(int entityId, IEnumerable<Type> types)
    {
        var tmp = componentMappers.Where(m => types.Contains(m.Type)).ToArray();
        return tmp.Any(mapper => mapper.Has(entityId));
    }

    public bool EntityContainsNoComponents(int entityId, IEnumerable<Type> types)
    {
        var tmp = componentMappers.Where(m => types.Contains(m.Type)).ToArray();
        return !tmp.Any(mapper => mapper.Has(entityId));
    }
}