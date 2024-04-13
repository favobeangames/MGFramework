using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.DataStructures.Collections;
using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.ECS;

public class GameWorld
{
    private int entityCount = 0;

    private List<IGameSystem> systems;
    private List<IUpdateSystem> updateSystems;
    private List<IDrawSystem> drawSystems;

    internal readonly ComponentService ComponentService;
    internal readonly EntitySystem EntitySystem;

    public GameWorld()
    {
        systems = new List<IGameSystem>();
        updateSystems = new List<IUpdateSystem>();
        drawSystems = new List<IDrawSystem>();

        ComponentService = new ComponentService();
        EntitySystem = new EntitySystem(ComponentService);

        AddSystem(EntitySystem);
    }

    public void AddSystem(IGameSystem system)
    {
        systems.Add(system);

        if (system is IUpdateSystem updateSystem)
        {
            updateSystems.Add(updateSystem);
        }

        if (system is IDrawSystem drawSystem)
        {
            drawSystems.Add(drawSystem);
        }

        system.Initialize(this);
    }

    /// <summary>
    /// Returns the entity associated with the ID
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns>Reference of the entity</returns>
    public Entity GetEntity(int entityId)
    {
        return EntitySystem.Get(entityId);
    }

    /// <summary>
    /// Creates a new entity and adds it to the Active entity list
    /// </summary>
    /// <returns>Reference of the entity created</returns>
    public Entity CreateEntity()
    {
        return EntitySystem.Create();
    }

    /// <summary>
    /// Destroys entity from the game world
    /// </summary>
    /// <param name="entityId"></param>
    public void DestroyEntity(int entityId)
    {
        EntitySystem.Destroy(entityId);
    }

    /// <summary>
    /// Destroys entity from the game world
    /// </summary>
    /// <param name="entity"></param>
    public void DestroyEntity(Entity entity)
    {
        DestroyEntity(entity.Id);
    }

    public void Update(GameTime gameTime)
    {
        foreach (IUpdateSystem updateSystem in updateSystems)
        {
            updateSystem.Update(gameTime);
        }
    }

    public void Draw()
    {
        foreach (IDrawSystem drawSystem in drawSystems)
        {
            drawSystem.Draw();
        }
    }
}