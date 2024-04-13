using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

public class EntitySystem : UpdateSystem
{
    private const int entityPoolSize = 2056;
    private int nextEntityId;
    private Pool<Entity> entityPool;

    /// <summary>
    /// Collection of all available entities
    /// </summary>
    public readonly Bag<Entity> Entities;

    /// <summary>
    /// Collection of newly added entities to be placed into the world
    /// </summary>
    private Bag<int> addedEntities;

    /// <summary>
    /// Collection of entities that have recently been updated
    /// </summary>
    private Bag<int> updatedEntities;

    /// <summary>
    /// Collection of entities flagged to be destroyed
    /// </summary>
    private Bag<int> destroyedEntites;

    private ComponentService componentService;

    public int ActiveEntityCount;

    public event Action<int> EntityAdded;
    public event Action<int> EntityUpdated;
    public event Action<int> EntityDestroyed;

    public EntitySystem(ComponentService componentService)
    {
        this.componentService = componentService;
        this.componentService.ComponentsChanged += OnComponentsChanged;
        Entities = new Bag<Entity>(entityPoolSize);
        addedEntities = new Bag<int>(entityPoolSize);
        updatedEntities = new Bag<int>(entityPoolSize);
        destroyedEntites = new Bag<int>(entityPoolSize);

        entityPool = new Pool<Entity>(() => new Entity(nextEntityId++, this.componentService), entityPoolSize);
    }

    public void Destroy(int entityId)
    {
        if (!destroyedEntites.Contains(entityId))
            destroyedEntites.Add(entityId);
    }

    public void Destroy(Entity entity)
    {
        Destroy(entity.Id);
    }

    public Entity Get(int entityId)
    {
        return Entities[entityId];
    }

    public Entity Create()
    {
        var entity = entityPool.Obtain();
        var id = entity.Id;
        Entities.Add(entity);
        addedEntities.Add(id);
        return entity;
    }

    public void OnComponentsChanged(int entityId)
    {
        updatedEntities.Add(entityId);
        EntityUpdated?.Invoke(entityId);
    }
    public override void Update(GameTime gameTime)
    {
        foreach (var entityId in addedEntities)
        {
            ActiveEntityCount++;
            EntityAdded?.Invoke(entityId);
        }

        foreach (var entityId in updatedEntities)
        {
            EntityUpdated?.Invoke(entityId);
        }

        foreach (var entityId in destroyedEntites)
        {
            EntityDestroyed?.Invoke(entityId);

            var entity = Entities[entityId];
            Entities[entityId] = null;
            componentService.Destroy(entityId);
            ActiveEntityCount--;
            entityPool.Free(entity);
        }

        addedEntities.Clear();
        updatedEntities.Clear();
        destroyedEntites.Clear();
    }
}