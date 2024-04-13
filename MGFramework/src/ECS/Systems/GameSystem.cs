using MonoGame.Extended.Collections;

namespace FavobeanGames.MGFramework.ECS;

public abstract class GameSystem : IGameSystem
{
    private const int entityCapacity = 128;

    protected GameWorld gameWorld;
    protected EntitySystem entitySystem;

    private bool entitiesUpdated;
    private Bag<int> activeEntities;

    public Bag<int> ActiveEntities
    {
        get
        {
            if (entitiesUpdated)
                UpdateActiveEntities();

            return activeEntities;
        }
    }

    /// <summary>
    /// Aspects store the types of components that we want the entities
    /// of the system to deal with
    /// </summary>
    private Aspect entityAspect;

    private AspectBuilder aspectBuilder;

    protected GameSystem()
    {
        activeEntities = new Bag<int>(entityCapacity);
    }
    protected GameSystem(AspectBuilder builder)
    {
        activeEntities = new Bag<int>(entityCapacity);
        aspectBuilder = builder;
    }

    public virtual void Initialize(GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        if (aspectBuilder == null)
        {
            entityAspect = new Aspect();
        }
        else
        {
            entityAspect = aspectBuilder.Build();
        }
        entityAspect.Initialize(gameWorld.ComponentService);
        entitySystem = gameWorld.EntitySystem;

        entitySystem.EntityAdded += OnEntityAdded;
        entitySystem.EntityUpdated += OnEntityUpdated;
        entitySystem.EntityDestroyed += OnEntityDestroyed;

        Initialize(gameWorld.ComponentService);
    }

    protected abstract void Initialize(IComponentService componentService);

    /// <summary>
    /// Updates the systems list of active entities.
    /// Used on initialization and when entities are updated
    /// </summary>
    /// <param name="entities"></param>
    public void UpdateActiveEntities()
    {
        activeEntities.Clear();

        foreach (var entity in entitySystem.Entities)
        {
            var id = entity.Id;
            if (entityAspect.CheckValidEntity(id))
                activeEntities.Add(id);
        }
    }

    protected Entity GetEntity(int entityId) => gameWorld.GetEntity(entityId);
    protected Entity CreateEntity() => gameWorld.CreateEntity();
    protected void DestroyEntity(int entityId) => gameWorld.DestroyEntity(entityId);

    protected virtual void OnEntityAdded(int entityId)
    {
        if (entityAspect.CheckValidEntity(entityId))
            activeEntities.Add(entityId);
    }
    protected virtual void OnEntityUpdated(int entityId) => entitiesUpdated = true;
    protected virtual void OnEntityDestroyed(int entityId) => entitiesUpdated = true;
}