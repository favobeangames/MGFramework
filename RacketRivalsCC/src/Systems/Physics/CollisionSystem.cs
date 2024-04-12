using FavobeanGames.MGFramework.ECS;
using FavobeanGames.MGFramework.Physics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace RacketRivalsCC.Systems;

public class CollisionSystem : EntityUpdateSystem
{
    private World world;
    private int stepIterations;

    private ComponentMapper<RigidBody> bodyMapper;
    private ComponentMapper<GameObject> gameObjectMapper;

    public CollisionSystem(Vector2 gravity, int stepIterations, WorldOptions worldOptions)
        : base(Aspect.All(typeof(GameObject)).Any(typeof(RigidBody)))
    {
        this.stepIterations = stepIterations;
        world = new World(gravity, worldOptions);
    }

    public override void Update(GameTime gameTime)
    {
        if (ActiveEntities.Count > 0)
        {
            var elapsedSeconds = gameTime.GetElapsedSeconds();
            world.Step(elapsedSeconds, stepIterations);

            foreach (var entityId in ActiveEntities)
            {
                
            }
        }
    }

    protected override void Initialize(IComponentService componentService)
    {
        bodyMapper = componentService.GetMapper<RigidBody>();
        gameObjectMapper = componentService.GetMapper<GameObject>();
    }

    protected override void OnEntityAdded(int entityId)
    {
        base.OnEntityAdded(entityId);
        if (ActiveEntities[entityId] == null) return;

        var body = bodyMapper.Get(entityId);
        if (body == null)
            return;
        var gameObj = gameObjectMapper.Get(entityId);

        if (gameObj != null)
            body.BodyResolveCheck += OnBodyCollisionResolution;

        world.AddBody(body);
    }

    protected override void OnEntityDestroyed(int entityId)
    {
        base.OnEntityDestroyed(entityId);
        var body = bodyMapper.Get(entityId);
        if (body != null)
            world.RemoveBody(body);
    }

    private bool OnBodyCollisionResolution(Manifold contact)
    {
        var gameObj1 = gameObjectMapper.Get(contact.BodyA.Id);
        var gameObj2 = gameObjectMapper.Get(contact.BodyB.Id);

        return gameObj1?.HeightOffGround < gameObj2?.Height && gameObj2?.HeightOffGround < gameObj1?.Height;
    }
}