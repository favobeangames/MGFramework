using System.Collections.Generic;
using System.Linq;
using FavobeanGames.MGFramework.DataStructures.Collections;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;

namespace FavobeanGames.MGFramework.Components;

public class GameWorld
{
    private List<EntitySystem> systems;
    private List<EntityUpdateSystem> updateSystems;
    private List<EntityDrawSystem> drawSystems;

    public readonly EntityList ActiveEntities;
    public readonly EntityList InactiveEntities;

    private World world;

    public GameWorld()
    {
        systems = new List<EntitySystem>();
        updateSystems = new List<EntityUpdateSystem>();
        drawSystems = new List<EntityDrawSystem>();

        ActiveEntities = new EntityList();
        InactiveEntities = new EntityList();

        world = new World();
    }

    public void AddSystem(EntitySystem system)
    {
        systems.Add(system);
        system.SetWorld(this);

        if (system is EntityUpdateSystem updateSystem)
        {
            updateSystems.Add(updateSystem);
        }

        if (system is EntityDrawSystem drawSystem)
        {
            drawSystems.Add(drawSystem);
        }
    }

    public void AddEntity(Entity entity)
    {
        ActiveEntities.Add(entity);
    }

    public void DestroyEntity(Entity entity)
    {
        ActiveEntities.Remove(entity);
        InactiveEntities.Remove(entity);
    }

    public void MoveEntityToActive(Entity entity)
    {
        var tmp = InactiveEntities.FirstOrDefault(e => e.Id == entity.Id);
        if (tmp is not null)
        {
            ActiveEntities.Add(tmp);
            InactiveEntities.Remove(tmp);
        }
        else
        {
            ActiveEntities.Add(entity);
        }
    }

    public void MoveEntityToInactive(Entity entity)
    {
        var tmp = ActiveEntities.FirstOrDefault(e => e.Id == entity.Id);
        if (tmp is not null)
        {
            InactiveEntities.Add(tmp);
            ActiveEntities.Remove(tmp);
        }
        else
        {
            InactiveEntities.Add(entity);
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (EntityUpdateSystem updateSystem in updateSystems)
        {
            updateSystem.Update(gameTime);
        }
    }

    public void Draw()
    {
        foreach (EntityDrawSystem drawSystem in drawSystems)
        {
            drawSystem.Draw(ActiveEntities);
        }
    }
}