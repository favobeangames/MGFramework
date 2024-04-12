using System.Collections.Generic;
using System.Linq;

namespace FavobeanGames.MGFramework.ECS;

public class GameSystem : IGameSystem
{
    protected GameWorld gameWorld;
    protected List<Entity> activeEntities;

    /// <summary>
    /// Aspects store the types of components that we want the entities
    /// of the system to deal with
    /// </summary>
    private readonly Aspect entityAspect;

    protected EntitySystem()
    {
        activeEntities = new List<Entity>();
        entityAspect = new Aspect();
    }
    protected EntitySystem(Aspect aspect)
    {
        activeEntities = new List<Entity>();
        entityAspect = aspect;
    }

    public void SetWorld(GameWorld world)
    {
        gameWorld = world;
    }

    /// <summary>
    /// Updates the systems list of active entities.
    /// Used on initialization and when entities are updated
    /// </summary>
    /// <param name="entities"></param>
    public void SetActiveEntities(IEnumerable<Entity> entities)
    {
        var updated = entities?
            .Where(EntityContainsAspectComponents)
            .ToList();

        activeEntities = updated;
    }

    /// <summary>
    /// Checks to see if the entity contains a component contained in the systems aspect
    /// Used to filter the game worlds entities into a subset that this system cares about
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    private bool EntityContainsAspectComponents(Entity entity)
    {
        var c = entity.Components.FindAll(c => entityAspect.Components.Any(ct => ct == c.Type));
        return c?.Count > 0;
    }

    /// <summary>
    /// Creates a new entity object in the world and returns it
    /// </summary>
    /// <returns>New Entity object</returns>
    protected Entity CreateEntity()
    {
        return gameWorld.CreateEntity();
    }
}