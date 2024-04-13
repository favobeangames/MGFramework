using System;
using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

/**
 * Entity is the base class for all game components.
 */
public class Entity
{
    /// <summary>
    /// Id value for the Entity
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Reference to the component service of the world
    /// </summary>
    private readonly ComponentService componentService;

    public Entity() { }
    public Entity(int id, ComponentService componentService)
    {
        Id = id;
        this.componentService = componentService;
    }

    public void AttachComponent<T>(T component)
        where T : class
    {
        var mapper = componentService.GetMapper<T>();
        mapper.Put(Id, component);
    }

    public T Get<T>()
        where T : class
    {
        var mapper = componentService.GetMapper<T>();
        return mapper.Get(Id);
    }

    public bool Has<T>()
        where T : class
    {
        var mapper = componentService.GetMapper<T>();
        return mapper.Has(Id);
    }

    public void Delete<T>()
        where T : class
    {
        var mapper = componentService.GetMapper<T>();
        mapper.Delete(Id);
    }
}